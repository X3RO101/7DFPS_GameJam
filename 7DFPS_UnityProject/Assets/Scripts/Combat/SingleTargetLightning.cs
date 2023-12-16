using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetLightning : MonoBehaviour
{
    public float lifetime = 10f;
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private GameObject impactPrefab = null;

    private void Start()
    {
        StartCoroutine("DestroyProjectile");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
            GameObject impact = Instantiate(impactPrefab);
            impact.transform.position = transform.position;

            // Enemy damage logic
            EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
            Color damageColor = Color.white;
            //Check if element can crit or is resistant to it (Set damage number to be gray if resistant, white for normal, yellow for crit)
            float damageMultiplier = 1.0f;
            if (!GameManager.inst.gpManager.player.combat.GetDomainActiveStatus())
            {
                if (temp.element == GameplayManager.ELEMENTS.LIGHTNING)
                {
                    damageMultiplier = GameManager.inst.gpManager.player.combat.weakMultiplier;
                    damageColor = Color.gray;
                }
                else if (temp.element == GameplayManager.ELEMENTS.ICE)
                {
                    damageMultiplier = GameManager.inst.gpManager.player.combat.critMultiplier;
                    damageColor = Color.yellow;
                }
            }
            else
            {
                damageMultiplier = GameManager.inst.gpManager.player.combat.critMultiplier;
                damageColor = Color.yellow;
            }

            //Calculate lightning damage
            int lightningDamage = (int)(GameManager.inst.gpManager.player.combat.singleTargetLightningDamage * damageMultiplier);
            int damage = lightningDamage + (int)Random.Range(-lightningDamage * 0.1f, lightningDamage * 0.1f);

            DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
            damageUI.InitDamageIndicator(damage, collision.gameObject.transform, damageColor);

            //Set enemy to flash white
			temp.FlashWhite();
			// Change damage value, placeholder value = 1
			temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);            
            //Set enemy to fly backwards
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 35f, ForceMode.Impulse);
            //Set object to stop moving for 0.1
            temp.StopAgentMovement(0.25f);
            temp.StopAnimation(0.25f);
            GameManager.inst.gpManager.totalDamage += damage;
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            GameObject impact = Instantiate(impactPrefab);
            impact.transform.position = transform.position;
        }
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
