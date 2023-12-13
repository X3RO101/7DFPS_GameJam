using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int maxHealth = 1;
    [HideInInspector] public int currentHealth;
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
            // Play death animation / particle system / sfx

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
    }
}
