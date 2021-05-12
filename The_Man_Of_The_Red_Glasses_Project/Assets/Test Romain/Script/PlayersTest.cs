using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersTest : MonoBehaviour
{
    public CharacterController controller;
    private float finalSpeed;
    public float baseSpeed;

    [SerializeField] Animator animator;
    [SerializeField] Animator animatorCollider;
    bool isJumping = false;
    private bool accroupi;
    Vector3 mouvement = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        accroupi = false;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        //float inputZ = Input.GetAxis("Vertical");

        mouvement.x = inputX * finalSpeed;
        //mouvement.z = inputZ * speed;

        animator.SetFloat("Speed", (Mathf.Abs(mouvement.x) + Mathf.Abs(mouvement.z)) * finalSpeed);

        if (controller.isGrounded)
        {
            if (!isJumping)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    mouvement.y = 7f;
                    isJumping = true;
                }
            }
            else
            {
                mouvement.y = 0;
                isJumping = false;
            }
        }
        else
        {
            mouvement.y -= 9.81f * Time.deltaTime;
        }

        Debug.Log(animatorCollider.GetBool("Accroupi") /*+ " " + "Var : " + accroupi)*/);

        if (Input.GetKeyDown(KeyCode.LeftControl) && !accroupi)
        {
            animatorCollider.SetBool("Accroupi", true);
            accroupi = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && accroupi)
        {
            animatorCollider.SetBool("Accroupi", false);
            accroupi = false;
            finalSpeed = baseSpeed;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            finalSpeed = baseSpeed * 2;
        }
        else if (accroupi)
        {
            finalSpeed = baseSpeed / 1.5f;
        }
        else
        {
            finalSpeed = baseSpeed;
        }

        controller.Move(mouvement * Time.deltaTime);
        Debug.Log(finalSpeed + " Speed");
    }

    void Gravity()
    {
        if (controller.isGrounded)
        {
            mouvement.y = 0;
        }
        else
        {
            mouvement.y += -.81f * Time.deltaTime;
        }
    }
}
