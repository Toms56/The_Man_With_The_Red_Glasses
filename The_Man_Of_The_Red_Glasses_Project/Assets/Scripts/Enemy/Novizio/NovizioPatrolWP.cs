using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovizioPatrolWP : MonoBehaviour
{
    #region Movement

    [SerializeField] float speed;
    [SerializeField] private Transform[] wayPoints;
    private int wayPointIndex = 0;
    private float chaseRange = 1f;

    private Rigidbody rigidbody;

    [SerializeField] private Transform castPoint;
    [SerializeField] private Transform castPoint2;
    public Transform target;
    [SerializeField]private float attackRange;

    #endregion
    [SerializeField] private Animator animator;
    
    private bool walk = true;

    private Transform deathPos;
    
    #region UI
    [SerializeField] private float maxHealth;
    public GameObject healthBarUI;
    public Slider slider;
    #endregion

    #region state

    private bool isAgro;

    private bool isSearching;

    private bool isFacingLeft;

    #endregion

    #region Heal

    [SerializeField] private float healthPts;

    #endregion

    #region sound

    public float volume = 0.5f;
    //[SerializeField] private AudioClip[] clips;
    public AudioClip walkSound;
    public AudioClip deathSound;
    public AudioSource audioSource;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        transform.position = wayPoints[wayPointIndex].transform.position;
        healthPts = maxHealth;
        slider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAgro && walk == false)
        {
            RushPlayer();
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
            /*rigidbody.constraints = RigidbodyConstraints.FreezePositionX;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY;*/
            healthPts = 0;
            animator.SetBool("Death",true);
            healthBarUI.SetActive(false);
            isAgro = false;
            walk = false;
            /*novizio.clip = deathClip;
            novizio.Play();
            Debug.Log(deathClip);*/
            //CancelInvoke("RushPlayer");
            StartCoroutine(Destroy());
        }

        /*if (isAgro == false && walk == false)
        {
            CancelInvoke("Patrol");
            CancelInvoke("RushPlayer");
        }*/

        float distance = Vector3.Distance(transform.position, target.position);
        //Debug.Log(distance);
        if (distance > chaseRange)
        {
            isAgro = false;
            //animator.SetBool("Chase", false);
        }
        if (isAgro == false && walk == true)
        {
            CancelInvoke("RushPlayer");
            animator.SetBool("Chase", false);
            Patrol();
        }
        else if (isAgro && distance < chaseRange)
        {
            CancelInvoke("Patrol");
            RushPlayer();
        }
    }
    
    void Patrol()
    {
        //audioSource.Play();
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
                //walk = false;
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
                Debug.Log("Player hit");
                //walk = false;
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
    IEnumerator Destroy()
    {
        //audioSource.PlayOneShot(deathSound, volume);
        //CancelInvoke("Patrol");
        rigidbody.detectCollisions = false;
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
