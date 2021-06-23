using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private float zAxis;

    // PV
    public int pv;
    public bool die = false;

    public bool wallJump = false;
    private Vector3 aimPosition;

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

    // Shoot
    public bool aiming;
    public bool equipSecondWeap = false;

    // For its Components
    private static Animator animator;
    private Camera mainCamera;
    private Transform playerTransform;
    private CharacterController charaController;


    // For other public Components
    // Rotation
    public Transform targetTform;
    public LayerMask mouseAimMask;
    public Transform weaponsTransf;
    public GameObject beretta;
    public GameObject firstWeapon;
    public GameObject secondWeapon;

    #region Sound
    public AudioClip walkSound;
    public AudioClip runSound;
    //public AudioClip deathSound;
    public AudioSource audioSource;

    #endregion

    public Camera mainCam;
    
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
        
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        pv = 4;
        animator = GetComponent<Animator>();
        charaController = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        mainCamera = Camera.main;
        zAxis = transform.position.z;
    }


    void Update()
    {
        Debug.DrawRay(playerTransform.position  +  new Vector3(0, 0.3f, 0) , transform.up * 0.15f, Color.green);

        if (UIManager2.Instance != null)
        {
            if (UIManager2.Instance.isPaused || UIManager2.Instance.isPausedTuto)
            {
                animator.SetFloat("Speed", 0);
                return;
            }
        }  

        if (pv <= 0)
        {
            movement.x = 0;
            movement.z = 0;
            movement.y -= 15f * Time.deltaTime;
            charaController.Move(movement * Time.deltaTime);
            die = true;
            animator.SetFloat("Speed", 0);
            animator.SetBool("Death", true);
            baseSpeed = 0;
            jumpHeight = 0;
            if (die)
            {
                return;
            }
        }

        #region movement

        // MOVEMENT
        float inputX = Input.GetAxis("Horizontal");
        if (!wallJump)
        {
            movement.x = inputX * finalSpeed;
        }
        animator.SetFloat("Speed", (facingSign * movement.x) / finalSpeed);

        #endregion

        

        #region Sneaky and speed

        // SNEAKY AND SPEED                                    
        if (Input.GetKeyDown(KeyCode.LeftControl) && !sneaky)
        {
            animator.SetBool("Sneaky", true);
            sneaky = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && sneaky && RaycastSneaky())
        {
            animator.SetBool("Sneaky", false);
            sneaky = false;
            finalSpeed = baseSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift) && finalSpeed != 0 && !sneaky && die == false)
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

        #endregion
        

        #region Jump

        // JUMP
        if (charaController.isGrounded)
        {
            wallJump = false;
            animator.SetBool("WallJump", false);
            if (!isGrounded)
            {
                if (Input.GetButtonDown("Jump") && !sneaky)
                {
                    movement.y = jumpHeight;
                    isGrounded = true;
                    animator.SetBool("Jump", true);
                }
                else if (Input.GetButtonDown("Jump") && sneaky)
                {
                    movement.y = jumpHeight * 1.2f;
                    isGrounded = true;
                    animator.SetBool("Jump", true);
                    animator.SetBool("Sneaky", false);
                    sneaky = false;
                    finalSpeed = baseSpeed;
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
            movement.y -= 15f * Time.deltaTime;
        }


        #endregion
        

        #region Facing Rotation

        // FACING ROTATION
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTform.position = hit.point;
            if (!wallJump)
            {
                aimPosition = hit.point;
            }
        }

        playerTransform.rotation = (Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(aimPosition.x - transform.position.x), 0)));

        //Debug.Log(Time.frameCount + " " + movement + wallJump);

        charaController.Move(movement * Time.deltaTime);
        //Debug.Log("Raycast Sneaky : " + RaycastSneaky());

        #endregion
        
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zAxis);
    }

    private void OnAnimatorIK()
    {
        if (Input.GetMouseButton(1) && die == false)
        {
            // Weapon Aim Target
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, targetTform.position);

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

        if (other.tag == "FirstWeapon")
        {
            firstWeapon = Instantiate(beretta, weaponsTransf);
            Destroy(other.gameObject);
        }

        if (other.tag == "SecondWeapon")
        {
            Instantiate(secondWeapon,weaponsTransf);
            Destroy(other.gameObject);
            equipSecondWeap = true;
            //Debug.Log("Second Weapon : " + Instantiate(secondWeapon,weaponsTransf) + equipSecondWeap);

            if (firstWeapon.activeSelf)
            {
                firstWeapon.SetActive(false);
            }
        }

        if (other.gameObject.tag == "cut")
        {
            pv--;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnnemyBullet")
        {
            mainCam.DOShakePosition(0.3f, 0.05f);
            pv --;
        }
    }
    

    // WALL JUMP
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.DrawRay(hit.point, hit.normal, Color.red, 3f);
        if (!charaController.isGrounded && hit.normal.y < 0.1f )
        {
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetBool("WallJump", true);
                aimPosition = hit.point + hit.normal.normalized * 5;
                wallJump = true;
                finalSpeed = baseSpeed;
                movement = hit.normal * finalSpeed;
                movement.y = jumpHeight;
                //Debug.Log(Time.frameCount + " Hit "  + movement);
            }
        }
    }

    public void DeathAnimation()
    {
        die = true;
    }

    // Raycast for sneaky
    bool RaycastSneaky()
    {
        // Raycast for detect the collision with other object at top
        Debug.DrawRay(playerTransform.position + new Vector3(0,2,0), transform.up * 2f, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(playerTransform.position + new Vector3(0, 0.2f, 0), transform.up * 0.15f, out hit,0.2F))
        {
            return false;
        }
        return true;
    }

    // First méthod for adapt the height of the CharacterController / Malheureusement au bout d'un moment la technique avec le lancement de la coroutine et le return ne marchait plus 
   /* IEnumerator DeathCollider()
    {
        yield return new WaitForSeconds(1.2f);
        charaController.height = 0f;
    } */ 

    // La deuxième méthode va être de dupliquer l'animation afin d'en avoir une qui ne soit pas en mode Read Only
    // Après cela importer un skin sur la scene et y ajouter un Characontroller / Faire glisser l'animation sur "Animator" dans l'inspecteur afin qu'un ControllerAnimator se créé
    // Ajouter la "property" CharaController.Height sur l'animation et adapter comme l'on souhaite
    // Enfin dans l'animator du player, changer l'animation sneaky de base par celle modifier / Possible de supprimer le skin importer sur la scène après
}
