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
    public HealthComponent hp; // health component to get/set hp related things
    public int damage; // damage to deal to the player when in contact

    [Header ("Materials")]
    public Material normalMat;
    public Material damagedMat;
    public Material normalFace;
    public Material damagedFace;

    [Header("Element Materials")]
    public Material[] elementMaterialList;

    private enum EnemyAnimationState
    {
        WALK = 0,
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

		if (bouncetime <= 0.0f)
		{
			FlashWhite();
			//hp.SetCurrentHealth(hp.GetCurrentHealth() - 1);
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
        Material[] damaged = { damagedMat, damagedFace };
        mr.materials = damaged;
        yield return new WaitForSeconds(0.15f);
		Material[] normal = { normalMat, normalFace };
		mr.materials = normal;
		yield break;
    }
}