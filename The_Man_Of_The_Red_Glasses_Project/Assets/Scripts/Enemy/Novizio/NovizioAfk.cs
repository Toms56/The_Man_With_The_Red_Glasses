using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovizioAfk : MonoBehaviour
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("Enemy Hit ! ");
        }
    }
}
