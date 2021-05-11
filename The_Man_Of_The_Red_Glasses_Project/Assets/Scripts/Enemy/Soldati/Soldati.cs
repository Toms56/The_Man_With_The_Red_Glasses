using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldati : MonoBehaviour
{

    private string currentState = "IdleState";
    [SerializeField]private float speed;
    public float chaseRange = 4f;

    [SerializeField]private float stopDistance;

    [SerializeField]private float retreatDistance;

    public Transform target;

    public Animator animator;
    private bool shoot;
    private bool patrol;
    
    // Start is called before the first frame update
    void Start()
    {
        //waitTime = startWaitTime;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance(transform.position, target.position);
        Debug.Log(distance);
        Debug.Log(currentState);

        //Patrol State
        if (shoot == false)
        {
            animator.SetBool("isShooting",false);
            patrol = true;
            animator.SetBool("isPatrolling",true);
            GetComponent<NavMeshAgent>().SetDestination(RandomNavMeshLocation(6f));
        }

        if (distance > chaseRange)
        {
            animator.SetBool("isShooting",false);
            animator.SetBool("isPatrolling",true);
            shoot = false;
        }else if (distance < chaseRange)
        {
            if (Vector2.Distance(transform.position, target.position) < stopDistance)
            {
                //transform.Translate(transform.right * speed * Time.deltaTime);
                currentState = "shootState";
                animator.SetBool("isPatrolling",false);
                animator.SetBool("isShooting", true);
                patrol = false;
                shoot = true;
            }
            if (Vector2.Distance(transform.position, target.position) > stopDistance)
            {
                transform.Translate(transform.right * -speed * Time.deltaTime);
                currentState = "shootState";
                animator.SetBool("isPatrolling",false);
                animator.SetBool("isShooting", true);
                patrol = false;
                shoot = true;
            }else if(Vector2.Distance(transform.position,target.position)< stopDistance && Vector2.Distance(transform.position,target.position)>retreatDistance)
            {
                transform.position = this.transform.position;
            }else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                transform.Translate(-transform.right * -speed * Time.deltaTime); // -transform.r == transform.left
                currentState = "shootState";
                animator.SetBool("isPatrolling",false);
                animator.SetBool("isShooting", true);
                patrol = false;
                shoot = true;
            } 
            if (target.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
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
