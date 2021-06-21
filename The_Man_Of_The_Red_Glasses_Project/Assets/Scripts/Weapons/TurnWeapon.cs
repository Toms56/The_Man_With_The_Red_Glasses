using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnWeapon : MonoBehaviour
{
    void Update(){
        Spin();
    }
    
    void Spin()
    {
        transform.Rotate(0, 0, 60 * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
