using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager2 : MonoBehaviour
{

     public static UIManager2 Instance;
    #region Panels
    //public GameObject panelUIGame;
    [SerializeField] GameObject panelPause;
    public GameObject panelLettre3;
    public GameObject panelWin;
    public GameObject panelBerreta;
    public GameObject panelThompson;
    public GameObject circleWeapons;
    public GameObject panelmagazine;

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
            Time.timeScale = 0;
        }

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
}
