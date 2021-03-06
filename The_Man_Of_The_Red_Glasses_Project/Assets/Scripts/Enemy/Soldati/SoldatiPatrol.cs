using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoldatiPatrol : MonoBehaviour
{
    #region Movement

    [SerializeField] float speed;
    [SerializeField]private float stopDistance;
    [SerializeField]private float retreatDistance;
    [SerializeField] float waitTime;

    private float currentWaitTime = 0f;

    [SerializeField]private float minX;
    [SerializeField]private float maxX;
    

    private Vector3 moveSpot;
    
    //Jumping
    private Rigidbody rb;
    private Vector3 localScale;
    private float dirX;

    #endregion

    #region animationState
    public float healthPts = 1; 
    
    private string currentState = "IdleState";
    public Transform target;

    public float chaseRange = 4f;
    public float attackRange = 0.5f;
    public Animator animator;

    private bool shoot;

    private bool patrol;
    

    #endregion

    #region Shoot
    public GameObject canon;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        GetGroundSize();
        moveSpot = GetNewPosition();
        //Jump
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        //Debug.Log(distance);
        if (shoot == false)
        {
            animator.SetBool("isShooting",false);
            animator.SetBool("shootBack", false);
            WatchYourStep();
            GetToStepping();
        }
        if (distance > chaseRange)
        {
            animator.SetBool("isShooting",false);
            animator.SetBool("shootBack", false);
            animator.SetBool("isPatrolling",true);
            shoot = false;
            //canon.SetActive(false);
        }else if (distance < chaseRange)
        {
            shoot = true;
            //canon.SetActive(true);
            if (Vector2.Distance(transform.position, target.position) < stopDistance)
            {
                //transform.Translate(transform.right * speed * Time.deltaTime);
                currentState = "shootState";
                animator.SetBool("isPatrolling",false);
                animator.SetBool("isShooting", true);
                animator.SetBool("shootBack", false);
                Vector3 targetDirection = target.position - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDirection);
                patrol = false;
                shoot = true;
            }
            
            if (Vector2.Distance(transform.position, target.position) > stopDistance + 0.1f)
            {
                transform.Translate(transform.right * -speed * Time.deltaTime);
                currentState = "shootState";
                animator.SetBool("isPatrolling",false);
                animator.SetBool("isShooting", true);
                animator.SetBool("shootBack", false);
                patrol = false;
                shoot = true;
            }
            
            else if(Vector2.Distance(transform.position,target.position)> stopDistance && Vector2.Distance(transform.position,target.position)<retreatDistance)
            {
                animator.SetBool("isPatrolling",false);
                animator.SetBool("isShooting", false);
                animator.SetBool("shootBack", false);
                animator.SetBool("stopWalk", true);
                //transform.position = this.transform.position;
            }else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                transform.Translate(-transform.right * -speed * Time.deltaTime); // -transform.right == transform.left
                currentState = "shootState";
                animator.SetBool("isPatrolling",false);
                animator.SetBool("isShooting", false);
                animator.SetBool("shootBack", true);
                /*Vector3 targetDirection = target.position - transform.position;
                transform.rotation = Quaternion.LookRotation(targetDirection);*/
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

    /*private void FixedUpdate()
    {
        rb.velocity = new Vector2(transform.localScale.x , rb.velocity.y);
    }*/

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
        transform.rotation = Quaternion.LookRotation(targetDirection);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Wall")
        {
            GetNewPosition();
        }
    }
}
