using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetFire : MonoBehaviour
{
    public float lifetime = 1f;
    public GameObject target = null;
    public GameObject source = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DestroyProjectile");
    }

    public void PlayAnimation()
    {
        if(target != null)
        {
            //Set position and distance from sword to object
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + Random.Range(1F,1.5F), target.transform.position.z);
            float dist = Vector3.Distance(target.transform.position, source.transform.position);
            Vector3 dir = Vector3.forward;
            if(dist > 1f)
            {
                Vector3 sourcePosition = new Vector3(source.transform.position.x + Random.Range(-5f, 5f),
                    source.transform.position.y + Random.Range(0f, 5f),
                    source.transform.position.z + Random.Range(-5f, 5f));
                dir = target.transform.position - sourcePosition;
            }
            else
            {
                Vector3 sourcePosition = new Vector3(source.transform.position.x + Random.Range(-1f, 1f),
                    source.transform.position.y + Random.Range(0f, 1f),
                    source.transform.position.z + Random.Range(-1f, 1f));
                dir = target.transform.position - sourcePosition;
            }

            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = rotation;
        }
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
