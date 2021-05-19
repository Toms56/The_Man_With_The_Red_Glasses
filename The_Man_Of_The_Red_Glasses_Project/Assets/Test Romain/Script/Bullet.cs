using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speedBullet;
    public float lifeTime;

    void Start()
    {
        Invoke("DestructBullet", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestructBullet()
    {
        Destroy(this.gameObject);
    }
}
