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

    private Vector3 bulletOffset;

    [SerializeField]
    private float fireRate;

    private float time = 0f;

    private int n = 6;
    // Start is called before the first frame update
    void Start()
    {
        /*StartShooting();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (n >= 0)
        {
            SpawnBullet();
        }
        if (n <= 0)
        {
            StartCoroutine(Shoot());  
        }
        //SpawnBullet();
        if (gameObject.activeSelf == false)
        {
            GetComponent<SoldatiCanon>().enabled = false;
            CancelInvoke("SpawnBullet");
        }
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    IEnumerator Shoot()
    {
        //Sons de reload
        yield return new WaitForSeconds(3);
        n = 6;
        SpawnBullet();
    }
    
    void SpawnBullet()
    {
        if (Time.time > time)
        {
            Debug.Log(n);
            Instantiate(soldatiBullet, transform.position + bulletOffset, transform.rotation);
            time = Time.time + fireRate;
            n -= 1;
        }
    }
}
