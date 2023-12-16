using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEFlame : MonoBehaviour
{
    public float lifetime = 5f;
    private void Start()
    {
        StartCoroutine("DestroyAOE");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Attack enemies in here
            EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
            Color damageColor = Color.white;
            //Check if element can crit or is resistant to it (Set damage number to be gray if resistant, white for normal, yellow for crit)
            float damageMultiplier = 1.0f;

            if(!GameManager.inst.gpManager.player.combat.GetDomainActiveStatus())
            {
                if (temp.element == GameplayManager.ELEMENTS.FIRE)
                {
                    damageMultiplier = GameManager.inst.gpManager.player.combat.weakMultiplier;
                    damageColor = Color.gray;
                }
                else if (temp.element == GameplayManager.ELEMENTS.LIGHTNING)
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


            //Calculate fire damage
            int fireDamage = (int)(GameManager.inst.gpManager.player.combat.aoeFireDamage * damageMultiplier);
            int damage = fireDamage + (int)Random.Range(-fireDamage * 0.1f, fireDamage * 0.1f);

            DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
            damageUI.InitDamageIndicator(damage, collision.gameObject.transform, damageColor);

            //Debug.Log("Hit enemy");
            temp.FlashWhite();
            // Change damage value, placeholder value = 1
            temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);

            //Set object to stop moving for 0.1
            temp.StopAgentMovement(0.2f);
            temp.StopAnimation(0.2f);

            //Add to stats
            GameManager.inst.gpManager.totalDamage += damage;
        }
    }

    private IEnumerator DestroyAOE()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
