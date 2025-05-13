using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public Transform[] patrolPoints;
    public int currentPatrolPoint;

    public NavMeshAgent agent;

    public Animator animator;

    public float waitAtPoint = 2f;
    private float waitCounter;

    public float chaseRange;

    public float attackRange = 1f;
    public float timeBetweenAttacks = 2f;
    private float attackCounter;

    private bool hasFirstAttacked;


    public enum AI_STATE
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking

    };
    public AI_STATE currentState;

    void Start()
    {
        waitCounter = waitAtPoint;
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerMovement.instance.transform.position);

        switch (currentState)
        {
            case AI_STATE.Idle:
                Idle(distanceToPlayer);
                break;
            case AI_STATE.Patrolling:
                Patrol(distanceToPlayer);
                break;
            case AI_STATE.Chasing:
                Chase(distanceToPlayer); 
                break;
            case AI_STATE.Attacking:
                Attack(distanceToPlayer);
                break;
        }

    }

    private void Patrol(float distanceToPlayer)
    {

        if (agent.remainingDistance <= .2f)
        {
            currentPatrolPoint++; //enemigo se mueve al siguiente punto
            if (currentPatrolPoint >= patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }

            currentState = AI_STATE.Idle;
            waitCounter = waitAtPoint;

        }


        if (distanceToPlayer <= chaseRange)
        {
            currentState = AI_STATE.Chasing;
        }

        animator.SetBool("IsMoving", true);
    }


    private void Idle(float distanceToPlayer)
    {
        animator.SetBool("IsMoving", false);
        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
        } else
        {
            currentState = AI_STATE.Patrolling;
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        }

        if (distanceToPlayer <= chaseRange)
        {
            currentState = AI_STATE.Chasing;
            animator.SetBool("IsMoving", true);
        }
    }

    private void Chase(float distanceToPlayer)
    {
        agent.SetDestination(PlayerMovement.instance.transform.position);

        if(distanceToPlayer <= attackRange)
        {
            currentState = AI_STATE.Attacking;
          //  animator.SetTrigger("Attack");
            animator.SetBool("IsMoving", false);
            
            agent.velocity = Vector3.zero; //dejarlo quieto cuando ataca
            agent.isStopped = true;
            attackCounter = 0f; 
            hasFirstAttacked = false; 
        }

        if(distanceToPlayer > chaseRange)
        {
            currentState = AI_STATE.Idle;
            waitCounter = waitAtPoint;
            agent.velocity = Vector3.zero;
            agent.SetDestination(transform.position);
        }
    }

    private void Attack(float distanceToPlayer)
    {
        transform.LookAt(PlayerMovement.instance.transform, Vector3.up);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        attackCounter -= Time.deltaTime;


        if (!hasFirstAttacked)
        {
            if (distanceToPlayer < attackRange)
            {
                animator.SetTrigger("Attack");
                hasFirstAttacked = true;
                attackCounter = timeBetweenAttacks;
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                if (distanceToPlayer < attackRange)
                {
                    animator.SetTrigger("Attack");
                    attackCounter = timeBetweenAttacks;
                }
                else
                {
                    currentState = AI_STATE.Idle;
                    waitCounter = waitAtPoint;
                    agent.isStopped = false;
                }
            }
        }


/*        if (attackCounter <= 0)
        {
            if(distanceToPlayer < attackRange)
            {
                animator.SetTrigger("Attack");
                attackCounter = timeBetweenAttacks;
            } else
            {
                currentState = AI_STATE.Idle;
                waitCounter = waitAtPoint;
                agent.isStopped = false;
            }
        }*/
    }
}
