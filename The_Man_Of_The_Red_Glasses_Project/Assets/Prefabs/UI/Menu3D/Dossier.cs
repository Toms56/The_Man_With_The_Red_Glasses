using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dossier : MonoBehaviour
{

    public GameObject dossier;
    public Animator anim;

    private Animation animation;
    public float timePage = 1f;

    bool fileOpen;
    public GameObject mainMenuPage;
    
    public GameObject[] creditsPages;
    public GameObject creditsPage1;
    public GameObject creditsPage2;
    public GameObject creditsPage3;

    public GameObject optionPage;
    
    public AudioSource audioSource;

    public AudioClip pageSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = pageSound;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.forward, Color.green);
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag("main"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    
                    OnClickOpen();
                    mainMenuPage.SetActive(true);
                }
            }
            else if (selection.CompareTag("option") && !fileOpen)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    anim.SetBool("open", true);
                    fileOpen = true;
                    OnClickParameters();
                }
            }else if (selection.CompareTag("Credits") && !fileOpen)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.Play();
                    anim.SetBool("open", true);
                    fileOpen = true;
                    creditsPage1.SetActive(true);
                } 
            }
            else if (selection.CompareTag("option") && fileOpen)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnClickParameters();
                }
            }else if (selection.CompareTag("Credits") && fileOpen)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.Play();

                    creditsPage1.SetActive(true);
                } 
            }
            else if (selection.CompareTag("credits1"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.Play();
                    creditsPage1.SetActive(true);

                    creditsPage2.SetActive(false);
                } 
            }
            else if (selection.CompareTag("credits2"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.Play();
                    creditsPage1.SetActive(false);
                    creditsPage3.SetActive(false);
                    creditsPage2.SetActive(true);
                } 
            }
            else if (selection.CompareTag("credits3"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.Play();

                    creditsPage2.SetActive(false);
                    creditsPage3.SetActive(true);
                } 
            }
            else if (selection.CompareTag("closeCredits"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    audioSource.Play();

                    creditsPage1.SetActive(false);
                    creditsPage2.SetActive(false);
                } 
            }
            else if (selection.CompareTag("muteOn"))
            {
                MuteOn();
            }
            else if (selection.CompareTag("muteOff"))
            {
                MuteOff();
            }
            else if (selection.CompareTag("loadTuto"))
            {
                LoadTuto();
            }
            else if (selection.CompareTag("loadLvl1"))
            {
                LoadLvl1();
            }
            else if (selection.CompareTag("leaveGame"))
            {
                LeaveGame();
            }
            else if (selection.CompareTag("mainMenu"))
            {
                for (int i = 0; i < creditsPages.Length; i++)
                {
                    creditsPages[i].SetActive(false);
                }
                optionPage.SetActive(false);
                mainMenuPage.SetActive(true);
            }
        }
    }

    public void OnClickOpen()
    {
        audioSource.Play();
        anim.SetBool("open", true);
        fileOpen = true;
    }

    public void OnClickParameters()
    {
        for (int i = 0; i < creditsPages.Length; i++)
        {
            creditsPages[i].SetActive(false);
        }
        audioSource.Play();
        optionPage.SetActive(true);
    }

    public void LoadTuto()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLvl1()
    {
        SceneManager.LoadScene(2);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void MuteOn()
    {
        AudioListener.pause = true;
    }
    public void MuteOff()
    {
        AudioListener.pause = false;
    }

    public void DisplayCredits1()
    {
        audioSource.Play();

        creditsPage1.SetActive(true);
    }
    public void DisplayCredits2()
    {
        audioSource.Play();

        creditsPage2.SetActive(true);
    }

    IEnumerator pageParameter()
    {
        
        Debug.Log("Jsuis la wsh");
        yield return new WaitForSeconds(0.5f);
        audioSource.Play();
        anim.SetBool("page2", true);
    }
}
