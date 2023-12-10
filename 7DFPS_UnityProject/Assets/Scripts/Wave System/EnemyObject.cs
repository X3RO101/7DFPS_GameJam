using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyObject : MonoBehaviour
{
    public GameplayManager.ENEMY_TYPE behaviour;
    public GameplayManager.ELEMENTS element;

    private NavMeshAgent agent; // navmeshagent component
    private MeshRenderer mr; // mesh renderer component, lets us hotswap materials at runtime (can be used to change material depending on element)

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mr = GetComponent<MeshRenderer>();
	}

    // Update is called once per frame
    void Update()
    {
		// Update the enemy according to its type
		switch (behaviour)
        {
            case GameplayManager.ENEMY_TYPE.ZOMBIE:
				agent.SetDestination(GameManager.inst.gpManager.player.transform.position);
                break;
            case GameplayManager.ENEMY_TYPE.CHARGER:
                break;
            default:
                // default behaviour is zombie
				agent.SetDestination(GameManager.inst.gpManager.player.transform.position);
				break;
		}
    }
}
