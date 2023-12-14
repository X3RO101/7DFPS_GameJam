using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private GameObject impactPrefab = null;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            GameObject impact = Instantiate(impactPrefab);
            impact.transform.position = transform.position;

            // Damage enemy logic
            EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
            Color damageColor = Color.white;
            //Check if element can crit or is resistant to it (Set damage number to be gray if resistant, white for normal, yellow for crit)
            float damageMultiplier = 1.0f;
            if (temp.element == GameplayManager.ELEMENTS.ICE)
            {
                damageMultiplier = GameManager.inst.gpManager.player.combat.weakMultiplier;
                damageColor = Color.gray;
            }
            else if (temp.element == GameplayManager.ELEMENTS.FIRE)
            {
                damageMultiplier = GameManager.inst.gpManager.player.combat.critMultiplier;
                damageColor = Color.yellow;
            }

            //Calculate ice damage
            int iceDamage = (int)(GameManager.inst.gpManager.player.combat.singleTargetIceDamage * damageMultiplier);
            int damage = iceDamage + (int)Random.Range(-iceDamage * 0.1f, iceDamage * 0.1f);

            //Spawn damage number
            DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
            damageUI.InitDamageIndicator(damage, collision.gameObject.transform, damageColor);
           
			temp.FlashWhite();
			// Change damage value, placeholder value = 1
			temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);

            //Set enemy to fly backwards
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5f, ForceMode.Impulse);
            //Set object to stop moving for 0.1
            temp.StopAgentMovement(0.2f);
            temp.StopAnimation(0.2f);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            GameObject impact = Instantiate(impactPrefab);
            impact.transform.position = transform.position;
        }
    }
}
