using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<GameObject> spawnedEnemy = new List<GameObject>();
    [SerializeField]private GameObject enemy;
    [SerializeField]private GameObject[] spawnPoints;
    [SerializeField]private float spavnDelay;
    [SerializeField]private bool changePower,spawnWave;
    [SerializeField]private int maxEnemyOnWave;
    private int i = 1,rnd;
    public bool color = false;
    private float power = 0;
    private float nextTimeToSpavn;
    private void Update()
    {
        if(Time.time >= nextTimeToSpavn)
        {
            if(!spawnWave || (spawnWave && i <= maxEnemyOnWave))
            {
                rnd = Random.Range(0 , spawnPoints.Length);
                GameObject enemy1 = Instantiate(enemy, spawnPoints[rnd].transform.position, spawnPoints[rnd].transform.rotation);
                spawnedEnemy.Add(enemy1);
                if(changePower)
                {
                    enemy1.transform.localScale = enemy1.transform.localScale + new Vector3(power,power,0);
                    //enemy1.GetComponent<HealthSystem>().maxHealth += power;
                    if(power<5)
                        power += 0.02f;
                }
                nextTimeToSpavn = Time.time + spavnDelay;
                i++;
            }
            else
            {
                int k = 0;
                int r = 0;
                while(k < spawnedEnemy.Count)
                {
                    if(!spawnedEnemy[k])
                        r++;
                    k++;
                }
                if(r == spawnedEnemy.Count)
                    i = 1;
            }
        }
    }
}
