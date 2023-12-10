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
