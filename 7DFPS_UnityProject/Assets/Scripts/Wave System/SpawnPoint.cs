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

    public void SpawnZombie(int enemyCount, float statMultiplier, float statAdditive, GameplayManager.ELEMENTS elementType)
    {
        GameObject temp = null;
        EnemyObject tempEnemyObj = null;
        for (int i = 0; i < enemyCount; ++i)
        {
            temp = GameManager.inst.gpManager.enemyObjectPool.GetPooledZombie();
			tempEnemyObj = temp.GetComponent<EnemyObject>();
			temp.transform.position = this.transform.position;
            // TO ADD ENEMY STATS CODE HERE, modify healthcomponent, element etc
            // Enemy HP
            tempEnemyObj.hp.SetMaxHealth((int)((tempEnemyObj.hp.GetMaxHealth() * statMultiplier) + statAdditive));
            tempEnemyObj.hp.SetCurrentHealth(tempEnemyObj.hp.GetMaxHealth());

            // Enemy Element
            tempEnemyObj.element = elementType;
            tempEnemyObj.normalMat = tempEnemyObj.elementMaterialList[(int)elementType];
            tempEnemyObj.mr.material = tempEnemyObj.normalMat;
            // Add damage code
            temp.SetActive(true);

            // Add this enemy to a list to keep track of all alive enemies in the current wave
            GameManager.inst.gpManager.waveManager.aliveEnemies.Add(temp);
        }
    }
    public void SpawnCharger(int enemyCount, float difficultyScale)
    {

    }
}
