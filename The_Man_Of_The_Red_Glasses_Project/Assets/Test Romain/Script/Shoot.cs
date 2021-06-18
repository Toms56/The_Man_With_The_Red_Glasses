using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform gunTransform;
    public GameObject bullet;

    /*#region Sound
    public AudioClip shotSound;
    public AudioSource audioSource;
    #endregion*/

    /*private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
    }

    void Shooting()
    {
        //audioSource.clip = shotSound;
        Instantiate(bullet, gunTransform.position, gunTransform.rotation, gunTransform.transform);
        //audioSource.Play();
    }
}
