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

            //Calculate lightning damage
            int lightningDamage = GameManager.inst.gpManager.player.combat.singleTargetLightningDamage;
            int damage = lightningDamage + (int)Random.Range(-lightningDamage * 0.1f, lightningDamage * 0.1f);

            DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
            damageUI.InitDamageIndicator(damage, collision.gameObject.transform, Color.white);

            // Enemy damage logic
            EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
			temp.FlashWhite();
			// Change damage value, placeholder value = 1
			temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);            
            //Set enemy to fly backwards
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 35f, ForceMode.Impulse);
            //Set object to stop moving for 0.1
            temp.StopAgentMovement(0.25f);
            temp.StopAnimation(0.25f);
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
