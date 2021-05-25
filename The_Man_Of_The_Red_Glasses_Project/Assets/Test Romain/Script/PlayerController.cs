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
    private float mouvement;
    private bool accroupi;
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
        mainCamera = Camera.main;
    }


    void Update()
    {
        
    }
}
