using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPlayer : MonoBehaviour
{
    public Transform targetTransform;
    public GameObject bullet;

    [SerializeField] int magazine;
    [SerializeField] int bulletSpeed;

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
        /*  if (Input.GetMouseButton(1))
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

              else
              {
                  transform.rotation = Quaternion.Euler(new Vector3(0f,0f,weaponAngle));

              }
          } */

        if (Input.GetMouseButtonDown(0) && !PlayerNoController.Instance.die)
        {
            if (magazine != 0)
            {
                Shoot();
            }
        }   
    }

    void Shoot()
    {
        GameObject firedBullet = Instantiate(bullet, targetTransform.position, targetTransform.rotation);
        firedBullet.GetComponent<Rigidbody>().velocity = firedBullet.transform.right * 10f;
        magazine -= 1;
        //Debug.Log(gunTransform.parent.localPosition);
        //firedBullet.transform.Translate(Vector3.Lerp(gunTransform.transform.position, PlayerNoController.Instance.targetTform.transform.position,2f));
        //firedBullet.transform.Translate((Vector3.forward * 10f) * Time.deltaTime);
    }
}
