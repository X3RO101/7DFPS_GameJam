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
        target = GameManager.inst.getPlayerPos();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
		target = GameManager.inst.getPlayerPos();
		agent.destination = target;
    }
}
