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

			// Damage enemy logic
			EnemyObject temp = collision.gameObject.GetComponent<EnemyObject>();
			temp.FlashWhite();
			// Change damage value, placeholder value = 1
			temp.hp.SetCurrentHealth(temp.hp.GetCurrentHealth() - 1);
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
