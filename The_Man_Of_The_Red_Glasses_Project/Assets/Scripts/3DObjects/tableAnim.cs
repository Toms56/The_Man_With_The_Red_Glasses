using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tableAnim : MonoBehaviour
{
    [SerializeField]private float healhPts;

    public Animator anim;

    public Collider cl;
    // Start is called before the first frame update
    void Start()
    {
        cl.enabled = false;
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
            Debug.Log("Table HIT");
            cl.enabled = true;
            anim.SetBool("fall", true);
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
