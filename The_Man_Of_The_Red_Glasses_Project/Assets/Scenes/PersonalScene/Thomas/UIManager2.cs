using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager2 : MonoBehaviour
{
    public bool isPausedTuto;

    public Animator anim;
    public GameObject panelFinal;
    public GameObject panelHealth;
    public static UIManager2 Instance;

     public Image pv4;
     public Image pv3;
     public Image pv2;
     public Image pv1;
     
    #region Panels
    //public GameObject panelUIGame;
    [SerializeField] GameObject panelPause;
    public GameObject panelLettre3;
    public GameObject panelWin;
    public GameObject panelBerreta;
    public GameObject panelThompson;
    public GameObject circleWeapons;
    public GameObject panelmagazine;
    public GameObject panelGameOver;

    public AudioSource audioSource;
    public AudioClip endSound;
    #endregion

    #region Texts
    public Text magazineTxt;
    #endregion

    private bool isMuted;
    
    public bool isPaused;
    public bool levelFinish;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
        panelBerreta.SetActive(false);
        panelThompson.SetActive(false);
        circleWeapons.SetActive(false);
        panelmagazine.SetActive(false);
        levelFinish = false;
        Time.timeScale = 1;
        #region  sound
        AudioListener.pause = false;
        #endregion
        //isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            panelBerreta.SetActive(false);
            panelThompson.SetActive(false);
            circleWeapons.SetActive(false);
            panelmagazine.SetActive(false);
            panelHealth.SetActive(false);
        }else if (!isPaused)
        {
            panelBerreta.SetActive(true);
            panelThompson.SetActive(true);
            circleWeapons.SetActive(true);
            panelmagazine.SetActive(true);
            panelHealth.SetActive(true);
        }
        #region HealthBar

        if (PlayerController.Instance.pv == 0)
        {
            StartCoroutine(Die());
        }

        if (PlayerController.Instance.pv == 4)
        {
            pv4.enabled = true;
            pv3.enabled = false;
            pv2.enabled = false;
            pv1.enabled = false;
        }else if (PlayerController.Instance.pv == 3)
        {
            pv4.enabled = false;
            pv3.enabled = true;
            pv2.enabled = false;
            pv1.enabled = false; 
        }else if (PlayerController.Instance.pv == 2)
        {
            pv4.enabled = false;
            pv3.enabled = false;
            pv2.enabled = true;
            pv1.enabled = false;
        }else if (PlayerController.Instance.pv == 1)
        {
            pv4.enabled = false;
            pv3.enabled = false;
            pv2.enabled = false;
            pv1.enabled = true;
        }
        else if (PlayerController.Instance.pv == 0)
        {
            pv4.enabled = false;
            pv3.enabled = false;
            pv2.enabled = false;
            pv1.enabled = false;
        }
        #endregion
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else if (isPaused)
            {
                Resume();
            }
        }

        if (isPaused && levelFinish == false)
        {
            panelPause.SetActive(true);
        }
        else if (!isPaused)
        {
            panelPause.SetActive(false);
        }

        if (levelFinish)
        {
            panelWin.SetActive(true);
            audioSource.Stop();
            Time.timeScale = 0;
        }

        if (PlayerController.Instance.firstWeapon != null && isPaused == false)
        {
            if (PlayerController.Instance.firstWeapon.activeSelf)
            {
                panelBerreta.SetActive(true);
                panelmagazine.SetActive(true);
                magazineTxt.text = "" + Beretta.Instance.magazine;
                panelThompson.SetActive(false);
            
                circleWeapons.SetActive(true);           
            }
            else
            {
                panelBerreta.SetActive(false);
                panelThompson.SetActive(true);
                magazineTxt.text = "" + Thompson.Instance.magazine;
            }  
        }
        
    }

    public void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    public void OnClickDisplayWin()
    {
        levelFinish = true;
    }
    public void Onclick_Play()
    {
        isPausedTuto = false;
    }

    public void Mute(int i)
    {
        if (i == 0)
        {
            AudioListener.pause = true;
        }else if (i == 1)
        {
            AudioListener.pause = false;
        }
    }

    public void Onclick_ChangeScene( int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2.5f);
        Time.timeScale = 0;
        panelGameOver.SetActive(true);
        panelBerreta.SetActive(false);
        panelThompson.SetActive(false);
        circleWeapons.SetActive(false);
        panelmagazine.SetActive(false);
        panelHealth.SetActive(false);
    }
    public void OnClickEndGame()
    {
        audioSource.clip = endSound;
        audioSource.Play();
        panelBerreta.SetActive(false);
        panelThompson.SetActive(false);
        circleWeapons.SetActive(false);
        panelmagazine.SetActive(false);
        panelHealth.SetActive(false);
        panelFinal.SetActive(true);
        anim.SetBool("endGame", true);
    }
}
