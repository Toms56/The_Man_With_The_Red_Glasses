using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class NovizioRaycast : MonoBehaviour
{
    [SerializeField]
    private Transform castPoint;
    [SerializeField]
    private Transform castPoint2;

    [SerializeField] private float speed;

    public Animator animator;

    private Rigidbody rb;
    private float chaseRange = 1.5f;
    [SerializeField]private float attackRange;
    
    #region Sound

    public float volume = 0.5f;
    //public AudioClip runSound;
    public AudioClip deathSound;
    public AudioClip surpriseSound;
    public AudioClip[] surpriseClips;
    public AudioSource audioSource;
    #endregion

    #region state

    private bool isAgro;

    private bool isSearching;

    private bool isFacingLeft;

    #endregion
    #region UI
    [SerializeField] private float maxHealth;
    public GameObject healthBarUI;
    public Slider slider;
    #endregion

    #region Heal

    [SerializeField] private float healthPts;
    #endregion

    private bool doOnce;
    private float zAxis;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
        zAxis = transform.position.z;
        isAgro = false;
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        healthPts = maxHealth;
        slider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();
        if (healthPts < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (healthPts > maxHealth)
        {
            healthPts = maxHealth;
        }
        if (healthPts <= 0)
        {
            healthPts = 0;
            animator.SetBool("Death",true);
            healthBarUI.SetActive(false);
            isAgro = false;
            //audioSource.Play();
            if (!doOnce)
            { 
                
                StartCoroutine(Destroy());
                doOnce = true;
            }
        }
        float distance = Vector3.Distance(transform.position, target.position);
        //Debug.Log(distance);
        if (distance > chaseRange)
        {
            isAgro = false;
            //animator.SetBool("Chase",false);
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
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zAxis);
    }

    void RushPlayer()
    {
        Debug.Log(audioSource);
        audioSource.clip = surpriseSound;
        audioSource.Play();
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
        RaycastHit hit2;

        if (Physics.Raycast(castPoint2.position, -transform.right, out hit2, 1f, 1 << LayerMask.NameToLayer("Default")))
        {
            //penser a desac .forward lors de la rotation
            if (hit2.collider.CompareTag("Player"))
            {
                //Debug.Log("Player detected");
                isAgro = true;
            }
            Debug.DrawLine(castPoint2.position, hit2.point, Color.red);
        }
        else
        {
            isAgro = false;
            Debug.DrawLine(castPoint2.position, castPoint2.position + -transform.right.normalized * 1f,
                Color.green);
        }
    }
    

    IEnumerator Destroy()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        rb.detectCollisions = false;
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    float CalculateHealth()
    {
        return healthPts / maxHealth;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            healthPts--;
        }
    }
}
