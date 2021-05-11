using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Novizio : MonoBehaviour
{
    private string currentState = "IdleState";
    public Transform target;

    public float chaseRange = 4f;
    public float attackRange = 0.5f;

    public float speed = 2f;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Debug.Log(distance);
        //Debug.Log(currentState);
        if (distance < chaseRange)
        { 
            currentState = "ChaseState";
            if (currentState == "ChaseState")
            {
                //animator.SetTrigger("chase");
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
    }
}
