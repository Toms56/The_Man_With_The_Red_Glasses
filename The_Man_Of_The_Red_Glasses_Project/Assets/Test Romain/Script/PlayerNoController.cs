﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoController : MonoBehaviour
{
    public static PlayerNoController Instance;

    // For Mouvement
    public float jumpHeight;
    public float baseSpeed;
    private float finalSpeed;
    private float mouvement;
    private bool accroupi;

    // PV
    public int pv = 3;
    public bool die = false;

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
    private Transform tform;
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
        tform = GetComponent<Transform>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (pv == 0)
        {
            // Mort du joueur / Empêcher le shoot 
            animator.SetBool("Jump", true);
            die = true;
            return;
        }

        //Movement
        mouvement = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector3(mouvement * finalSpeed, rbody.velocity.y, 0);
        animator.SetFloat("Speed", (facingSign * rbody.velocity.x) / finalSpeed);

        // Sneaky and Speed                                             REMETTRE L ANIMATION POUR LE SNEAKY 
        if (Input.GetKeyDown(KeyCode.LeftControl) && !accroupi)
        {
            animator.SetBool("Accroupi", true);
            accroupi = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && accroupi)
        {
            animator.SetBool("Accroupi", false);
            accroupi = false;
            finalSpeed = baseSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            finalSpeed = baseSpeed * 2;
            animator.SetBool("Run", true);
        }
        else if (accroupi)
        {
            finalSpeed = baseSpeed / 1.5f;
        }
        else
        {
            finalSpeed = baseSpeed;
            animator.SetBool("Run", false);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rbody.velocity = new Vector3(rbody.velocity.x, jumpHeight, 0);
            animator.SetBool("Jump",true);
        }

        if (rbody.velocity.y == 0)
        {
            animator.SetBool("Jump", false);
        }

        Debug.Log(isGrounded);

        // Facing Rotation
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTform.position = hit.point;
        }

        rbody.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(targetTform.position.x - transform.position.x),0)));

        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask, QueryTriggerInteraction.Ignore);
    }

    private void OnAnimatorIK()
    {
        if (Input.GetMouseButton(1) && die == false)
        {
            animator.SetBool("Aiming", true);

            // Weapon Aim Target
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, targetTform.position);

            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(targetTform.position);
        }
        else
        {
            animator.SetBool("Aiming", false);
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