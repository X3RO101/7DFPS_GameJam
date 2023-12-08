using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public PlayerInfo player;
    public List<GameObject> domainList;
    public enum ELEMENTS
    {
        FIRE = 0,
        ICE,
        LIGHTNING
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DomainExpansion(ELEMENTS domain)
    {
        GameObject temp = Instantiate(domainList[(int)domain]);
        yield return new WaitForSeconds(13.0f);
        Destroy(temp);
    }
}
