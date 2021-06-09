using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldatiBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * bulletSpeed * Time.deltaTime);
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
