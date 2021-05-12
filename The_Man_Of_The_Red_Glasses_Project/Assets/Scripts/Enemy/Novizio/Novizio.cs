using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Novizio : MonoBehaviour
{
    public float healthPts = 1; 
    
    private string currentState = "IdleState";
    public Transform target;

    public float chaseRange = 4f;
    public float attackRange = 0.5f;

    public float speed = 2f;

    public Animator animator;

    private bool chase;

    private bool patrol;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
     
        if (chase == false)
        {
            animator.SetBool("Chase",false);
            patrol = true;
            animator.SetBool("isPatrolling",true);
            GetComponent<NavMeshAgent>().SetDestination(RandomNavMeshLocation(50f));
        }
        if (distance < chaseRange)
        { 
            currentState = "ChaseState";
            if (currentState == "ChaseState")
            {
                animator.SetBool("isPatrolling",false);
                animator.SetBool("Chase",true);
            
                if (target.position.x > transform.position.x)
                {
                    transform.Translate(transform.right * speed * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.Translate(-transform.right * speed * Time.deltaTime); // -transform.r == transform.left
                    transform.rotation = Quaternion.identity;
                }

                
            }
        }else if (distance > chaseRange)
        {
            currentState = "IdleState";
            animator.SetBool("Chase",false);
        }
        if (distance < attackRange)
        {
            currentState = "AttackState";
            animator.SetBool("isAttacking", true);
        }

        if (distance > attackRange)
        {
            animator.SetBool("isAttacking", false);
            if (distance < chaseRange)
            {
                currentState = "ChaseState";
                animator.SetBool("Chase", true);
            }else if (distance > chaseRange)
            {
                currentState = "IdleState";
                animator.SetBool("Chase",false);
            }
        }

        if (healthPts <= 0)
        {
            //Vfx de l'effusion de sang
            //call de l'anim de destruction
            Destroy(gameObject);
        }
    }

    public Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
