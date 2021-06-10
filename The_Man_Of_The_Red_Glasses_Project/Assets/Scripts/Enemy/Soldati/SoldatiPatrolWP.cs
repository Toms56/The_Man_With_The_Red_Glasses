using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoldatiPatrolWP : MonoBehaviour
{
    #region Movement
    
        [SerializeField] float speed;
        [SerializeField]private float stopDistance;
        [SerializeField]private float retreatDistance;
        [SerializeField] private Transform[] wayPoints;
        private int wayPointIndex = 0;
        private float chaseRange = 2f;
        
        [SerializeField]
        private Transform castPoint;
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

    #region UI
    public float healthPts;
    [SerializeField] private float maxHealth;
    public GameObject healthBarUI;
    public Slider slider;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = wayPoints[wayPointIndex].transform.position;
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
            animator.SetBool("Death", true);
            healthPts = 0;
            StartCoroutine(Destroy());
        }
        slider.value = CalculateHealth();
        if (healthPts < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > chaseRange)
        {
            shoot = false;
        }
        if (shoot == false)
        {
            canonScript.enabled = false;
            animator.SetBool("isShooting", false);
            CancelInvoke("ShootPlayer");
            Move();
        }
        else if (shoot && distance< chaseRange)
        {
            canonScript.enabled = true;
            CancelInvoke("Move");
            ShootPlayer();
        }
    }

    void Move()
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
    }

    void ShootPlayer()
    {
        animator.SetBool("isPatrolling",false);
        canon.SetActive(true);
        if (Vector2.Distance(transform.position, target.position) < stopDistance)
        {
            //transform.Translate(transform.right * speed * Time.deltaTime);
            animator.SetBool("isShooting", true);
            animator.SetBool("shootBack", false);
            walk = false;
            shoot = true;
        }
        if (Vector2.Distance(transform.position, target.position) > stopDistance+0.1f)
        {
            transform.Translate(transform.right * -speed * Time.deltaTime);
            animator.SetBool("isShooting", true);
            animator.SetBool("shootBack", false);
            walk = false;
            shoot = true;
        }else if(Vector2.Distance(transform.position,target.position) < stopDistance && Vector2.Distance(transform.position,target.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
        {
            transform.Translate(-transform.right * -speed * Time.deltaTime); // -transform.r == transform.left
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
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
    
    IEnumerator Destroy()
    {
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
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            healthPts--;
        }    
    }
}
