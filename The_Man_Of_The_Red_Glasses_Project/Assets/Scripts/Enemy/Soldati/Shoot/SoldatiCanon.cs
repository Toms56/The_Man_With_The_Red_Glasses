using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldatiCanon : MonoBehaviour
{

    public Transform target;
    
    //Shoot
    public GameObject soldatiBullet;

    private Vector3 bossBulletOffset;

    [SerializeField]
    private float fireRate;

    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        StartShooting();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        StartShooting();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    
    void SpawnBullet()
    {
        if (Time.time > time)
        {
            Instantiate(soldatiBullet, transform.position + bossBulletOffset, transform.rotation);
            time = Time.time + fireRate;
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //utiliser ds boss
        }
    }
    
    public void StartShooting()
    {
        InvokeRepeating("SpawnBullet", 2f, 0.40f);
    }
}
