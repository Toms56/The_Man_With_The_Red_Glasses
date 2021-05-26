using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
        /*StartShooting();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();*/
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.activeSelf + "CANON");
        //InvokeRepeating("SpawnBullet", 0.5f, 0.40f);
        SpawnBullet();
        if (gameObject.activeSelf == false)
        {
            GetComponent<SoldatiCanon>().enabled = false;
            CancelInvoke("SpawnBullet");
        }
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    
    void SpawnBullet()
    {
        Debug.Log("Function Called");
        if (Time.time > time)
        {
            Instantiate(soldatiBullet, transform.position + bossBulletOffset, transform.rotation);
            time = Time.time + fireRate;
        }
    }
}
