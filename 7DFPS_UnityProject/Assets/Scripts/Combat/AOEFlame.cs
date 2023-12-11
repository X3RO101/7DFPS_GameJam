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
            Debug.Log("Hit enemy");
        }
    }

    private IEnumerator DestroyAOE()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
