using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AOELightning : MonoBehaviour
{
    public float lifetime = 10f;

    [SerializeField] private SphereCollider sphere = null;
    [SerializeField] private float startingIdleTime = 0.1f;
    [SerializeField] private float startIdleTimer = 0.0f;

    private float lightningStrikeTimer = 0.0f;
    private float lightningStrikeRate = 0.23f;

    private float stopTimer = 0.0f;
    private float stopTime = 4.75f;

    private void Start()
    {
        StartCoroutine("DestroyAOE");
    }

    private void Update()
    {
        startIdleTimer += 1 * Time.deltaTime;
        if (startIdleTimer > startingIdleTime)
        {
            sphere.enabled = true;

            lightningStrikeTimer += 1 * Time.deltaTime;
            stopTimer += 1 * Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (lightningStrikeTimer > lightningStrikeRate && stopTimer <= stopTime)
            {
                Debug.Log("Do hit now!");
                lightningStrikeTimer = 0.0f;

                //Calculate lightning damage
                int lightningDamage = GameManager.inst.gpManager.player.combat.aoeLightningDamage;
                int damage = lightningDamage + (int)Random.Range(-lightningDamage * 0.1f, lightningDamage * 0.1f);

                DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
                damageUI.InitDamageIndicator(damage, collision.gameObject.transform, Color.white);

                //Attack enemies in here
                //Debug.Log("Hit enemy");
                EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
                temp.FlashWhite();
                // Change damage value, placeholder value = 1
                temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);

                //Set object to stop moving for 0.1
                temp.StopAgentMovement(0.5f);
                temp.StopAnimation(0.5f);
            }
		}
    }

    private IEnumerator DestroyAOE()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
