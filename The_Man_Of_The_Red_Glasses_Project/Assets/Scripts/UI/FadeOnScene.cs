using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOnScene : MonoBehaviour
{

    public Image img;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeScreen(true));
    }

    IEnumerator FadeScreen(bool fadeOn)
    {
        // fade from opaque to transparent
        if (fadeOn)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                img.color = new Color(0,0,0,i);
                yield return null;
            }
        }
    }
}
