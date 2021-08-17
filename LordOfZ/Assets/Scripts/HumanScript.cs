using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanScript : Character
{
    public HumanEnum humanEnum;

    [SerializeField] private Animator animator;
    private List<ZombieScript> zombiesNearby = new List<ZombieScript>();
    private float checkForTargetsTimer = 0;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private float checkForTargetsDuration = 1.5f;
    [SerializeField] private Transform lineOfSight;
    [SerializeField] private LayerMask zombieLayer;

    private float minDistance = float.MaxValue;
    [SerializeField] private ZombieScript closestZombie;
    [SerializeField] private Transform[] wayPoints;
    public bool startMoving;
    private int wayPointIndex;

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
        //Still alive
        else
        {
            if (humanEnum == HumanEnum.Melee)
            {
                checkForTargetsTimer += Time.deltaTime;

                if (checkForTargetsTimer > checkForTargetsDuration)
                {
                    if (closestZombie != null)
                    {
                        if (Vector3.Distance(transform.position, closestZombie.transform.position) > attackDistance)
                        {
                            closestZombie = null;
                            closestZombie = FindNearestZombie();
                        }
                    }
                    checkForTargetsTimer = 0;
                }

                AttackZombie();
            }

            if (humanEnum == HumanEnum.Gunner)
            {
                checkForTargetsTimer += Time.deltaTime;

                if (checkForTargetsTimer > checkForTargetsDuration)
                {
                    checkForTargetsTimer = 0;
                }

                AttackZombie();
            }

            if (startMoving)
            {
                if (wayPoints.Length > 0 && speed > 0)
                {
                    animator.SetBool("Walking", true);
                    if (wayPointIndex < wayPoints.Length)
                    {
                        navMeshAgent.SetDestination(wayPoints[wayPointIndex].position);
                        transform.LookAt(new Vector3(wayPoints[wayPointIndex].position.x, transform.position.y, wayPoints[wayPointIndex].position.z));
                        if (Vector3.Distance(transform.position, wayPoints[wayPointIndex].position) < .5f)
                        {
                            wayPointIndex++;
                        }
                    }
                    else
                    {
                        wayPointIndex = 0;
                    }
                }
            }
        }
    }

    private void AttackZombie()
    {
        if (GameManager.Instance.playerZombies.Count > 0)
        {
            if (closestZombie != null)
            {
                if (Vector3.Distance(transform.position, closestZombie.transform.position) < attackDistance)
                {
                    transform.LookAt(new Vector3(closestZombie.gameObject.transform.position.x, transform.position.y, closestZombie.gameObject.transform.position.z));

                    if (speed > 0)
                    {
                        animator.SetBool("Walking", false);
                    }
                    if (humanEnum == HumanEnum.Melee)
                    {
                        animator.SetBool("Attacking", true);
                        navMeshAgent.isStopped = true;
                        attackCooldown += Time.deltaTime;
                        if (attackCooldown > attackSpeed)
                        {
                            closestZombie.Hit(damageDealt);
                            attackCooldown = 0;
                        }

                        if (closestZombie.isDying)
                        {
                            closestZombie = FindNearestZombie();
                        }
                    }

                    if (humanEnum == HumanEnum.Gunner)
                    {
                        if (Physics.Raycast(lineOfSight.transform.position, lineOfSight.transform.forward, out RaycastHit hit, attackDistance / 2, zombieLayer))
                        {
                            if (hit.collider.gameObject.GetComponent<ZombieScript>() != null)
                            {
                                animator.SetBool("Attacking", true);
                                navMeshAgent.isStopped = true;
                                attackCooldown += Time.deltaTime;
                                if (attackCooldown > attackSpeed)
                                {
                                    closestZombie.Hit(damageDealt);
                                    attackCooldown = 0;
                                }
                            }

                            if (closestZombie.isDying)
                            {
                                closestZombie = FindNearestZombie();
                            }
                        }
                    }
                }
                else
                {
                    navMeshAgent.isStopped = false;
                    attackCooldown = attackSpeed;
                    if (speed > 0)
                    {
                        animator.SetBool("Walking", true);
                        if (humanEnum == HumanEnum.Melee)
                        {
                            if (wayPoints.Length <= 0)
                            {
                                if (!Physics.Raycast(lineOfSight.transform.position, lineOfSight.transform.forward, out RaycastHit hit, attackDistance, zombieLayer))
                                {
                                    var dir = closestZombie.gameObject.transform.position - transform.position;
                                    var dirNormalized = dir.normalized;
                                    var enemyPoint = transform.position + (1.5f * dirNormalized);
                                    navMeshAgent.destination = enemyPoint;
                                }
                            }
                        }
                    }

                    animator.SetBool("Attacking", false);
                }
            }
            else
            {
                minDistance = float.MaxValue;
                attackCooldown = attackSpeed;
                closestZombie = FindNearestZombie();
                animator.SetBool("Attacking", false);
            }
        }
        else
        {
            animator.SetBool("Attacking", false);
        }
    }

    private ZombieScript FindNearestZombie()
    {
        if (GameManager.Instance.playerZombies.Count > 0)
        {
            foreach (var zombie in GameManager.Instance.playerZombies)
            {
                var difference = Mathf.Abs(Vector3.Distance(transform.position, zombie.gameObject.transform.position));
                if (difference < minDistance)
                {
                    minDistance = difference;
                    closestZombie = zombie;
                }
            }
        }
        else if (GameManager.Instance.playerZombies.Count == 1)
        {
            closestZombie = GameManager.Instance.playerZombies[0];
        }

        return closestZombie;
    }

    public void Reset()
    {
        lifePoints = maxLifePoints;
        isDying = false;
        GetComponentInChildren<Canvas>().GetComponentInChildren<LifeBarScript>().HandleHealthChanged(lifePoints);
        animator.SetBool("Death", false);
    }

    private IEnumerator Death()
    {
        isDying = true;
        animator.SetBool("Death", true);
        switch (humanEnum)
        {
            case (HumanEnum)0:
                AudioManager.Instance.Play("Woman_Pain");
                break;
            case (HumanEnum)1:
                AudioManager.Instance.Play("Melee_Pain");
                break;
            case (HumanEnum)2:
                AudioManager.Instance.Play("Gunner_Pain");
                break;
        }
        navMeshAgent.isStopped = true;
        GameManager.Instance.currentLevelGestion.humansToKill.Remove(this);
        yield return new WaitForSeconds(1f);
        Instantiate(ZombieSpawner.Instance.explosionPref, transform.position, Quaternion.identity);
        GameObject zombieInst = Instantiate(ZombieSpawner.Instance.zombiesList[0].zombiePref, transform.position, Quaternion.identity);
        AudioManager.Instance.Play("Groan_Walker");
        GameManager.Instance.playerZombies.Add(zombieInst.GetComponent<ZombieScript>());
        GetComponentInChildren<Canvas>().GetComponentInChildren<LifeBarScript>().StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
