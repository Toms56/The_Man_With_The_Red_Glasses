using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldatiBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-target.transform.position * bulletSpeed * Time.deltaTime);
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
