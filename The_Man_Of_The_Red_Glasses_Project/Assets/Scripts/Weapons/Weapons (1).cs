using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] GameObject bullet;

    public int magazine;
    public int bulletSpeed;


    public float fireRate;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetMouseButton(0) && !PlayerController.Instance.die && PlayerController.Instance.aiming)
        {
            if (magazine != 0)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (Time.time > time)
        {
            GameObject firedBullet = Instantiate(bullet, targetTransform.position, targetTransform.rotation);
            firedBullet.GetComponent<Rigidbody>().velocity = firedBullet.transform.right * bulletSpeed;
            firedBullet.transform.rotation = bullet.transform.rotation;
            magazine -= 1;
            time = Time.time + fireRate;
        }

        // Faire VFX ICI POUR LE SHOOT 

        //Debug.Log(gunTransform.parent.localPosition);
        //firedBullet.transform.Translate(Vector3.Lerp(gunTransform.transform.position, PlayerNoController.Instance.targetTform.transform.position,2f));
        //firedBullet.transform.Translate((Vector3.forward * 10f) * Time.deltaTime);
    }
}
