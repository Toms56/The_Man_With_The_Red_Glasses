using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Transform targetTransform;
    public GameObject bullet;

    public int magazine;
    public int bulletSpeed;

    public float fireRate;
    private float time = 0;
    
    #region Sound
    public AudioClip shotSound;
    public AudioClip notBulletSound;
    public AudioSource audioSource;

    public Camera mainCam;
    #endregion
    
    private void Awake()
    {
        audioSource = mainCam.GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //mainCam.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (magazine <= 0 && Input.GetMouseButtonDown(0))
        {
            audioSource.clip = notBulletSound;
            audioSource.Play();
        }
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
            audioSource.clip = shotSound;
            GameObject firedBullet = Instantiate(bullet, targetTransform.position, targetTransform.rotation);
            audioSource.Play();
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
