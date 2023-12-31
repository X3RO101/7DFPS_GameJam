using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int maxHealth = 1;
    [HideInInspector] public int currentHealth;

    [SerializeField] private GameObject deathPS;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            // Coroutine so that the effects above can finishing playing before we disable this gameobject
            StartCoroutine(Kill());
        }

        // Clamp the health to be between 0 and the max health
        UnityEngine.Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void SetCurrentHealth(int setthis)
    {
        currentHealth = setthis;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetMaxHealth(int setthis)
    {
        maxHealth = setthis;
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);

        //Init death particle system on slime pos
        GameObject ps = Instantiate(deathPS);
        ps.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);

        Color color = Color.red;
        EnemyObject enemyInfo = GetComponent<EnemyObject>();

        //Update domain expansion values
        enemyInfo.StopElementParticles();
        GameManager.inst.gpManager.player.combat.SetEnemiesKilled(GameManager.inst.gpManager.player.combat.GetEnemiesKilled() + 1);
        GameManager.inst.gpManager.player.combat.UpdateDomainChargeValue();

        if (enemyInfo.element == GameplayManager.ELEMENTS.ICE)
            color = Color.cyan;
        else if (enemyInfo.element == GameplayManager.ELEMENTS.LIGHTNING)
            color = new Color(101/255, 0, 255/255);

        var colorModifier = ps.GetComponent<ParticleSystem>().main;
        colorModifier.startColor = color;

        //Give exp to player
        GameManager.inst.gpManager.player.currExp += (int)(250 * GameManager.inst.gpManager.player.lv * 0.8f);

        //Add to stats
        GameManager.inst.gpManager.expGained += GameManager.inst.gpManager.player.currExp;
        GameManager.inst.gpManager.totalKills += 1;

        //Check if player can level up, if he can, level up player and prompt level up text
        if (GameManager.inst.gpManager.player.isReadyToLevelUp)
        {
           
            GameManager.inst.gpManager.player.lv += 1;
            GameManager.inst.gpManager.player.currExp -= GameManager.inst.gpManager.player.maxExp;
            GameManager.inst.gpManager.player.maxExp = (int)(GameManager.inst.gpManager.player.maxExp * 1.2f);
            GameManager.inst.gpManager.hudInfo.UpdateLevel(GameManager.inst.gpManager.player.lv);

            //Increase stats of enemy to scale with player (additive)
            GameManager.inst.gpManager.scalingManager.statAdditive += 100;

			if (GameManager.inst.gpManager.player.lv % 3 == 0)
            {

				// Increase stats of enemy to scale with player (multiplier)
				GameManager.inst.gpManager.scalingManager.statMultiplier += 0.2f;

                // Increase damage of enemy
                GameManager.inst.gpManager.scalingManager.enemyDamage = (int)(GameManager.inst.gpManager.scalingManager.enemyDamage * 1.5f);

                GameManager.inst.gpManager.player.skillpoints += 1;
                //Prompt level up text if needed
                if (!GameManager.inst.gpManager.hudInfo.isLevelUpPromptEnabled)
                {
                    GameManager.inst.gpManager.hudInfo.PromptLevelUpText(true);
                }
            }
        }
        GameManager.inst.gpManager.hudInfo.UpdateEXP(GameManager.inst.gpManager.player.currExp, GameManager.inst.gpManager.player.maxExp);
    }
}
