using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoldatiPatrolWP : MonoBehaviour
{
    #region sound

    public float volume = 0.5f;
    //public AudioClip walkSound;
    public AudioClip deathSound;
    public AudioSource audioSource;
    #endregion
    #region Movement
    
        [SerializeField] float speed;
        [SerializeField]private float stopDistance;
        [SerializeField]private float retreatDistance;
        [SerializeField] private Transform[] wayPoints;
        private int wayPointIndex = 0;
        private float chaseRange = 2f;
        
        [SerializeField]
        private Transform castPoint;
        [SerializeField]
        private Transform castPoint2;
        public Transform target;
        #endregion

    public GameObject canon;
    [SerializeField] private Animator animator;

    private bool shoot = false;
    Rigidbody rb;

    private bool walk = true;
    [SerializeField]
    private SoldatiCanon canonScript;

    private bool doOnce;
    private int isInvert;

    #region UI
    public float healthPts;
    [SerializeField] private float maxHealth;
    public GameObject healthBarUI;
    public Slider slider;
    #endregion
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        walk = true;
        transform.position = wayPoints[wayPointIndex].transform.position;
        rb = GetComponent<Rigidbody>();
        healthPts = maxHealth;
        slider.value = CalculateHealth();
        #region sound

        //walkSound = Resources.Load<AudioClip>("walkSound");
        audioSource = GetComponent<AudioSource>();

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot && walk == false)
        {
            ShootPlayer();
        }
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
            canon.SetActive(false);
            healthPts = 0;
            walk = false;
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
        if (walk == false && shoot == false)
        {
            CancelInvoke("ShootPlayer");
            CancelInvoke("Patrol");
        }
        
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > chaseRange)
        {
            shoot = false;
            walk = true;
        }
        if (shoot == false && walk)
        {
            canonScript.enabled = false;
            //animator.SetBool("isShooting", false);
            CancelInvoke("ShootPlayer");
            Patrol();
        }
        else if (shoot && distance< chaseRange)
        {
            walk = false;
            canonScript.enabled = true;
            CancelInvoke("Move");
            ShootPlayer();
        }
    }

    void Patrol()
    {
        animator.SetBool("isPatrolling", true);
        //walk = true;
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex].transform.position,
            speed * Time.deltaTime);
        if (transform.position == wayPoints[wayPointIndex].transform.position)
        {
            wayPointIndex += 1;
        }
        if (wayPointIndex == wayPoints.Length)
        {
            wayPointIndex = 0;
        }

        if (wayPoints[wayPointIndex].transform.position.x > gameObject.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
        
        RaycastHit hit;
        if (Physics.Raycast(castPoint.position, -transform.right, out hit, 1f, 1 << LayerMask.NameToLayer("Default")))
        {
            //penser a desac .forward lors de la rotation
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player hit");
                walk = false;
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
                Debug.Log("Player hit");
                walk = false;
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

    /*void Move()
    {
        canon.SetActive(false);
        animator.SetBool("isPatrolling", true);
        walk = true;
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex].transform.position,
            speed * Time.deltaTime);
        if (transform.position == wayPoints[wayPointIndex].transform.position)
        {
            wayPointIndex += 1;
        }
        if (wayPointIndex == wayPoints.Length)
        {
            wayPointIndex = 0;
        }

        if (wayPoints[wayPointIndex].transform.position.x > gameObject.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
        
        RaycastHit hit;

        if (Physics.Raycast(castPoint.position, -transform.right, out hit, 1f, 1 << LayerMask.NameToLayer("Default")))
        {
            //penser a desac .forward lors de la rotation
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player hit");
                shoot = true;
                walk = false;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.red);
        }
        else
        {
            shoot = false;
            walk = true;
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
    }*/

    void ShootPlayer()
    {
        animator.SetBool("isPatrolling", false);
        float distance = Mathf.Abs(target.position.x - transform.position.x);
        Debug.Log(distance);
        //Debug.Log(Distance + "StopDistance");
        if(shoot)
        {
            canon.SetActive(true);
            if (distance > stopDistance)
            {
                transform.Translate(-transform.right * speed * Time.deltaTime * isInvert);
                animator.SetBool("isShooting", true);
                animator.SetBool("shootBack", false);
                walk = false;
                shoot = true;
            }else if (distance < retreatDistance)
            {
                transform.Translate(transform.right * speed * Time.deltaTime * isInvert); // -transform.r == transform.left
                animator.SetBool("isShooting", false);
                animator.SetBool("shootBack", true);
                walk = false;
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
        if(other.gameObject.tag == "PlayerBullet")
        {
            healthPts--;
        }    
    }
}
