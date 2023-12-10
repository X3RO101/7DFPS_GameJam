using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for actual spawning of the enemies
 * WaveManager calls Spawn Function on this spawnpoint
 */
public class SpawnPoint : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnZombie(int enemyCount, float statMultiplier, float statAdditive)
    {
        GameObject temp = null;
        for (int i = 0; i < enemyCount; ++i)
        {
            temp = GameManager.inst.gpManager.enemyObjectPool.GetPooledZombie();
            temp.transform.position = this.transform.position;
            // TO ADD ENEMY STATS CODE HERE, modify healthcomponent
            temp.SetActive(true);
        }
    }
    public void SpawnCharger(int enemyCount, float difficultyScale)
    {

    }
}
