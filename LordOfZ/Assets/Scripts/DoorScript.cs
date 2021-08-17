using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private int doorLifePoints;
    private int startLife;
    private Rigidbody rb;
    private BoxCollider boxCol;
    private Animator animator;

    private void Awake()
    {
        startLife = doorLifePoints;
        rb = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (doorLifePoints <= 0)
        {
            boxCol.enabled = false;
            animator.SetBool("Fall", true);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<ZombieScript>() != null)
        {
            doorLifePoints -= 5;
        }
    }

    public void Reset()
    {
        doorLifePoints = startLife;
        boxCol.enabled = true;
        animator.SetBool("Fall", false);
    }
}
