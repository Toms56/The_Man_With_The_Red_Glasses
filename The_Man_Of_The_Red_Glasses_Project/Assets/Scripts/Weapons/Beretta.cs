using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beretta : Weapons
{
    public static Beretta Instance;

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

    protected override void  Update()
    {
        base.Update();
    }
}
