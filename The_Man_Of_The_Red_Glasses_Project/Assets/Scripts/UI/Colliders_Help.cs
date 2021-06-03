using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders_Help : MonoBehaviour
{
    [SerializeField] GameObject panel;
    private bool pastPlayer;
    public int numbers;

    // Start is called before the first frame update
    void Start()
    {
        pastPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !pastPlayer && numbers == 1)
        {
            UIManager.isPaused = true;
            panel.SetActive(true);
            pastPlayer = true;
        }

        if (other.tag == "Player" && !pastPlayer && numbers == 2 && PlayerController.Instance.equipSecondWeap)
        {
            UIManager.isPaused = true;
            panel.SetActive(true);
            pastPlayer = true;
        }
    }
}
