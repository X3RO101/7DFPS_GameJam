using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyObject : MonoBehaviour
{
    public GameplayManager.ENEMY_TYPE behaviour;
    public GameplayManager.ELEMENTS element;

    private NavMeshAgent agent; // navmeshagent component
    public SkinnedMeshRenderer mr; // mesh renderer component, lets us hotswap materials at runtime (can be used to change material depending on element)
    private Animator animator; // animation controller
    [HideInInspector] public HealthComponent hp; // health component to get/set hp related things
    public int damage; // damage to deal to the player when in contact

    public Material normalMat;
    public Material damagedMat;

    private enum EnemyAnimationState
    {
        WALK = 0,
        DAMAGED,
    };

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hp = GetComponent<HealthComponent>();
	}

    float bouncetime = 3.0f;

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

        // Test code for flashing white on damage
        if(bouncetime <= 0.0f)
        {
            FlashWhite();
            bouncetime = 3.0f;
        }
        else
        {
            bouncetime -= Time.deltaTime;
        }
	}

    // Changes the animation state of the enemy to the one specified
    private void UpdateAnimationState(EnemyAnimationState setthis)
    {
        switch(setthis)
        {
            case EnemyAnimationState.WALK:
                animator.SetTrigger("Walk");
                break;
            case EnemyAnimationState.DAMAGED:
                animator.SetTrigger("Damaged"); 
                break;
            default:
                break;
        }
    }

    public int GetDamage()
    {
        return damage;
    }
    public void SetDamage(int setthis)
    {
        damage = setthis;
    }

    public void FlashWhite()
    {
        StartCoroutine(FlashOnHit());
    }

    IEnumerator FlashOnHit()
    {
        mr.material = damagedMat;
        yield return new WaitForSeconds(0.1f);
        mr.material = normalMat;
        yield break;
    }
}
