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

            //Calculate ice damage
            int iceDamage = GameManager.inst.gpManager.player.combat.singleTargetIceDamage;
            int damage = iceDamage + (int)Random.Range(-iceDamage * 0.1f, iceDamage * 0.1f);

            //Spawn damage number
            DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
            damageUI.InitDamageIndicator(damage, collision.gameObject.transform, Color.white);
           
            // Damage enemy logic
            EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
			temp.FlashWhite();
			// Change damage value, placeholder value = 1
			temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);
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
