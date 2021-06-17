using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldatiStaticRaycast : MonoBehaviour
{

    #region sound

    public float volume = 0.5f;
    //[SerializeField] private AudioClip[] clips;
    public AudioClip deathSound;
    public AudioSource audioSource;
    #endregion
    #region Movement

    [SerializeField] float speed;
    [SerializeField]private float stopDistance;
    [SerializeField]private float retreatDistance;
    
    private float chaseRange = 2f;
    
    [SerializeField]
    private Transform castPoint;
    [SerializeField]
    private Transform castPoint2;
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
    [SerializeField] private float maxHealth;
    public GameObject healthBarUI;
    public Slider slider;

    private int isInvert;
    
    private bool doOnce;

    private bool isHit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        patrol = true;
        #region sound

        //walkSound = Resources.Load<AudioClip>("walkSound");
        audioSource = GetComponent<AudioSource>();

        #endregion
        target = GameObject.FindGameObjectWithTag("Player").transform;
        shoot = false;
        //Debug.Log(shoot);
        rb = GetComponent<Rigidbody>();

        healthPts = maxHealth;
        slider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.right);
        if (healthPts > maxHealth)
        {
            healthPts = maxHealth;
        }
        if (healthPts <= 0)
        {
            canon.SetActive(false);
            healthPts = 0;
            patrol = false;
            shoot = false;
            animator.SetBool("Death",true);
            healthBarUI.SetActive(false);
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            if (!doOnce)
            { 
                StartCoroutine(Destroy());
                doOnce = true;
            }
        }

        if (patrol == false && shoot == false)
        {
            CancelInvoke("ShootPlayer");
            CancelInvoke("Patrol");
        }
        
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > chaseRange)
        {
            shoot = false;
            patrol = true;
        }
        if (shoot == false && patrol)
        {
            canonScript.enabled = false;
            CancelInvoke("ShootPlayer");
            Patrol();
        }else if (shoot == true && distance < chaseRange)
        {
            canonScript.enabled = true;
            CancelInvoke("Patrol");
            ShootPlayer();
        }

        slider.value = CalculateHealth();
        if (healthPts < maxHealth)
        {
            healthBarUI.SetActive(true);
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
                //isHit = false;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            shoot = false;
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
                shoot = true;
                //isHit = false;
            }
            Debug.DrawLine(castPoint2.position, hit2.point, Color.red);
        }
        else
        {
            shoot = false;
            Debug.DrawLine(castPoint2.position, castPoint2.position + -transform.right.normalized * 1f,
                Color.green);
        }
    }
    
    void ShootPlayer()
    {
        float distance = Mathf.Abs(target.position.x - transform.position.x);
        //Debug.Log(Distance + "StopDistance");
        if(shoot)
        {
            canon.SetActive(true);
            if (distance > stopDistance)
            {
                transform.Translate(-transform.right * speed * Time.deltaTime * isInvert);
                animator.SetBool("isShooting", true);
                animator.SetBool("shootBack", false);
                patrol = false;
                shoot = true;
            }else if (distance < retreatDistance)
            {
                transform.Translate(transform.right * speed * Time.deltaTime * isInvert); // -transform.r == transform.left
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
                isInvert = -1;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isInvert = 1;
            }
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
        float distance = Mathf.Abs(target.position.x - transform.position.x);
        if(other.gameObject.tag == "PlayerBullet")
        {
            /*isHit = true;
            if (target.position.x < gameObject.transform.position.x)
            {
                animator.SetBool("isShooting", true);
                canon.SetActive(true);
                transform.Translate(-transform.right * speed * Time.deltaTime * isInvert);
            }else if (target.position.x > gameObject.transform.position.x)
            {
                canon.SetActive(true);
                animator.SetBool("isShooting", true);
                transform.Translate(transform.right * speed * Time.deltaTime * isInvert);
            }*/
            healthPts--;
        }

        if (other.gameObject.tag == "Player")
        {
            rb.detectCollisions = false;
        }
    }
}
