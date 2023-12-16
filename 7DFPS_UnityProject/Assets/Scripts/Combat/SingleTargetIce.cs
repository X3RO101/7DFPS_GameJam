using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTargetIce : MonoBehaviour
{
    public float lifetime = 20f;
    public float projectileSpeed = 40f;
    [SerializeField] private List<Rigidbody> arrowsRB = new List<Rigidbody>();

    [SerializeField] private float arrowSpawningRate = 0.15f;
    private float spawnRateTimer = 0.0f;
    private int arrowIndex = 0;

    private void Start()
    {
        StartCoroutine("DestroyProjectile");
		AudioManager.inst.Play("icesinglespawn");
	}

    private void Update()
    {
        if (arrowIndex + 1 > arrowsRB.Count)
            return;

        spawnRateTimer += 1 * Time.deltaTime;
        if (spawnRateTimer > arrowSpawningRate)
        {
            spawnRateTimer = 0f;
            GameObject arrow = arrowsRB[arrowIndex].gameObject;
            arrow.SetActive(true);

            

            Vector3 rotation = arrow.transform.rotation.eulerAngles;
            arrow.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            //arrowsRB[arrowIndex].AddForce(GameManager.inst.gpManager.crosshairToRay.direction * projectileSpeed, ForceMode.Impulse);
            //transform.parent = null;
            arrowIndex += 1;

            if(arrowIndex + 1 > arrowsRB.Count)
            {
                transform.parent = null;
                
                for(int i = 0; i < arrowsRB.Count; i++)
                {
                    arrowsRB[i].AddForce(GameManager.inst.gpManager.crosshairToRay.direction * projectileSpeed, ForceMode.Impulse);
                    arrowsRB[i].GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
