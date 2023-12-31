using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingManager : MonoBehaviour
{
    public float scalingFrequency = 30.0f;

    public float waveFrequency = 5.0f; // how long to wait before spawning the next wave
    public int groupSpawnCount = 1; // how many spawners should we activate

    // Enemy stats will be -> (Base Stats * Multiplier) + Additive
    public float statMultiplier = 1.0f; // how much to multiply the stats by
    public float statAdditive = 0.0f; // how much to add the stats by
    public int enemyDamage = 15; // default damage of the enemy, to be increased everytime the player gains a skill point

    // Enemy count modifiers
    public int enemyCount = 5;
    public float enemyCountMultiplier = 1.0f;

    public float timeElapsed = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        // We modify the different values above based on the time elapsed (temp values, we increase different values every 15s)
        if (timeElapsed % 15.0f < 0.5f)
        {
            IncreaseDifficultyScale();
        }

		timeElapsed += Time.deltaTime;
	}

    // Decides what enemy to spawn depending on the time lapsed, or player current weapons
    public GameplayManager.ENEMY_TYPE GetEnemyTypeToSpawn()
    {
        return GameplayManager.ENEMY_TYPE.ZOMBIE;

  //      // Placeholder time
  //      if (timeElapsed < 300.0f)
  //      {
		//	return GameplayManager.ENEMY_TYPE.ZOMBIE;
		//}
  //      else
  //      {
  //          // we only spawn other enemy types after a certain amount of time
  //          float rand = Random.Range(0.0f, 1.0f);
  //          if (rand < 0.7f)
  //          {
  //              return GameplayManager.ENEMY_TYPE.ZOMBIE;
  //          }
  //          else
  //          {
  //              return GameplayManager.ENEMY_TYPE.ZOMBIE;   // Change this line to another enemy type i.e. charger
  //          }

  //      }
	}

    public GameplayManager.ELEMENTS GetElementTypeToSpawn()
    {
        float rand = Random.Range(0, 3);

        // Choose a random element to spawn
        if (rand < 1.0f)
        {
            return GameplayManager.ELEMENTS.ICE;
        }
        else if (rand < 2.0f)
        {
            return GameplayManager.ELEMENTS.FIRE;
        }
        else if (rand < 3.0f)
        {
            return GameplayManager.ELEMENTS.LIGHTNING;
        }

        return GameplayManager.ELEMENTS.FIRE;
    }

    private void IncreaseDifficultyScale()
    {
        if (enemyCount < 7 && timeElapsed >= 45.0f)
        {
            enemyCount = 8;
        }
        
    }
}
