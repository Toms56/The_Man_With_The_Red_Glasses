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
    
    // Start is called before the first frame update
    void Start()
    {
        //waitTime = startWaitTime;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Patrol();
        float distance = Vector3.Distance(transform.position, target.position);
        Debug.Log(distance);
        Debug.Log(currentState);

        if (shoot == false)
        {
            animator.SetBool("isShooting",false);
            GetComponent<NavMeshAgent>().Move(transform.forward*Time.deltaTime);
        }

        if (distance > chaseRange)
        {
            animator.SetBool("isShooting",false);
            shoot = false;
        }else if (distance < chaseRange)
        {
            if (Vector2.Distance(transform.position, target.position) < stopDistance)
            {
                //transform.Translate(transform.right * speed * Time.deltaTime);
                currentState = "shootState";
                animator.SetBool("isShooting", true);
                shoot = true;
            }
            if (Vector2.Distance(transform.position, target.position) > stopDistance)
            {
                transform.Translate(transform.right * -speed * Time.deltaTime);
                currentState = "shootState";
                animator.SetBool("isShooting", true);
                shoot = true;
            }else if(Vector2.Distance(transform.position,target.position)< stopDistance && Vector2.Distance(transform.position,target.position)>retreatDistance)
            {
                transform.position = this.transform.position;
            }else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                transform.Translate(-transform.right * -speed * Time.deltaTime); // -transform.r == transform.left
                currentState = "shootState";
                animator.SetBool("isShooting", true);
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

    /*public void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }*/
}
