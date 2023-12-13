using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager inst;

    [HideInInspector] public List<int> playerStatLevels; // Ice, Fire, Lightning, HP
    private int unusedLevels;

    public List<GameObject> emptyPipContainers; // Parent obj of all the empty pip icons

    [Header("Unused Levels Text")]
    public TextMeshProUGUI unusedLevelsTMP;

    [Header("Level Pip Sprite")]
    public List<Sprite> levelPipSpriteList; // Ice Pip, Fire Pip, Lightning Pip, HP Pip, Empty Pip

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
		// Add 3 initial values, Ice, Fire, Lightning, HP
		playerStatLevels.Add(0);
		playerStatLevels.Add(0);
		playerStatLevels.Add(0);
		playerStatLevels.Add(0);

        // Initialise number of levels the player has
        unusedLevels = 100;

	}

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get/Set for unusedLevels to be used for when player clicks the +/- button in the upgrade panel, and to render the number of levels they have remaining
    public int GetUnusedLevels()
    {
        return unusedLevels;
    }
    public void SetUnusedLevels(int setthis)
    {
        unusedLevels = setthis;
    }

    public void ClampLevels()
    {
        for(int i = 0; i < playerStatLevels.Count; ++i)
        {
            playerStatLevels[i] = Mathf.Clamp(playerStatLevels[i], 1, 10);
        }
    }

    // Call this function after player has clicked the confirm button
    // Function applies all the relevant stat changes for whatever the player did in the upgrade panel
    public void UpdateStats()
    {
        for(int i = 0; i < playerStatLevels.Count; ++i)
        {
            // Modify the attacks/stats here in the respective switch case below
            // To get the current level for that particular stat -> playerStatLevels[i]
            switch(i)
            {
                case 0: // ICE
                    break;
                case 1: // FIRE
                    break;
                case 2: // LIGHTNING
                    break;
                case 3: // HP
                    break;
                default:
                    break;
            }
        }
    }

    public void IncreaseUnusedLevels()
    {
        unusedLevels += 1;
        unusedLevelsTMP.text = "Unspent Skill-Points: " + unusedLevels;
    }
    public void DecreaseUnusedLevels()
    {
        unusedLevels -= 1;
		unusedLevelsTMP.text = "Unspent Skill-Points: " + unusedLevels;
	}

    public void IncreaseStatLevel(int increaseThis)
    {
        if (unusedLevels == 0 || playerStatLevels[increaseThis] == 10)
        {
            return;
        }

		Mathf.Clamp(playerStatLevels[increaseThis], 0, 9);
		emptyPipContainers[increaseThis].transform.GetChild(playerStatLevels[increaseThis]).GetComponent<Image>().sprite = levelPipSpriteList[increaseThis];
		playerStatLevels[increaseThis] += 1;

        DecreaseUnusedLevels();
    }
    public void DecreaseStatLevel(int decreaseThis)
    {
        if (playerStatLevels[decreaseThis] == 0)
        {
            return;
        }

		Mathf.Clamp(playerStatLevels[decreaseThis], 0, 9);
		emptyPipContainers[decreaseThis].transform.GetChild(playerStatLevels[decreaseThis] - 1).GetComponent<Image>().sprite = levelPipSpriteList[4];
		playerStatLevels[decreaseThis] -= 1;

		IncreaseUnusedLevels();
	}

}
