using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour
{
    public List<GameObject> enemyPrefabList = new List<GameObject>();
    public int poolSize;

    // For each type of enemy create a list to store those prefabs
    private List<GameObject> zombieEnemyPool;

	// Start is called before the first frame update
	void Start()
    {
		
    }

	public void Init()
    {
		zombieEnemyPool = new List<GameObject>();
		GameObject temp = null;
		for (int i = 0; i < poolSize; ++i)
		{
			temp = Instantiate(enemyPrefabList[(int)GameplayManager.ENEMY_TYPE.ZOMBIE]);
			temp.SetActive(false);
			zombieEnemyPool.Add(temp);
		}
	}
	

	// Update is called once per frame
	void Update()
    {
        
    }

    public GameObject GetPooledZombie()
    {
        for (int i = 0; i < zombieEnemyPool.Count; ++i)
        {
            if (zombieEnemyPool[i].activeSelf == false)
            {
                return zombieEnemyPool[i];
            }
        }
        GameObject temp = null;
        for (int i = 0; i < 10; ++i)
        {
            temp = Instantiate(enemyPrefabList[(int)GameplayManager.ENEMY_TYPE.ZOMBIE]);
            temp.SetActive(false);
            zombieEnemyPool.Add(temp);
        }

        return temp;
    }
}
