using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPlayer : MonoBehaviour
{
    public GameObject gun;
    public Transform gunTransform;
    public GameObject bullet;



    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - gunPos.x;
            mousePos.y = mousePos.y - gunPos.y;
            float weaponAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(180f,0f,-weaponAngle));
            }

            if (Input.GetMouseButtonDown(0)){
                Shoot();
            }

            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f,0f,weaponAngle));

            }
        }
    }

    void Shoot()
    {
        GameObject firedBullet = Instantiate(bullet, gunTransform.position, gunTransform.rotation);
        firedBullet.GetComponent<Rigidbody>().velocity = gunTransform.right * 10f;
    }

}
