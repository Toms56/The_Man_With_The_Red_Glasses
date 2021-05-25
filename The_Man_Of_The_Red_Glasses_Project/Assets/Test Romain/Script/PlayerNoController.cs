using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoController : MonoBehaviour
{
    public static PlayerNoController Instance;

    // PV
    public int pv = 3;
    public bool die = false;


    // For Mouvement
    public float jumpHeight;
    public float baseSpeed;
    private float finalSpeed;
    private float mouvement;
    private bool sneaky;
        // Rotation (Permet à la bonne animation de se faire selon le sens où il regarde)
    private int facingSign
    {
        get
        {
            Vector3 perpendi = Vector3.Cross(transform.forward, Vector3.forward);
            float dir = Vector3.Dot(perpendi, transform.up);
            return dir > 00f ? -1 : dir < 0f ? 1 : 0;
        }
    }
    private bool isGrounded;
    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundCheckMask;


    // For its Components
    private static Animator animator;
    private Rigidbody rbody;
    private Camera mainCamera;


    // For other public Components
    // Rotation
    public Transform targetTform;
    public LayerMask mouseAimMask;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        targetTform.position = new Vector3(targetTform.position.x, targetTform.position.y, -0.14f);

        if (pv == 0)
        {
            // Mort du joueur / Empêcher le shoot 
            animator.SetBool("Death", true);
            die = true;
            return;
        }


        //MOVEMENT
        mouvement = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector3(mouvement * finalSpeed, rbody.velocity.y, 0);
        animator.SetFloat("Speed", (facingSign * rbody.velocity.x) / finalSpeed);

        // SNEAKY AND SPEED                                    REMETTRE L ANIMATION POUR LE SNEAKY 
        if (Input.GetKeyDown(KeyCode.LeftControl) && !sneaky)
        {
            animator.SetBool("Sneaky", true);
            sneaky = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && sneaky)
        {
            animator.SetBool("Sneaky", false);
            sneaky = false;
            finalSpeed = baseSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift) && finalSpeed != 0)
        {
            finalSpeed = baseSpeed * 2;
            animator.SetBool("Run", true);
        }
        else if (sneaky)
        {
            finalSpeed = baseSpeed / 1.5f;
        }
        else
        {
            finalSpeed = baseSpeed;
            animator.SetBool("Run", false);
        }


        // JUMP
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rbody.velocity = new Vector3(rbody.velocity.x, jumpHeight, 0);
            animator.SetBool("Jump",true);
        }

        if (rbody.velocity.y == 0)
        {
            animator.SetBool("Jump", false);
        }

        Debug.Log("Grounded : " + isGrounded);
        Debug.Log("Run : " + animator.GetBool("Run"));


        // FACING ROTATION
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTform.position = hit.point;
        }

        rbody.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(targetTform.position.x - transform.position.x),0)));


        // GROUND CHECK
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask, QueryTriggerInteraction.Ignore);
    }

    private void OnAnimatorIK()
    {
        if (Input.GetMouseButton(1) && die == false)
        {
            // Weapon Aim Target
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, targetTform.position/* + new Vector3(0,1,0)*/);

            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(targetTform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "BulletEnemy")
        {
            pv -= 1;
        }
    }
}
