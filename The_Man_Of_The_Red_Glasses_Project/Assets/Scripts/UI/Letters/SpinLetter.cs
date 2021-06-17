using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpinLetter : MonoBehaviour
{

    public GameObject panelLettre3; 
    void Update(){
        Spin();
    }
    
    void Spin()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0 );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            panelLettre3.SetActive(true);
        }
    }
}
