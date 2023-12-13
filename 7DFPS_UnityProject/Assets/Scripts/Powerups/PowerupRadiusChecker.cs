using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupRadiusChecker : MonoBehaviour
{
    public SphereCollider sphereCollider = null;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Powerup>().target = GameManager.inst.gpManager.player.gameObject;
    }
}
