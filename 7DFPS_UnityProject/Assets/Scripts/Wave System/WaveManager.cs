using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Instantiated in GPManager
 * Handles scaling difficulty of the AI (Health, Damage, Frequency)
 * Handles all the spawners (Will be spawners at different locations)
 * Spawners will be magic circles at different areas, prioritise spawners which are further from the player
 */

public class WaveManager : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints; // Contains all possible spawn locations
    private List<float> spawnDistFromPlayer;

    private float spawnBounceTime = 5.0f;
    private ScalingManager sm;

    public List<GameObject> aliveEnemies; // Keeps track of all alive enemies in the current wave

	// Start is called before the first frame update
	void Start()
    {
	}

    public void Init()
    {
		spawnDistFromPlayer = new List<float>();
		spawnPoints = new List<SpawnPoint>();
        sm = GameManager.inst.gpManager.scalingManager;
        spawnBounceTime = sm.waveFrequency;
        aliveEnemies = new List<GameObject>();

	}

    // Update is called once per frame
    void Update()
    {
	    if (spawnBounceTime <= 0.0f)
        {
            // Clear the previous list of alive enemies
            aliveEnemies.Clear();
            aliveEnemies.TrimExcess();

            UpdateSpawnDistances(false);
            float enemyCount = sm.enemyCount * sm.enemyCountMultiplier;
            float[] enemyStatModiers = { sm.statMultiplier, sm.statAdditive};
            int enemyDamage = sm.enemyDamage;

            GameplayManager.ELEMENTS elementType;

            for (int i = 0; i < sm.groupSpawnCount; ++i)
            {
				elementType = sm.GetElementTypeToSpawn();

				// choose the furtherest spawnpoint
				SpawnPoint temp = ChooseSpawnPoint();

                // check with scaling manager what type of enemy to spawn (zombie, charger etc)
                GameplayManager.ENEMY_TYPE enemyType = sm.GetEnemyTypeToSpawn();

                switch(enemyType)
                {
                    case GameplayManager.ENEMY_TYPE.ZOMBIE:
                        temp.SpawnZombie((int)(sm.enemyCount * sm.enemyCountMultiplier), enemyStatModiers[0], enemyStatModiers[1], enemyDamage, elementType);
                        break;
                    default:
                        break;
                }

            }
            spawnBounceTime = sm.waveFrequency;
        }
        else
        {
            spawnBounceTime -= Time.deltaTime;
        }

    }

    // Calculates the distance of each spawnpoint to the player
    public void UpdateSpawnDistances(bool init)
    {
        for(int i = 0; i < spawnPoints.Count; ++i)
        {
            if (init)
            {
				spawnDistFromPlayer.Add(Vector3.Distance(GameManager.inst.gpManager.player.transform.position, spawnPoints[i].gameObject.transform.position));
			}
            else
            {
			    spawnDistFromPlayer[i] = Vector3.Distance(GameManager.inst.gpManager.player.transform.position, spawnPoints[i].gameObject.transform.position);
			}
           
        }
    }


    // Finds the furtherst spawnpoint from the player and then sets it so it won't be used again until recalculated
    private SpawnPoint ChooseSpawnPoint()
    {
        float maxDist = Mathf.Max(spawnDistFromPlayer.ToArray());
        int maxDistIndex = spawnDistFromPlayer.FindIndex(x => x.Equals(maxDist));
        spawnDistFromPlayer[maxDistIndex] = 0.0f;

        return spawnPoints[maxDistIndex];
    }
}
