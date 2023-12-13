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
            //Calculate fire damage
            int fireDamage = GameManager.inst.gpManager.player.combat.aoeFireDamage;
            int damage = fireDamage + (int)Random.Range(-fireDamage * 0.1f, fireDamage * 0.1f);

            DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
            damageUI.InitDamageIndicator(damage, collision.gameObject.transform, Color.white);

            Debug.Log("Hit enemy");
            EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
            temp.FlashWhite();
            // Change damage value, placeholder value = 1
            temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);

            //Set object to stop moving for 0.1
            temp.StopAgentMovement(0.2f);
            temp.StopAnimation(0.2f);
        }
    }

    private IEnumerator DestroyAOE()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
