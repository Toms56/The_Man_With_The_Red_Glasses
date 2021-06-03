using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSwitch : MonoBehaviour
{ 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.equipSecondWeap)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                SwitchWeapons();
                Debug.Log("Scroll fonctionnel");
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                SwitchWeapons();
                Debug.Log("Scroll fonctionnel 2 ");
            }
        }
    }

    void SwitchWeapons()
    {
        foreach (Transform weapons in transform)
        {
            weapons.gameObject.SetActive(!weapons.gameObject.activeSelf);
        }
    }
}
