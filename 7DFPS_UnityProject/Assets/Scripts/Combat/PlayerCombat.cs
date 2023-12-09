using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameplayManager.ELEMENTS equippedElement = GameplayManager.ELEMENTS.LIGHTNING;

    [SerializeField] private Transform projectileSpawnLocation = null;
    [SerializeField] private float singleTargetFireCooldown = 0.25f;
    [SerializeField] private float singleTargetLightningCooldown = 1f;
    [SerializeField] private float singleTargetIceCooldown = 0.1f;

    private float abilityCastTimer = 0.0f;
    private float abilityCooldown = 1f;

    [Header("Element attacks")]
    [SerializeField] private GameObject singleTargetFirePrefab = null;
    [SerializeField] private GameObject singleTargetLightningPrefab = null;
    [SerializeField] private GameObject singleTargetIcePrefab = null;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(abilityCastTimer < abilityCooldown)
            abilityCastTimer += 1f * Time.deltaTime;
        else
        {
            SingleTargetAttacks(equippedElement);
        }
    }

    private void SingleTargetAttacks(GameplayManager.ELEMENTS elementType)
    {
        switch(elementType)
        {
            case GameplayManager.ELEMENTS.ICE:
                if (Input.GetMouseButton(0))
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
                    Ray ray = GameManager.inst.gpManager.crosshairToRay;
                    RaycastHit hit;

                    //Detect if enemy touched with raycast
                    if(Physics.Raycast(ray, out hit))
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
        projectile.transform.position = projectileSpawnLocation.position;

        Vector3 rotation = projectile.transform.rotation.eulerAngles;
        projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        projectile.GetComponent<Rigidbody>().AddForce(GameManager.inst.gpManager.crosshairToRay.direction * projectileSpeed, ForceMode.Impulse);
    }

    private void SingleTargetIce()
    {
        float projectileSpeed = 50f;
        //Spawn it at specified position
        GameObject projectile = Instantiate(singleTargetIcePrefab);
        projectile.transform.position = projectileSpawnLocation.position;

        Vector3 rotation = projectile.transform.rotation.eulerAngles;
        projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        projectile.GetComponent<Rigidbody>().AddForce(GameManager.inst.gpManager.crosshairToRay.direction * projectileSpeed, ForceMode.Impulse);
    }

    private void SingleTargetFire(GameObject enemy)
    {
        GameObject projectile = Instantiate(singleTargetFirePrefab);
        //Spawn sword at location between player and enemy, randomized location
        Debug.Log("Use fire");
        SingleTargetFire fireSword = projectile.GetComponent<SingleTargetFire>();
        fireSword.target = enemy;
        fireSword.source = gameObject;
        fireSword.PlayAnimation();
    }
}
