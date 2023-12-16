using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyObject : MonoBehaviour
{
    public GameplayManager.ENEMY_TYPE behaviour;
    public GameplayManager.ELEMENTS element;

    private NavMeshAgent agent; // navmeshagent component
    public SkinnedMeshRenderer mr; // mesh renderer component, lets us hotswap materials at runtime (can be used to change material depending on element)
    public Animator animator; // animation controller
    public HealthComponent hp; // health component to get/set hp related things

    
    [HideInInspector] public Material normalMat;
    [HideInInspector] public Material normalFace;
    [HideInInspector] public Material damagedFace;
    public GameObject psList; // list of particle system for the enemy, will change depending on element

	[Header("Materials")]
	public Material damagedMat;
	public Material[] elementMaterialList;
    public Material[] normalFaceMaterialList;
    public Material[] damagedFaceMaterialList;

    [Header("Particle Effects")]
    public GameObject[] elementParticleList;

    [Header("Attacking Player Info")]
    public int damage = 15;
    public float attackRate = 0.25f;        //enemy attacks every 0.25s if player is colliding with player
    [SerializeField] private float attackTimer = 0.0f;

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

    //float bouncetime = 3.0f;

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

        //if (bouncetime <= 0.0f)
        //{
        //	FlashWhite();
        //	//hp.SetCurrentHealth(hp.GetCurrentHealth() - 1);
        //	bouncetime = 3.0f;
        //}
        //else
        //{
        //	bouncetime -= Time.deltaTime;
        //}

        if (attackTimer < attackRate)
            attackTimer += 1f * Time.deltaTime;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            attackTimer = attackRate;   //make player get hit by enemy the moment player collides with enemy
        }
    }
    //Collision detection with enemy
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(attackTimer >= attackRate)
            {
                attackTimer = 0.0f;                             //reset enemy
                GameManager.inst.gpManager.hudInfo.PromptHurtRadialUI();    //spawn radial red ui to give feedback
                GameManager.inst.gpManager.player.hp -= damage; //lower player hp
                GameManager.inst.gpManager.hudInfo.UpdateHP(GameManager.inst.gpManager.player.hp, GameManager.inst.gpManager.player.maxHP);
                AudioManager.inst.Play("playerdamaged");
            }
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

    public void StopElementParticles()
    {
        psList.transform.GetChild((int)element).gameObject.SetActive(false);
    }
    public void StartElementParticles()
    {
        psList.transform.GetChild((int)element).gameObject.SetActive(true);
    }

    public void FlashWhite()
    {
        StartCoroutine(FlashOnHit());
    }

    private IEnumerator FlashOnHit()
    {
        Material[] damaged = { damagedMat, damagedFace };
        mr.materials = damaged;
        yield return new WaitForSeconds(0.1f);
		Material[] normal = { normalMat, normalFace };
		mr.materials = normal;
		yield break;
    }

    public void StopAgentMovement(float time)
    {
        StartCoroutine(StopAgentMovementCoroutine(time));
    }

    private IEnumerator StopAgentMovementCoroutine(float time)
    {
        //Stop agent from moving
        agent.isStopped = true;
        yield return new WaitForSeconds(time);
        agent.isStopped = false;
        yield break;
    }

    public void StopAnimation(float time)
    {
        StartCoroutine(StopAnimationCoroutine(time));
    }

    private IEnumerator StopAnimationCoroutine(float time)
    {
        //disable animation being played
        animator.enabled = false;
        yield return new WaitForSeconds(time);
        animator.enabled = true;
        yield break;
    }
}
