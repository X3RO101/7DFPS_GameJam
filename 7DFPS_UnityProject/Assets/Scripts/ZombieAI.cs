using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private Vector3 target;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.inst.gpManager.player.transform.position;
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = target;
    }

    // Update is called once per frame
    void Update()
    {
        //target = GameManager.inst.player.transform.position;
        agent.SetDestination(GameManager.inst.gpManager.player.transform.position);
    }
}
