using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovizioAfk : MonoBehaviour
{

    public float healthPts;
    public Animator animator;

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("Hit!");
            Debug.Log(healthPts);
            healthPts--;
            if (healthPts <= 0)
            {
                Destroy(gameObject);
            }
        }    
    }
}
