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
    public AudioClip walkSound;
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

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
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
        if (healthPts > maxHealth)
        {
            healthPts = maxHealth;
        }
        if (healthPts <= 0)
        {
            healthPts = 0;
            patrol = false;
            shoot = false;
            animator.SetBool("Death",true);
            healthBarUI.SetActive(false);
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            /*novizio.clip = deathClip;
            novizio.Play();
            Debug.Log(deathClip);*/
            StartCoroutine(Destroy());
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
        //float distance = Vector2.Distance(transform.position, target.position);
        if(shoot)
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
            healthPts--;
        }

        if (other.gameObject.tag == "Player")
        {
            rb.detectCollisions = false;
        }
    }

    IEnumerator Destroy()
    {
        //audioSource.PlayOneShot(deathSound, volume);
        rb.detectCollisions = false;
        canon.SetActive(false);
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    float CalculateHealth()
    {
        return healthPts / maxHealth;
    }
}
