using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableAnim : MonoBehaviour
{
    [SerializeField]private float healhPts;
    private Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healhPts <= 0)
        {
            healhPts = 0;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.Play("table");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnnemyBullet")
        {
            healhPts--;
        }
    }
}
