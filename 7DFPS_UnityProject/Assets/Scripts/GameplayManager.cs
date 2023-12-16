using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public HUDInfo hudInfo;
    public PlayerInfo player;
    public List<GameObject> domainList;

	public int poolSize = 100; // default to 100
	public List<GameObject> enemyPrefabList;
    public List<GameObject> spawnPointList;

    public int totalKills = 0;
    public int totalDamage = 0;
    public int expGained = 0;

    // Scaling Settings
    [Header("Scaling Settings")]
    public float waveFrequency = 10.0f; // How long to wait before spawning the next wave
	public float statMultiplier = 1.0f; // how much to multiply the stats by
	public float statAdditive = 0.0f; // how much to add the stats by

    [Header("Power Ups")]
    public GameObject powerupContainer;
    public float powerupFrequency = 15.0f;
    private float powerupBounceTime = 0.0f;

	[HideInInspector] public EnemyObjectPool enemyObjectPool;
    [HideInInspector] public WaveManager waveManager;
    [HideInInspector] public ScalingManager scalingManager;
    public Camera mainCam = null;

    public enum ELEMENTS
    {
		ICE = 0,
		FIRE,    
        LIGHTNING
    };
    
	public enum ENEMY_TYPE
	{
		ZOMBIE = 0,
		CHARGER,
	};

    //returns reference to crosshair in game
    public GameObject crosshairGO
    {
        get { return hudInfo.crosshairGO; }
    }

    //returns position of crosshair in 3D world space
    public Vector3 crosshairToWorld
    {
        get
        {
            Vector3 pos = mainCam.ScreenToWorldPoint(new Vector3(
                    crosshairGO.transform.position.x,
                    crosshairGO.transform.position.y,
                    0));

            return pos;
        }
    }

    //returns ray/line of crosshair in 3d world space (mainly used for aiming, directing objects to real world space with crosshair)
    public Ray crosshairToRay
    {
        get
        {
            Ray ray = mainCam.ScreenPointToRay(new Vector3(
                    crosshairGO.transform.position.x,
                    crosshairGO.transform.position.y,
                    0));

            return ray;
        }
    }


    private void Awake()
    {
        if(GameManager.inst != null)
            GameManager.inst.gpManager = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitEnemySystem();
        //Init stats
        totalDamage = 0;
        expGained = 0;
        totalKills = 0;
        ClearPowerUps();
        powerupBounceTime = powerupFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        // Randomly spawn a power up after a set amount of time
        if (powerupBounceTime <= 0.0f && CheckForActivePowerUps() == false)
        {
            int temp = Random.Range(0, 3);
            SpawnPowerUp((ELEMENTS)temp);
            powerupBounceTime = powerupFrequency;
        }
        else
        {
            powerupBounceTime -= Time.deltaTime;
        }
	}

    public void DomainExpansion()
    {
		player.combat.SetDomainActiveStatus(true);
		StartCoroutine(SpawnDomainExpansion(GameManager.inst.gpManager.player.GetComponent<PlayerCombat>().equippedElement));   
	}

    IEnumerator SpawnDomainExpansion(ELEMENTS domain)
    {
        Vector3 pos = new Vector3(player.gameObject.transform.position.x, 0, player.gameObject.transform.position.z);
        GameObject temp = Instantiate(domainList[(int)domain], position: pos, Quaternion.identity);

        switch((int)domain)
        {
            case 0:
                AudioManager.inst.Play("icedomain");
                break;
            case 1:
				AudioManager.inst.Play("firedomain");
				break;
            case 2:
				AudioManager.inst.Play("lightningdomain");
				break;
        }

        yield return new WaitForSeconds(12.0f);
        player.combat.SetDomainActiveStatus(false);

        Destroy(temp);
    }

    // Initialise all the systems required for enemies to spawn
    private void InitEnemySystem()
    {
        // Create different gameplayer element managers
        enemyObjectPool = this.AddComponent<EnemyObjectPool>();
        waveManager = this.AddComponent<WaveManager>();
        scalingManager = this.AddComponent<ScalingManager>();

        // Init enemy object pool
        enemyObjectPool.poolSize = poolSize;
        for (int i = 0; i < enemyPrefabList.Count; ++i)
        {
            enemyObjectPool.enemyPrefabList.Add(enemyPrefabList[i]);
        }
        enemyObjectPool.Init();

        // Init wave system       
        waveManager.Init();
        for (int i = 0; i < spawnPointList.Count; ++i)
        {
            waveManager.spawnPoints.Add(spawnPointList[i].GetComponent<SpawnPoint>());
        }
        waveManager.UpdateSpawnDistances(true);

        // Init difficulty scaling system  
        scalingManager.waveFrequency = waveFrequency;
        scalingManager.statMultiplier = statMultiplier;
        scalingManager.statAdditive = statAdditive;
    }

    public void SpawnPowerUp(ELEMENTS element)
    {
        switch(element)
        {
            case ELEMENTS.ICE:
                powerupContainer.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case ELEMENTS.FIRE:
                powerupContainer.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case ELEMENTS.LIGHTNING:
                powerupContainer.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }

    public void ClearPowerUps()
    {
        for (int i = 0; i < 3; ++i)
        {
            powerupContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private bool CheckForActivePowerUps()
    {
        for (int i = 0; i < 3; ++i)
        {
            if (powerupContainer.transform.GetChild(i).gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator PlayLightningAOEAudio()
    {
		yield return new WaitForSeconds(0.23f);
		for (int i = 0; i < 19; ++i)
        {
			AudioManager.inst.PlayOneShot("lightningaoe");
            yield return new WaitForSeconds(0.23f);
		}
		AudioManager.inst.PlayOneShot("lightningaoe");
		yield break;   
    }
}
