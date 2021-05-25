using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 camOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;
   
    void Start()
    {
        camOffset = transform.position - playerTransform.position;
    }

    void Update()
    {
        Vector3 newPos = playerTransform.position + camOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
    }
}
