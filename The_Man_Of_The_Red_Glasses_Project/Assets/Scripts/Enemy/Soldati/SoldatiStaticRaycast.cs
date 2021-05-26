using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldatiStaticRaycast : MonoBehaviour
{
    
    #region Movement

    [SerializeField] float speed;
    [SerializeField]private float stopDistance;
    [SerializeField]private float retreatDistance;
    
    private float chaseRange = 2f;
    
    [SerializeField]
    private Transform castPoint;
    public Transform target;
    #endregion

    [SerializeField]
    private SoldatiCanon canonScript;
    
    private Rigidbody rb;

    public GameObject canon;
    
    public Animator animator;

    private bool shoot;

    private bool patrol;

    private bool isPatrolling;

    [SerializeField] private float healthPts;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        shoot = false;
        Debug.Log(shoot);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance(transform.position, target.position);
        //Debug.Log(distance);
        if (distance > chaseRange)
        {
            shoot = false;
            Debug.Log(shoot + ">chase Range");
        }
        if (shoot == false)
        {
            Debug.Log(canonScript);
            //GetComponent<SoldatiCanon>().enabled = false;
            canonScript.enabled = false;
            CancelInvoke("ShootPlayer");
            Patrol();
        }else if (shoot == true && distance < chaseRange)
        {
            //GetComponent<SoldatiCanon>().enabled = true;
            canonScript.enabled = true;
            CancelInvoke("Patrol");
            ShootPlayer();
        }
    }

    void Patrol()
    {
        canon.SetActive(false);
        animator.SetBool("isShooting", false);
        animator.SetBool("shootBack", false);
        RaycastHit hit;

        if (Physics.Raycast(castPoint.position, -transform.right, out hit, 1f, 1 << LayerMask.NameToLayer("Default")))
        {
            //penser a desac .forward lors de la rotation
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player hit");
                shoot = true;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            shoot = false;
            Debug.DrawLine(castPoint.position, castPoint.position + -transform.right.normalized * 1f,
                Color.green);
        }
    }
    
    void ShootPlayer()
    {
        //float distance = Vector2.Distance(transform.position, target.position);
        if(shoot == true)
        {
            canon.SetActive(true);
            if (Vector2.Distance(transform.position, target.position) < stopDistance)
            {
                //transform.Translate(transform.right * speed * Time.deltaTime);
                animator.SetBool("isShooting", true);
                animator.SetBool("shootBack", false);
                patrol = false;
                shoot = true;
            }
            if (Vector2.Distance(transform.position, target.position) > stopDistance+0.1f)
            {
                transform.Translate(transform.right * -speed * Time.deltaTime);
                animator.SetBool("isShooting", true);
                animator.SetBool("shootBack", false);
                patrol = false;
                shoot = true;
            }else if(Vector2.Distance(transform.position,target.position) < stopDistance && Vector2.Distance(transform.position,target.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                transform.Translate(-transform.right * -speed * Time.deltaTime); // -transform.r == transform.left
                animator.SetBool("isShooting", false);
                animator.SetBool("shootBack", true);
                patrol = false;
                shoot = true;
            } 
            if (target.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("isShooting", true);
                animator.SetBool("shootBack", false);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("Enemy Hit ! ");
            healthPts--;
            //Destroy(other.gameObject);
            if (healthPts <= 0)
            {
                Destroy(gameObject);
            }
        }    
    }

}
