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
    

    void Start()
    {
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
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            /*novizio.clip = deathClip;
            novizio.Play();
            Debug.Log(deathClip);*/
            StartCoroutine(Destroy());
        }
    }
    IEnumerator Destroy()
    {
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
