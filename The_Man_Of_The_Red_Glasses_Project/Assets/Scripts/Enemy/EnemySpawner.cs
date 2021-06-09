using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
     //private int spawnCount = 3;

    private int n;
    public GameObject enemy;
    public GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemies = new GameObject[n];
        InvokeRepeating("SpawnEnemy", 3f, 4f);
    }


    void SpawnEnemy()
    {
        if (n < 1)
        {
            //enemy.layer = 1000;
            Instantiate(enemies[Random.Range(0,enemies.Length)], transform.position, transform.rotation);
            n = n + 1;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (n >= 1)
        {
            CancelInvoke("SpawnEnemy");
        }
    }
}
