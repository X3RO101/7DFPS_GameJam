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
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //Attack enemies in here
            Debug.Log("Hit enemy");
			EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
			temp.FlashWhite();
			// Change damage value, placeholder value = 1
			temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - 1);
		}
    }

    private IEnumerator DestroyAOE()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
