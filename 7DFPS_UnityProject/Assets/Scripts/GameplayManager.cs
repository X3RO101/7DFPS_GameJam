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

    // Scaling Settings
    public float waveFrequency = 10.0f; // How long to wait before spawning the next wave
	public float statMultiplier = 1.0f; // how much to multiply the stats by
	public float statAdditive = 0.0f; // how much to add the stats by


	[HideInInspector] public EnemyObjectPool enemyObjectPool;
    [HideInInspector] public WaveManager waveManager;
    [HideInInspector] public ScalingManager scalingManager;
    public Camera mainCam = null;

    public enum ELEMENTS
    {
        FIRE = 0,
        ICE,
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

    // Start is called before the first frame update
    void Start()
    {
        //InitEnemySystem();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.P))
        { 
		    waveManager.spawnPoints[0].SpawnZombie(5, 1.0f, 0.0f);
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
            StartCoroutine(DomainExpansion(ELEMENTS.FIRE));
		}
	}

    IEnumerator DomainExpansion(ELEMENTS domain)
    {
        GameObject temp = Instantiate(domainList[(int)domain], player.gameObject.transform);
        yield return new WaitForSeconds(12.0f);
        Destroy(temp);
    }

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
}
