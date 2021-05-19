using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    private float dirX;

    [SerializeField] private float speed;

    private Rigidbody rb;

    private Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        dirX = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -9f)
        {
            dirX = 1f;
        }else if (transform.position.x > 9f)
        {
            dirX = -1f;
        }

        if (transform.position.y > 4)
        {
            
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "BigStump":
                rb.AddForce(Vector2.up * 300f);
                break;
        }
    }
}
