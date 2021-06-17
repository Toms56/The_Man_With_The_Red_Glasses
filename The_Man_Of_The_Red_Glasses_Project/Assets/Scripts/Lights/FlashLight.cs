using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public float totalSeconds;     // The total of seconds the flash wil last
    public float maxIntensity;     // The maximum intensity the flash will reach
    public Light myLight;        // Your light


    private void Update()
    {
        StartCoroutine(flashNow());
    }

    public IEnumerator flashNow ()
    {
        float waitTime = totalSeconds / 2;                        
        // Get half of the seconds (One half to get brighter and one to get darker)
        while (myLight.intensity < maxIntensity) {
            myLight.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (myLight.intensity > 0) {
            myLight.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        yield return null;
    }
}
