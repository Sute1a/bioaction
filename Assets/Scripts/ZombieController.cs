using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{

    Animator animator;
    NavMeshAgent agent;

    public float walkingSpeed;

   public enum STATE { IDLE, WANDER, ATTACK, CHASE, DEAD };
    public STATE state = STATE.IDLE;

    GameObject target;
    public float runSpaaed;

    public int attackDamege;

    public GameObject ragdoll;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }


    public void TurnOffTrigger()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Death", false);
        animator.SetBool("Attack", false);
    }


    float DistanceToPlayer()
    {
        if (GameState.GameOver)
        {
            return Mathf.Infinity;
        }
        return Vector3.Distance(target.transform.position, transform.position);
    }

    bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 15)
        {
            return true;
        }
        return false;
    }

    bool ForGetPlayer()
    {
        if (DistanceToPlayer() > 20)
        {
            return true;
        }
        return false;
    }

    public void DamegePlayer()
    {
        if (target != null)
        {
            target.GetComponent<FPSController>().TakeHit(attackDamege);
        }
    }

    public void ZombieDeath()
    {
        TurnOffTrigger();
        animator.SetBool("Death", true);
        state = STATE.DEAD;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.IDLE:
                TurnOffTrigger();

                if (CanSeePlayer()　==true)
                {
                    state = STATE.CHASE;
                }

                else if (Random.Range(0, 5000) < 5)
                {
                    state = STATE.WANDER;
                }


                break;

            case STATE.WANDER:
                if (!agent.hasPath)
                {
                    float newX = transform.position.x + Random.Range(-5, -5);
                    float newZ = transform.position.z + Random.Range(-5, -5);

                    Vector3 NextPos = new Vector3(newX, transform.position.y, newZ);

                    agent.SetDestination(NextPos);
                    agent.stoppingDistance = 0;

                    TurnOffTrigger();

                    agent.speed = walkingSpeed;
                    animator.SetBool("Walk", true);
                }

                
                if (Random.Range(0, 5000) < 5)
                {
                    state = STATE.IDLE;
                    agent.ResetPath();
                }

                

                if (CanSeePlayer())
                {
                    state = STATE.CHASE;
                }

                break;


            case STATE.CHASE:

                if (GameState.GameOver)
                {
                    TurnOffTrigger();

                    agent.ResetPath();
                    state = STATE.WANDER;

                    return;
                }

                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 3;

                TurnOffTrigger();

                agent.speed = runSpaaed;
                animator.SetBool("Run",true);

                if (agent.remainingDistance<= agent.stoppingDistance + 2)
                {
                    state = STATE.ATTACK;
                }

                if (ForGetPlayer())
                {
                    agent.ResetPath();
                    state = STATE.WANDER;
                }

                break;

            case STATE.ATTACK:

                if (GameState.GameOver)
                {
                    TurnOffTrigger();

                    agent.ResetPath();
                    state = STATE.WANDER;

                    return;
                }

                TurnOffTrigger();
                animator.SetBool("Attack", true);

                transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));

                
                if (DistanceToPlayer() > agent.stoppingDistance + 2)
                {
                    state = STATE.CHASE;
                }
                break;

            case STATE.DEAD:

                Destroy(agent);

                break;
        }
    }
}

