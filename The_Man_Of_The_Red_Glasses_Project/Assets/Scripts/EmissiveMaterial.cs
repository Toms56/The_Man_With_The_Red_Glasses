using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissiveMaterial : MonoBehaviour
{
    private Color color;
    private float f = 0.4f;

    // Start is called before the first frame update
    /*void Start()
    {
        GetComponent<Renderer>().material.SetColor("_EmissionColor",color*f);
        rend.material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        f = getIntensity();
        Color color = Color.red * f;
        rend.material.SetColor("_EmissionColor", color);
    }*/
}
