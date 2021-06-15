using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovizioAfk : MonoBehaviour
{

    public Animator animator;
    private Rigidbody rb;
    
    #region UI
    public float healthPts;
    [SerializeField] private float maxHealth;
    public GameObject healthBarUI;
    public Slider slider;
    #endregion
    
    #region Sound

    public float volume = 0.5f;
    //public AudioClip runSound;
    public AudioClip deathSound;
    public AudioSource audioSource;
    #endregion

    private bool doOnce;
    private float zAxis;
    void Start()
    {
        zAxis = transform.position.z;
        rb = GetComponent<Rigidbody>();
        healthPts = maxHealth;
        slider.value = CalculateHealth();
    }
    private void Update()
    {
        slider.value = CalculateHealth();
        if (healthPts < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (healthPts > maxHealth)
        {
            healthPts = maxHealth;
        }
        if (healthPts <= 0)
        {
            healthPts = 0;
            animator.SetBool("Death",true);
            healthBarUI.SetActive(false);
            //audioSource.Play();
            if (!doOnce)
            { 
                StartCoroutine(Destroy());
                doOnce = true;
            }
        }
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zAxis);
    }
    IEnumerator Destroy()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        rb.detectCollisions = false;
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    float CalculateHealth()
    {
        return healthPts / maxHealth;
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            healthPts--;
        }    
    }
}
