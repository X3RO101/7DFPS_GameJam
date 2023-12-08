using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public PlayerInfo player;
    public List<GameObject> domainList;

    public enum ELEMENTS
    {
        FIRE = 0,
        ICE,
        LIGHTNING
    };

	private void Awake()
	{
		if (inst != null && inst != this)
        {
            Destroy(this);
        }
        else
        {
            inst = this;
        }
	}

	// Start is called before the first frame update
	void Start()
    {
        StartCoroutine(DomainExpansion(ELEMENTS.FIRE));
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
