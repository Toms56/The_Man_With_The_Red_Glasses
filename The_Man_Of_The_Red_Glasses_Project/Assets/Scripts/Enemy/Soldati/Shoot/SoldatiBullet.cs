using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldatiBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    public Transform target;

    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        var pos = target.position;
        pos.y = transform.position.y;
        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        moveDirection = pos - transform.position;
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * bulletSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player HIT buy bullet");
            Destroy(gameObject);
        }    
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }*/
}
