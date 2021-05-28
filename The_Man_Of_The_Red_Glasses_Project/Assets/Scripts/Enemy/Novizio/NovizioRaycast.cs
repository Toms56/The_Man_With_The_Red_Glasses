using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovizioRaycast : MonoBehaviour
{
    [SerializeField]
    private Transform castPoint;

    [SerializeField] private float speed;

    public Animator animator;

    private Rigidbody rb;
    private float chaseRange = 1.5f;
    [SerializeField]private float attackRange;

    #region state

    private bool isAgro;

    private bool isSearching;

    private bool isFacingLeft;

    #endregion

    #region Heal

    [SerializeField] private float healthPts;
    #endregion

    #region Audio

    [SerializeField] private AudioSource novizio;
    [SerializeField] private AudioClip deathClip;
    
    #endregion

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        isAgro = false;
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthPts <= 0)
        {
            healthPts = 0;
        }
        float distance = Vector3.Distance(transform.position, target.position);
        //Debug.Log(distance);
        if (distance > chaseRange)
        {
            isAgro = false;
            animator.SetBool("Chase",false);
        }
        if (isAgro == false)
        {
            CancelInvoke("RushPlayer");
            animator.SetBool("Chase",false);
            Patrol();
        }else if (isAgro && distance < chaseRange)
        {
            CancelInvoke("Patrol");
            RushPlayer();
        }
    }

    void RushPlayer()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        animator.SetBool("Chase",true);
        if (distance > attackRange)
        {
            animator.SetBool("isAttacking", false);
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
        
        if (distance < attackRange)
        {
            animator.SetBool("isAttacking", true);
        }
    }
    void Patrol()
    {
        RaycastHit hit;

        if (Physics.Raycast(castPoint.position, -transform.right, out hit, 1f, 1 << LayerMask.NameToLayer("Default")))
        {
            //penser a desac .forward lors de la rotation
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player hit");
                isAgro = true;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            isAgro = false;
            Debug.DrawLine(castPoint.position, castPoint.position + -transform.right.normalized * 1f,
                Color.green);
        }
    }

    IEnumerator DestroyNovizio()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("Enemy Hit ! ");
            healthPts--;
            if (healthPts <= 0)
            {
                novizio.clip = deathClip;
                novizio.Play();
                Debug.Log(deathClip);
                StartCoroutine(DestroyNovizio());
            }
        }
    }
}
