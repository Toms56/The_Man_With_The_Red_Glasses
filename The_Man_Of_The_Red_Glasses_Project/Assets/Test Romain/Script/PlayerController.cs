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
    GameObject firstWeapon;
    public GameObject secondWeapon;

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
        mainCamera = Camera.main;
    }


    void Update()
    {
        if (pv == 0 && !die)
        {
            // Mort du joueur / Empêcher le shoot 
            animator.SetBool("Death", true);
            die = true;
            if (die)
            {
                StartCoroutine(DeathCollider());
                return;
            }
        }

        // MOVEMENT
        float inputX = Input.GetAxis("Horizontal");
        movement.x = inputX * finalSpeed;
        animator.SetFloat("Speed", (facingSign * movement.x) / finalSpeed);


        // SNEAKY AND SPEED                                    
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
        if (charaController.isGrounded)
        {
            if (!isGrounded)
            {
                if (Input.GetButtonDown("Jump") && !sneaky)
                {
                    movement.y = jumpHeight;
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
        //Debug.Log(charaController.height);
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

            if (firstWeapon.activeSelf)
            {
                firstWeapon.SetActive(false);
            }
        }
    }

    // First méthod for adapt the height of the CharacterController
    IEnumerator DeathCollider()
    {
        yield return new WaitForSeconds(1.2f);
        charaController.height = 0f;
    }

    // La deuxième méthode va être de dupliquer l'animation afin d'en avoir une qui ne soit pas en mode Read Only
    // Après cela importer un skin sur la scene et y ajouter un Characontroller / Faire glisser l'animation sur "Animator" dans l'inspecteur afin qu'un ControllerAnimator se créé
    // Ajouter la "property" CharaController.Height sur l'animation et adapter comme l'on souhaite
    // Enfin dans l'animator du player, changer l'animation sneaky de base par celle modifier / Possible de supprimer le skin importer sur la scène après
}
