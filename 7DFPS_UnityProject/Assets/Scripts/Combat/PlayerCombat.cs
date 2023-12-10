using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCombat : MonoBehaviour
{
    public GameplayManager.ELEMENTS equippedElement = GameplayManager.ELEMENTS.LIGHTNING;
    public bool aoeMode = false;

    [SerializeField] private Transform lightningProjectileSpawnLocation = null;
    [SerializeField] private Transform iceProjectileSpawnLocation = null;
    
    [Header("Single target abilities")]
    [SerializeField] private float singleTargetFireCooldown = 0.25f;
    [SerializeField] private float singleTargetLightningCooldown = 1f;
    [SerializeField] private float singleTargetIceCooldown = 0.5f;
    [SerializeField] private LayerMask singleTargetLayerMask;

    [Header("AOE abilities")]
    [SerializeField] private float aoeFireCooldown = 0.5f;
    [SerializeField] private float aoeLightningCooldown = 0.5f;
    [SerializeField] private float aoeIceCooldown = 0.5f;

    [SerializeField] private GameObject aoeIndicator = null;
    [SerializeField] private LayerMask aoeIndicatorLayerMask;
    private float abilityCastTimer = 0.0f;
    private float abilityCooldown = 1f;

    [Header("Element prefabs")]
    [SerializeField] private GameObject singleTargetFirePrefab = null;
    [SerializeField] private GameObject singleTargetLightningPrefab = null;
    [SerializeField] private GameObject singleTargetIcePrefab = null;

    [SerializeField] private GameObject aoeFirePrefab = null;
    [SerializeField] private GameObject aoeLightningPrefab = null;
    [SerializeField] private GameObject aoeIcePrefab = null;


    private void Start()
    {
        
    }

    private void Update()
    {
        //Toggle AOE mode
        if(Input.GetMouseButtonDown(1))
        {
            aoeMode = !aoeMode;
            aoeIndicator.SetActive(aoeMode);
        }

        if(aoeMode)
        {
            UpdateIndicator(); //Updates position of indicator
        }

        if(abilityCastTimer < abilityCooldown)
            abilityCastTimer += 1f * Time.deltaTime;
        else
        {
            if (aoeMode)
                AOEAttacks(equippedElement);
            else
                SingleTargetAttacks(equippedElement);
        }
    }
    private void UpdateIndicator()
    {
        RaycastHit hit;
        if (Physics.Raycast(GameManager.inst.gpManager.crosshairToRay, out hit, 40f, aoeIndicatorLayerMask))
        {
            aoeIndicator.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
        }
    }
    private void AOEAttacks(GameplayManager.ELEMENTS elementType)
    {
        switch (elementType)
        {
            case GameplayManager.ELEMENTS.ICE:
                if (Input.GetMouseButtonDown(0))
                {
                    abilityCastTimer = 0.0f;
                    abilityCooldown = aoeIceCooldown;
                    AOEIce();
                }

                break;
            case GameplayManager.ELEMENTS.FIRE:
                if (Input.GetMouseButtonDown(0))
                {
                    abilityCastTimer = 0.0f;
                    abilityCooldown = aoeFireCooldown;
                    AOEFire();
                }
                break;
            case GameplayManager.ELEMENTS.LIGHTNING:
                if (Input.GetMouseButtonDown(0))
                {
                    abilityCastTimer = 0.0f;
                    abilityCooldown = aoeLightningCooldown;
                    AOELightning();
                }
                break;
            default:
                break;
        }
    }

    private void AOEFire()
    {
        GameObject aoe = Instantiate(aoeFirePrefab);
        aoe.transform.position = aoeIndicator.transform.position;
    }
    private void AOELightning()
    {
        GameObject aoe = Instantiate(aoeLightningPrefab);
        aoe.transform.position = aoeIndicator.transform.position;
    }
    private void AOEIce()
    {
        GameObject aoe = Instantiate(aoeIcePrefab);
        aoe.transform.position = aoeIndicator.transform.position;
    }
    private void SingleTargetAttacks(GameplayManager.ELEMENTS elementType)
    {
        switch(elementType)
        {
            case GameplayManager.ELEMENTS.ICE:
                if (Input.GetMouseButtonDown(0))
                {
                    abilityCastTimer = 0.0f;
                    abilityCooldown = singleTargetIceCooldown;
                    SingleTargetIce();
                }

                break;
            case GameplayManager.ELEMENTS.FIRE:
                if (Input.GetMouseButtonDown(0))
                {
                    //float fireRange = 20f;
                    //abilityCastTimer = 0.0f;
                    //abilityCastTimer = singleTargetFireCooldown;
                    Ray ray = GameManager.inst.gpManager.crosshairToRay;
                    RaycastHit hit;

                    //Detect if enemy touched with raycast
                    if(Physics.Raycast(ray, out hit, 2000f, singleTargetLayerMask))
                    {
                        if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                            SingleTargetFire(hit.collider.gameObject);
                    }
                }
                break;
            case GameplayManager.ELEMENTS.LIGHTNING:
                if (Input.GetMouseButtonDown(0))
                {
                    abilityCastTimer = 0.0f;
                    abilityCooldown = singleTargetLightningCooldown;
                    SingleTargetLightning();
                }
                break;
            default:
                break;
        }
    }

    private void SingleTargetLightning()
    {
        float projectileSpeed = 20f;
        //Spawn it at specified position
        GameObject projectile = Instantiate(singleTargetLightningPrefab);
        projectile.transform.position = lightningProjectileSpawnLocation.position;

        Vector3 rotation = projectile.transform.rotation.eulerAngles;
        projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        projectile.GetComponent<Rigidbody>().AddForce(GameManager.inst.gpManager.crosshairToRay.direction * projectileSpeed, ForceMode.Impulse);
    }

    private void SingleTargetIce()
    {
        //Spawn it at specified position
        GameObject projectile = Instantiate(singleTargetIcePrefab, transform);
        projectile.transform.position = iceProjectileSpawnLocation.position;

        Vector3 rotation = projectile.transform.rotation.eulerAngles;
        projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        //projectile.GetComponent<Rigidbody>().AddForce(GameManager.inst.gpManager.crosshairToRay.direction * projectileSpeed, ForceMode.Impulse);
    }

    private void SingleTargetFire(GameObject enemy)
    {
        GameObject projectile = Instantiate(singleTargetFirePrefab);
        //Spawn sword at location between player and enemy, randomized location
        SingleTargetFire fireSword = projectile.GetComponent<SingleTargetFire>();
        fireSword.target = enemy;
        fireSword.source = gameObject;
        fireSword.PlayAnimation();
    }
}
