using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : Character
{
    public int zCost;
    public ZombieEnum zombieEnum;
    [SerializeField] private Animator zombieAnimator;
    [SerializeField] private HumanScript closest;
    private NavMeshAgent navMeshAgent;
    private float minDistance = float.MaxValue;
    private float timer = 0;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        navMeshAgent.speed = speed;
    }

    private void Update()
    {
        if (lifePoints <= 0)
        {
            if (!isDying)
            {
                StartCoroutine(Death());
            }
        }
        else
        {
            if (GameManager.Instance.currentLevelGestion.humansToKill.Count > 0)
            {
                timer += Time.deltaTime;

                if (timer > 1.5f)
                {
                    FindNearestTarget();
                }

                if (closest != null)
                {
                    //Walking/Running to the target
                    if (Vector3.Distance(transform.position, closest.gameObject.transform.position) > attackDistance)
                    {
                        navMeshAgent.isStopped = false;
                        zombieAnimator.SetBool("Walking", true);
                        zombieAnimator.SetBool("Attacking", false);
                        var dir = closest.gameObject.transform.position - transform.position;
                        var dirNormalized = dir.normalized;
                        var enemyPoint = transform.position + (2f * dirNormalized);
                        navMeshAgent.destination = enemyPoint;

                        if (closest.isDying)
                        {
                            closest = null;
                            closest = FindNearestTarget();
                        }
                    }
                    else
                    //Close to the target and attacks
                    {
                        navMeshAgent.isStopped = true;
                        zombieAnimator.SetBool("Walking", false);
                        zombieAnimator.SetBool("Attacking", true);
                        transform.LookAt(new Vector3(closest.transform.position.x, transform.position.y, closest.transform.position.z));

                        if (closest.isDying)
                        {
                            closest = null;
                            closest = FindNearestTarget();
                        }
                        else
                        {
                            attackCooldown += Time.deltaTime;
                            if (attackCooldown > attackSpeed)
                            {
                                closest.Hit(damageDealt);
                                attackCooldown = 0;
                            }
                        }
                    }
                }
                else
                {
                    minDistance = float.MaxValue;
                    attackCooldown = attackSpeed;
                    closest = FindNearestTarget();
                    zombieAnimator.SetBool("Attacking", false);
                }
            }
            else
            {
                zombieAnimator.SetBool("Attacking", false);
            }
        }
    }

    private HumanScript FindNearestTarget()
    {
        HumanScript close = null;
        if (GameManager.Instance.currentLevelGestion.humansToKill.Count > 0)
        {
            foreach (var human in GameManager.Instance.currentLevelGestion.humansToKill)
            {
                var difference = Mathf.Abs(Vector3.Distance(transform.position, human.gameObject.transform.position));
                if (difference < minDistance)
                {
                    minDistance = difference;
                    close = human;
                }
            }
        }

        if (GameManager.Instance.currentLevelGestion.humansToKill.Count == 1)
        {
            close = GameManager.Instance.currentLevelGestion.humansToKill[0];
            return close;
        }

        return close;
    }

    private IEnumerator Death()
    {
        isDying = true;
        navMeshAgent.isStopped = true;
        zombieAnimator.SetBool("Death", true);
        AudioManager.Instance.Play("ZombieDeath");
        GameManager.Instance.playerZombies.Remove(this);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
