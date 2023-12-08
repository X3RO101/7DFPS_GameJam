using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public GameplayManager gpManager;

	private void Awake()
	{
		if (inst != null && inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            inst = this;
        }

        DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start()
    {
        //StartCoroutine(DomainExpansion(ELEMENTS.FIRE));
    }

    // Update is called once per frame
    void Update()
    {

    }

}
