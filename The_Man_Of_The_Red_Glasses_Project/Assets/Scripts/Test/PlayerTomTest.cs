using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTomTest : MonoBehaviour
{
    public CharacterController controller;
    public float speed;

    Vector3 mouvement = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // Creation of the movement /use of "transform.right" to move on the X axis and use of "transform.froward" to move on the z axis
        mouvement.x =  inputX * speed;
        mouvement.z =  inputZ * speed;
        
        mouvement.y -= 9.81f * Time.deltaTime;
            
        //Debug.Log("Grounded : " + controller.isGrounded); 
        controller.Move(mouvement * Time.deltaTime);

    }
    
    void Gravity()
    {
        if (controller.isGrounded)
        {
            mouvement.y = 0;
        }
        else
        {
            mouvement.y += -50f * Time.deltaTime;
        }
    }
}
