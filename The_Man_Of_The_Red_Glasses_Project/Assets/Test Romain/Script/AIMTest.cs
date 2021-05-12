using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMTest : MonoBehaviour
{
    public Transform gunTransform;
    public GameObject bullet;

    private Vector2 lookDirection;
    private float lookAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject firedBullet = Instantiate(bullet, gunTransform.position, gunTransform.rotation);
        firedBullet.GetComponent<Rigidbody>().velocity = gunTransform.right * 10f;
    }
}
