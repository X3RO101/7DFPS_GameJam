using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public GameObject player;
    public List<GameObject> domainList;

    private Vector3 playerPos;

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
        playerPos = player.transform.position;
        StartCoroutine(DomainExpansion(ELEMENTS.FIRE));
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
    }

    public Vector3 getPlayerPos()
    {
        return playerPos;
    }

    IEnumerator DomainExpansion(ELEMENTS domain)
    {
        GameObject temp = Instantiate(domainList[(int)domain]);
		yield return new WaitForSeconds(13.0f);
        Destroy(temp);
    }
}
