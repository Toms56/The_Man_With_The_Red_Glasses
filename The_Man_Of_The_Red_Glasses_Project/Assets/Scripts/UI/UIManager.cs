using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Panels
    public GameObject panelUIGame;
    [SerializeField] GameObject panelPause;
    public GameObject panelBerreta;
    public GameObject panelThompson;
    #endregion

    #region Texts
    public Text magazineTxt;
    #endregion

    #region Img

    [SerializeField] Image circleWeapons;

    #endregion

    private bool isMuted;
    public static bool isPaused;
    

    void Start()
    {
        #region  sound
        AudioListener.pause = false;
        #endregion
        isPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            isPaused = true;
            panelPause.SetActive(true);
        }

        if (PlayerController.Instance.firstWeapon.activeSelf)
        {
            panelThompson.SetActive(false);
            panelBerreta.SetActive(true);
            magazineTxt.text = "" +  Beretta.Instance.magazine;
        }
        else
        {
            panelBerreta.SetActive(false);
            panelThompson.SetActive(true);
            magazineTxt.text = "" + Thompson.Instance.magazine;
        }
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


    public void Onclick_Play()
    {
        isPaused = false;
    }

    public void Onclick_Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnCLickRetry(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
