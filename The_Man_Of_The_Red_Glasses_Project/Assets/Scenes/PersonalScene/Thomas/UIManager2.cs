using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager2 : MonoBehaviour
{
    #region Panels
    //public GameObject panelUIGame;
    [SerializeField] GameObject panelPause;
    public GameObject panelLettre3;
    public GameObject panelWin;

    #endregion
    #region Texts
    public Text magasineTxt;
    #endregion

    #region Img

    public Image berettaUsed;
    public Image berettaNotUsed;

    public Image thompsonUsed;
    public Image thompsonNotUsed;

    #endregion

    private bool isMuted;
    public static bool isPaused;
    public bool levelFinish;
    

    void Start()
    {
        levelFinish = false;
        Time.timeScale = 1;
        #region  sound
        AudioListener.pause = false;
        #endregion
        thompsonNotUsed.enabled = false;
        thompsonUsed.enabled = false;
        berettaUsed.enabled = false;
        berettaNotUsed.enabled = false;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }else if (isPaused)
            {
                Resume();
            }
        }

        if (isPaused && levelFinish == false)
        {
            panelPause.SetActive(true);
        }else if (!isPaused)
        {
            panelPause.SetActive(false);
        }

        if (levelFinish)
        {
            panelWin.SetActive(true);
            Time.timeScale = 0;
        }

        #region MagasineManagement
        magasineTxt.text = " " + Weapons.magazine;
        #endregion

        if (Beretta.Instance)
        {
            thompsonNotUsed.enabled = true;
            thompsonUsed.enabled = false;
            berettaUsed.enabled = true;
            berettaNotUsed.enabled = false;
        }

        if (Thompson.Instance == null)
        {
            thompsonNotUsed.enabled = false;
            thompsonUsed.enabled = false;
        }

        if (Thompson.Instance)
        {
            thompsonNotUsed.enabled = false;
            thompsonUsed.enabled = true;
            berettaUsed.enabled = false;
            berettaNotUsed.enabled = true;
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

    /*public void Onclick_Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnCLickRetry(int scene)
    {
        SceneManager.LoadScene(scene);
    }*/
}
