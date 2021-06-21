using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dossier : MonoBehaviour
{

    public GameObject dossier;
    public Animator anim;

    public float timePage = 1f;

    bool fileOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickOpen()
    {
        anim.SetBool("open", true);
        fileOpen = true;
    }

    public void OnClickParameters()
    {
        if (!fileOpen)
        {
            anim.SetBool("open", true);
            StartCoroutine(pageParameter());
        }else if (fileOpen)
        {
            anim.SetBool("page2", true);
        }
    }

    IEnumerator pageParameter()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("page2", true);
    }
}
