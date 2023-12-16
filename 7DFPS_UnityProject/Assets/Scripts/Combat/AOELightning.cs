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
        lightningStrikeTimer = stopTimer = 0.0f;
        StartCoroutine("DestroyAOE");
        StartCoroutine(GameManager.inst.gpManager.PlayLightningAOEAudio());
    }

    private void Update()
    {
        startIdleTimer += 1 * Time.deltaTime;
        if (startIdleTimer > startingIdleTime && stopTimer < stopTime)
        {
            sphere.enabled = true;
            lightningStrikeTimer += 1 * Time.deltaTime;
            stopTimer += 1 * Time.deltaTime;
		}
        else if(startIdleTimer > startingIdleTime && stopTimer > stopTime)
        {
            sphere.enabled = false;
            Debug.Log("Disabled sphere");
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (lightningStrikeTimer > lightningStrikeRate)
        {
            Debug.Log("Check collision");
            lightningStrikeTimer = 0.0f;

			if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {

				//Debug.Log("Do hit now!");
				//Attack enemies in here
				//Debug.Log("Hit enemy");
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
                int lightningDamage = (int)(GameManager.inst.gpManager.player.combat.aoeLightningDamage * damageMultiplier);
                int damage = lightningDamage + (int)Random.Range(-lightningDamage * 0.1f, lightningDamage * 0.1f);

                DamageNumber damageUI = GameManager.inst.gpManager.hudInfo.SpawnDamageIndicator();
                damageUI.InitDamageIndicator(damage, collision.gameObject.transform, damageColor);

                temp.FlashWhite();
                // Change damage value, placeholder value = 1
                temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - damage);

                //Set object to stop moving for 0.1
                temp.StopAgentMovement(0.5f);
                temp.StopAnimation(0.5f);

                GameManager.inst.gpManager.totalDamage += damage;
            }
        }
    }

    private IEnumerator DestroyAOE()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
