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
        if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerController.Instance.equipSecondWeap)
        {
            SwitchWeapons();
        }
    }

    void SwitchWeapons()
    {
        foreach(Transform weapons in transform)
        {
            weapons.gameObject.SetActive(!weapons.gameObject.activeSelf);
        }
    }
}
