using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    // PV
    public int pv = 3;
    public bool die = false;


    // For Mouvement
    public float jumpHeight;
    public float baseSpeed;
    private float finalSpeed;
    private Vector3 movement;
    private bool sneaky;
    private bool isGrounded;
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

    public bool aiming;

    // For its Components
    private static Animator animator;
    private Camera mainCamera;
    private Transform playerTransform;
    //private Animation animCollider;
    private CharacterController charaController;


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
        charaController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        //animCollider = GetComponent<Animation>();
        mainCamera = Camera.main;
    }


    void Update()
    {
        if (pv == 0 && !die)
        {
            // Mort du joueur / Empêcher le shoot 
            animator.SetBool("Death", true);
            //animCollider.Play("DeathCollider");
            die = true;
            if (die)
            {
                charaController.height = 0f;
            }
            return;
        }

        // MOVEMENT
        float inputX = Input.GetAxis("Horizontal");
        movement.x = inputX * finalSpeed;
        animator.SetFloat("Speed", (facingSign * movement.x) / finalSpeed);


        // SNEAKY AND SPEED                                    
        if (Input.GetKeyDown(KeyCode.LeftControl) && !sneaky)
        {
            animator.SetBool("Sneaky", true);
            //animCollider["SneakyCollider"].speed = 1;
            //animCollider.Play("SneakyCollider");
            charaController.height = 6f;
            sneaky = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && sneaky)
        {
            animator.SetBool("Sneaky", false);
            //animCollider["SneakyCollider"].speed = -1;
            //animCollider.Play("SneakyCollider");
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
        if (charaController.isGrounded)
        {
            if (!isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    movement.y = 7f;
                    isGrounded = true;
                    animator.SetBool("Jump", true);
                }
            }
            else
            {
                movement.y = 0;
                isGrounded = false;
                animator.SetBool("Jump", false);
            }
        }
        else
        {
            movement.y -= 9.81f * Time.deltaTime;
        }


        // FACING ROTATION
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTform.position = hit.point;
        }

        playerTransform.rotation = (Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(targetTform.position.x - transform.position.x), 0)));


        charaController.Move(movement * Time.deltaTime);
        Debug.Log(charaController.height);
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

            aiming = true;
        }
        else
        {
            aiming = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BulletEnemy")
        {
            pv -= 1;
        }
    }
}
