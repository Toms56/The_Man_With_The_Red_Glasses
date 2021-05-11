using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    Transform target;
    public Transform Player;
    public float speed;
 
    void Start()
    {
        target = Camera.main.transform;
    }
     
    void Update () {
 
        Vector3 targetPosition = new Vector3(Player.position.x, target.position.y, target.position.z);
 
        target.position = Vector3.Lerp(target.position, targetPosition, Time.deltaTime * speed);
    }
}
