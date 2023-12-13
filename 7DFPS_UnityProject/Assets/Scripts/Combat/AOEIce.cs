using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AOEIce : MonoBehaviour
{
    public float lifetime = 5f;

    [SerializeField]
    private SphereCollider sphere = null;

    private void Start()
    {
        StartCoroutine("DestroyAOE");
        DOTween.To(() => sphere.radius, x => sphere.radius = x, 6, 0.5f);
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