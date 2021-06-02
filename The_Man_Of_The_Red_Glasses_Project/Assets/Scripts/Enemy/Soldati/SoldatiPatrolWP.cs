using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        public Transform target;
        #endregion

    public GameObject canon;
    [SerializeField] private Animator animator;

    private bool shoot = false;

    private bool walk = true;
    [SerializeField]
    private SoldatiCanon canonScript;

    public float healthPts;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = wayPoints[wayPointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
