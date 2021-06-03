using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static bool isPaused;
    [SerializeField] GameObject panelPause;

    void Start()
    {
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
    }

    public void Onclick_Play()
    {
        isPaused = false;
    }

    public void Onclick_Menu()
    {
        SceneManager.LoadScene(0);
    }
}
