using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovizioPatrol : MonoBehaviour
{

    [SerializeField] float speed;

    [SerializeField] float waitTime;

    private float currentWaitTime = 0f;

    [SerializeField]private float minX;
    [SerializeField]private float maxX;

    private Vector3 moveSpot;

    #region animationState
    public float healthPts = 1; 
    
    private string currentState = "IdleState";
    public Transform target;

    public float chaseRange = 4f;
    public float attackRange = 0.5f;
    public Animator animator;

    private bool chase;

    private bool patrol;
    

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        GetGroundSize();
        moveSpot = GetNewPosition();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        Debug.Log(distance);
        if (chase == false)
        {
            animator.SetBool("Chase",false);
            WatchYourStep();
            GetToStepping();
        }
        if (distance < chaseRange)
        {
            chase = true;
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
            chase = false;
            currentState = "IdleState";
            animator.SetBool("Chase",false);
        }
        if (distance < attackRange && distance<chaseRange)
        {
            currentState = "AttackState";
            animator.SetBool("isAttacking", true);
            animator.SetBool("Chase",false);
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
    }

    private void GetGroundSize()
    {
        GameObject ground = GameObject.FindWithTag("Ground");
        Renderer groundSize = ground.GetComponent<Renderer>();
        minX = (groundSize.bounds.center.x - groundSize.bounds.extents.x);
        minX = (groundSize.bounds.center.x + groundSize.bounds.extents.x);
    }

    Vector3 GetNewPosition()
    {
        animator.SetBool("isPatrolling",false);
        float randomX = Random.Range(minX, maxX);
        Vector3 newPosition = new Vector3(randomX,transform.position.y,transform.position.z);
        return newPosition;
    }

    void GetToStepping()
    {
        //Debug.Log(currentWaitTime);
        transform.position = Vector3.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);
        if (currentWaitTime <= 0)
        {
            currentWaitTime = waitTime;
            moveSpot = GetNewPosition();
        }
        else
        {
            animator.SetBool("isPatrolling",true);
            currentWaitTime -= Time.deltaTime;
        }
    }

    void WatchYourStep()
    {
        Vector3 targetDirection = moveSpot - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

   
}
