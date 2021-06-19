using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Camera cam;
    private float startingFOV;

    public float minFOV;
    public float maxFOV;
    public float zoomRate;

    private float currentFOV;
    Transform target;
    public float speed;
    
    public Transform playerTransform;
    [Range(0.01f, 1.0f)]
    //public float smoothFactor = 0.5f;
    private Vector3 camOffset;
 
    void Start()
    {
        camOffset = transform.position - playerTransform.position;

        cam = GetComponent<Camera>();
        startingFOV = cam.fieldOfView;
        
        target = Camera.main.transform;
    }
     
    void Update ()
    {

        currentFOV = cam.fieldOfView;

        if (Input.GetKey(KeyCode.Tab))
        {
            currentFOV += zoomRate;
        }
        else
        {
            currentFOV -= zoomRate;
        }

        currentFOV = Mathf.Clamp(currentFOV, minFOV, maxFOV);
        cam.fieldOfView = currentFOV;
        
        Vector3 targetPosition = playerTransform.position + camOffset;
 
        target.position = Vector3.Lerp(target.position, targetPosition, Time.deltaTime * speed);
    }
}
