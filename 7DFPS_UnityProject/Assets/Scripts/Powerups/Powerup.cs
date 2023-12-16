using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameplayManager.ELEMENTS elementType;
    //public GameObject target = null;

    //[SerializeField]
    //private Rigidbody rb;

    private void LateUpdate()
    {
        //if (target == null)
        //    return;

        //Vector3 velocity = (target.transform.position - transform.position) * 5F;
        //rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(elementType)
        {
            case GameplayManager.ELEMENTS.ICE:
                //Do not do anything if player is at max ammo
                if (GameManager.inst.gpManager.player.combat.iceAmmo == GameManager.inst.gpManager.player.combat.maxIceAmmo)
                    return;

                GameManager.inst.gpManager.player.combat.iceAmmo += 12;
                GameManager.inst.gpManager.player.combat.iceAmmo = Mathf.Clamp(GameManager.inst.gpManager.player.combat.iceAmmo, 0, GameManager.inst.gpManager.player.combat.maxIceAmmo);
                break;
            case GameplayManager.ELEMENTS.FIRE:
                //Do not do anything if player is at max ammo
                if (GameManager.inst.gpManager.player.combat.fireAmmo == GameManager.inst.gpManager.player.combat.maxFireAmmo)
                    return;

                GameManager.inst.gpManager.player.combat.fireAmmo += 25;
                GameManager.inst.gpManager.player.combat.fireAmmo = Mathf.Clamp(GameManager.inst.gpManager.player.combat.fireAmmo, 0, GameManager.inst.gpManager.player.combat.maxFireAmmo);
                break;
            case GameplayManager.ELEMENTS.LIGHTNING:
                //Do not do anything if player is at max ammo
                if (GameManager.inst.gpManager.player.combat.lightningAmmo == GameManager.inst.gpManager.player.combat.maxLightningAmmo)
                    return;

                GameManager.inst.gpManager.player.combat.lightningAmmo += 5;
                GameManager.inst.gpManager.player.combat.lightningAmmo = Mathf.Clamp(GameManager.inst.gpManager.player.combat.lightningAmmo, 0, GameManager.inst.gpManager.player.combat.maxLightningAmmo);
                break;
            default:
                break;
        }

        GameManager.inst.gpManager.ClearPowerUps();
        //Destroy(gameObject);
    }
}
