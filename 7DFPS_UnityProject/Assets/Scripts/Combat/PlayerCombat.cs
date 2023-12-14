using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCombat : MonoBehaviour
{
    public GameplayManager.ELEMENTS equippedElement = GameplayManager.ELEMENTS.LIGHTNING;
    public bool aoeMode = false;
    public int iceAmmo = 35;
    public int fireAmmo = 50;
    public int lightningAmmo = 15;
    public int maxIceAmmo = 35;
    public int maxFireAmmo = 50;
    public int maxLightningAmmo = 15;

    [Header("Ability cost")]
    public int singleTargetIceCost = 5;
    public int singleTargetFireCost = 1;
    public int singleTargetLightningCost = 1;
    public int aoeIceCost = 25;
    public int aoeFireCost = 30;
    public int aoeLightningCost = 10;

    [Header("Ability damage")]
    public int singleTargetIceDamage = 50;          //Per arrow! there's 5 arrows so *5
    public int singleTargetFireDamage = 50;
    public int singleTargetLightningDamage = 250;
    public int aoeIceDamage = 300;          
    public int aoeFireDamage = 300;
    public int aoeLightningDamage = 100;            //Per lightning strike

    [Header("Damage multipliers")]
    public float weakMultiplier = 0.8f;
    public float critMultiplier = 1.2f;

    [Space(10)]
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
        SwapElement(); //Call to swap elements if input key


        //Toggles AOE mode
        if(Input.GetMouseButtonDown(1))
        {
            //Check if player can even use anything in AOE mode
            if(CanAOE(equippedElement))
            {
                aoeMode = !aoeMode;
                aoeIndicator.SetActive(aoeMode);
                GameManager.inst.gpManager.hudInfo.SetCrosshairEnable(!aoeMode);    //Disable crosshair
            }
        }

        //Renders indicator if in AOE mode
        if(aoeMode)
        {
            UpdateIndicator(); //Updates position of indicator
        }

        //Timer for player abilities
        if(abilityCastTimer < abilityCooldown)
            abilityCastTimer += 1f * Time.deltaTime;
        else
        {
            //Use AOE abilities over single target attacks if in AOE mode
            if (aoeMode)
                AOEAttacks(equippedElement);
            else
                SingleTargetAttacks(equippedElement);
        }
    }
    //returns true if specified element type has sufficient ammo to cast aoe
    public bool CanAOE(GameplayManager.ELEMENTS elementType)
    {
        if(elementType == GameplayManager.ELEMENTS.ICE)
        {
            if (iceAmmo >= aoeIceCost)
                return true;
            else 
                return false;
        }
        else if(elementType == GameplayManager.ELEMENTS.FIRE)
        {
            if (fireAmmo >= aoeFireCost)
                return true;
            else
                return false;
        }
        else if (elementType == GameplayManager.ELEMENTS.LIGHTNING)
        {
            if (lightningAmmo >= aoeLightningCost)
                return true;
            else
                return false;
        }

        return false;
    }
    private void SwapElement()
    {
        GameplayManager.ELEMENTS previousElement = equippedElement;
        if(Input.GetKey(KeyCode.Alpha1))
        {
            equippedElement = GameplayManager.ELEMENTS.ICE;
            GameManager.inst.gpManager.hudInfo.UpdateCrosshair(equippedElement);
            GameManager.inst.gpManager.hudInfo.UpdateElementContainer(equippedElement, previousElement);
        }
        else if(Input.GetKey(KeyCode.Alpha2))
        {
            equippedElement = GameplayManager.ELEMENTS.FIRE;
            GameManager.inst.gpManager.hudInfo.UpdateCrosshair(equippedElement);
            GameManager.inst.gpManager.hudInfo.UpdateElementContainer(equippedElement, previousElement); 
        }
        else if(Input.GetKey(KeyCode.Alpha3))
        {
            equippedElement = GameplayManager.ELEMENTS.LIGHTNING;
            GameManager.inst.gpManager.hudInfo.UpdateCrosshair(equippedElement);
            GameManager.inst.gpManager.hudInfo.UpdateElementContainer(equippedElement, previousElement);
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
        //Check if have sufficient resources to cast AOE, if no: return.
        if (!CanAOE(elementType))
            return;

        switch (elementType)
        {
            case GameplayManager.ELEMENTS.ICE:
                if (Input.GetMouseButtonDown(0))
                {
                    iceAmmo -= aoeIceCost;
                    abilityCastTimer = 0.0f;
                    abilityCooldown = aoeIceCooldown;
                    AOEIce();
                    //Disable indicator after using ability
                    aoeMode = false;
                    aoeIndicator.SetActive(false);
                    //Enable crosshair again
                    GameManager.inst.gpManager.hudInfo.SetCrosshairEnable(true);
                }

                break;
            case GameplayManager.ELEMENTS.FIRE:
                if (Input.GetMouseButtonDown(0))
                {
                    fireAmmo -= aoeFireCost;
                    abilityCastTimer = 0.0f;
                    abilityCooldown = aoeFireCooldown;
                    AOEFire();
                    //Disable indicator after using ability
                    aoeMode = false;
                    aoeIndicator.SetActive(false);
                    //Enable crosshair again
                    GameManager.inst.gpManager.hudInfo.SetCrosshairEnable(true);
                }
                break;
            case GameplayManager.ELEMENTS.LIGHTNING:
                if (Input.GetMouseButtonDown(0))
                {
                    lightningAmmo -= aoeLightningCost;
                    abilityCastTimer = 0.0f;
                    abilityCooldown = aoeLightningCooldown;
                    AOELightning();
                    //Disable indicator after using ability
                    aoeMode = false;
                    aoeIndicator.SetActive(false);
                    //Enable crosshair again
                    GameManager.inst.gpManager.hudInfo.SetCrosshairEnable(true);
                }
                break;
            default:
                break;
        }
        //Updates the ammo to UI after use of abilities
        GameManager.inst.gpManager.hudInfo.UpdateAmmoCounter(iceAmmo, fireAmmo, lightningAmmo);
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
                if (iceAmmo == 0)
                    break;
                
                if (Input.GetMouseButtonDown(0))
                {
                    iceAmmo -= singleTargetIceCost;
                    if (iceAmmo < 0)
                        iceAmmo = 0;

                    abilityCastTimer = 0.0f;
                    abilityCooldown = singleTargetIceCooldown;
                    SingleTargetIce();
                }

                break;
            case GameplayManager.ELEMENTS.FIRE:
                if (fireAmmo == 0)
                    break;

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
                        {
                            fireAmmo -= singleTargetFireCost;
                            if (fireAmmo < 0)
                                fireAmmo = 0;
                            SingleTargetFire(hit.collider.gameObject);
						}
                            
                    }
                }
                break;
            case GameplayManager.ELEMENTS.LIGHTNING:
                if (lightningAmmo == 0)
                    break;

                if (Input.GetMouseButtonDown(0))
                {
                    lightningAmmo -= singleTargetLightningCost;
                    if (lightningAmmo < 0)
                        lightningAmmo = 0;

                    abilityCastTimer = 0.0f;
                    abilityCooldown = singleTargetLightningCooldown;
                    SingleTargetLightning();
                }
                break;
            default:
                break;
        }

        GameManager.inst.gpManager.hudInfo.UpdateAmmoCounter(iceAmmo, fireAmmo, lightningAmmo);
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

        EnemyObject temp = enemy.gameObject.GetComponent<EnemyObject>();
        Color damageColor = Color.white;
        //Check if element can crit or is resistant to it (Set damage number to be gray if resistant, white for normal, yellow for crit)
        float damageMultiplier = 1.0f;
        if (temp.element == GameplayManager.ELEMENTS.FIRE)
        {
            damageMultiplier = weakMultiplier;
            damageColor = Color.gray;
        }
        else if (temp.element == GameplayManager.ELEMENTS.LIGHTNING)
        {
            damageMultiplier = critMultiplier;
            damageColor = Color.yellow;
        }

        //Calculate fire damage
        int damage = (int)(singleTargetFireDamage * damageMultiplier) + (int)Random.Range(-singleTargetFireDamage * 0.1f, singleTargetFireDamage * 0.1f);

        DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
        damageUI.InitDamageIndicator(damage, enemy.transform, damageColor);
        temp.FlashWhite();
        // Change damage value, placeholder value = 1
        temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);            
        
        //Set enemy to fly backwards
        enemy.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5f, ForceMode.Impulse);
        //Set object to stop moving for 0.1
        temp.StopAgentMovement(0.15f);
        temp.StopAnimation(0.15f);
    }
}
