using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeManager : MonoBehaviour
{
    //public static UpgradeManager inst;

    [HideInInspector] public List<int> playerStatLevels; // Ice, Fire, Lightning, HP
    private int unusedLevels;

    public GameObject upgradePanel; // Upgrade panel GameObject, we will doTween this to move it in and out

    public List<GameObject> emptyPipContainers; // Parent obj of all the empty pip icons

    [Header("Unused Levels Text")]
    public TextMeshProUGUI unusedLevelsTMP;

    [Header("Level Pip Sprite")]
    public List<Sprite> levelPipSpriteList; // Ice Pip, Fire Pip, Lightning Pip, HP Pip, Empty Pip

	//private void Awake()
	//{
	//	if (inst != null && inst != this)
	//	{
	//		Destroy(gameObject);
	//		return;
	//	}
	//	else
	//	{
	//		inst = this;
	//	}

	//	DontDestroyOnLoad(gameObject);
	//}

	// Start is called before the first frame update
	void Start()
    {
		// Add 3 initial values, Ice, Fire, Lightning, HP
		playerStatLevels.Add(0);
		playerStatLevels.Add(0);
		playerStatLevels.Add(0);
		playerStatLevels.Add(0);

        // Initialise number of levels the player has
        unusedLevels = 0;

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
        PlayerCombat playerCombat = GameManager.inst.gpManager.player.combat;
        for (int i = 0; i < playerStatLevels.Count; ++i)
        {
            // Modify the attacks/stats here in the respective switch case below
            // To get the current level for that particular stat -> playerStatLevels[i]
            //switch(i)
            //{
            //    case 0: // ICE
            //        playerCombat.singleTargetIceDamage += (int)(playerCombat.singleTargetIceDamage * 0.5f);
            //        playerCombat.aoeIceDamage += (int)(playerCombat.aoeIceDamage * 0.15f);
            //        playerCombat.singleTargetIceCost = (int)(playerCombat.singleTargetIceCost * 0.5f);
            //        playerCombat.aoeIceCost = (int)(playerCombat.aoeIceCost * 0.5f);
            //        playerCombat.iceAmmo = playerCombat.maxIceAmmo;
            //        break;
            //    case 1: // FIRE
            //        playerCombat.singleTargetFireDamage *= 2;//+= (int)(playerCombat.singleTargetFireDamage * 0.5f);
            //        playerCombat.aoeFireDamage *= 2; //+= (int)(playerCombat.aoeFireDamage * 0.5f);
            //        playerCombat.aoeFireCost = (int)(playerCombat.aoeFireCost * 0.5f);
            //        playerCombat.aoeFireCooldown = (int)(playerCombat.aoeFireCooldown * 0.75f);
            //        playerCombat.fireAmmo = playerCombat.maxFireAmmo;
            //        break;
            //    case 2: // LIGHTNING
            //        playerCombat.singleTargetLightningDamage *= 3; //+= (int)(playerCombat.singleTargetLightningDamage * 0.25F);
            //        playerCombat.aoeLightningDamage *= 3;   //+= (int)(playerCombat.aoeLightningDamage * 0.25F);
            //        playerCombat.singleTargetLightningCost = (int)(playerCombat.singleTargetLightningCost * 0.75f);
            //        playerCombat.aoeLightningCost = (int)(playerCombat.aoeLightningCost * 0.75f);
            //        playerCombat.lightningAmmo = playerCombat.maxLightningAmmo;
            //        break;
            //    case 3: // HP
            //        GameManager.inst.gpManager.player.maxHP *= 2;
            //        GameManager.inst.gpManager.player.hp = GameManager.inst.gpManager.player.maxHP;
            //        GameManager.inst.gpManager.hudInfo.UpdateHP(GameManager.inst.gpManager.player.hp, GameManager.inst.gpManager.player.maxHP);
            //        break;
            //    default:
            //        break;
            //}
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

        PlayerCombat playerCombat = GameManager.inst.gpManager.player.combat;

        switch (increaseThis)
        {
            case 0: // ICE
                playerCombat.singleTargetIceDamage += (int)(playerCombat.singleTargetIceDamage * 0.5f);
                playerCombat.aoeIceDamage += (int)(playerCombat.aoeIceDamage * 0.15f);
                playerCombat.singleTargetIceCost = (int)(playerCombat.singleTargetIceCost * 0.5f);
                playerCombat.aoeIceCost = (int)(playerCombat.aoeIceCost * 0.5f);
                playerCombat.iceAmmo = playerCombat.maxIceAmmo;

                if (playerCombat.singleTargetIceCost < 1)
                    playerCombat.singleTargetIceCost = 1;

                if (playerCombat.aoeIceCost < 5)
                    playerCombat.aoeIceCost = 5;
                break;
            case 1: // FIRE
                playerCombat.singleTargetFireDamage *= 2;//+= (int)(playerCombat.singleTargetFireDamage * 0.5f);
                playerCombat.aoeFireDamage *= 2; //+= (int)(playerCombat.aoeFireDamage * 0.5f);
                playerCombat.aoeFireCost = (int)(playerCombat.aoeFireCost * 0.5f);
                playerCombat.aoeFireCooldown = (int)(playerCombat.aoeFireCooldown * 0.75f);
                playerCombat.fireAmmo = playerCombat.maxFireAmmo;

                if (playerCombat.aoeFireCost < 5)
                    playerCombat.aoeFireCost = 5;
                break;
            case 2: // LIGHTNING
                playerCombat.singleTargetLightningDamage *= 3; //+= (int)(playerCombat.singleTargetLightningDamage * 0.25F);
                playerCombat.aoeLightningDamage *= 3;   //+= (int)(playerCombat.aoeLightningDamage * 0.25F);
                playerCombat.singleTargetLightningCost = (int)(playerCombat.singleTargetLightningCost * 0.75f);
                playerCombat.aoeLightningCost = (int)(playerCombat.aoeLightningCost * 0.75f);
                playerCombat.lightningAmmo = playerCombat.maxLightningAmmo;

                if (playerCombat.aoeLightningCost < 5)
                    playerCombat.aoeLightningCost = 5;
                break;
            case 3: // HP
                GameManager.inst.gpManager.player.maxHP *= 2;
                GameManager.inst.gpManager.player.hp = GameManager.inst.gpManager.player.maxHP;
                GameManager.inst.gpManager.hudInfo.UpdateHP(GameManager.inst.gpManager.player.hp, GameManager.inst.gpManager.player.maxHP);
                break;
            default:
                break;
        }

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

    // Apply stat changes, close the upgrade panel, resume the game
    public void ConfirmButton()
    {
        // Apply stat changes
        UpdateStats();

        // Close the upgrade panel

        upgradePanel.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).SetAutoKill(true).OnComplete(() => { upgradePanel.SetActive(false); });
        //upgradePanel.transform.DOMoveY(-Screen.height / 2, 0.6f).SetUpdate(true).SetEase(Ease.InBack).SetAutoKill(true).OnComplete(()=> { upgradePanel.SetActive(false); });

        // Resume
        Time.timeScale = 1.0f;
        GameManager.inst.gpManager.player.inUpgradeManager = false;
        GameManager.inst.gpManager.player.mouseLook.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Pause the game, open the upgrade panel
    public void OpenUpgradePanel()
    {
        Time.timeScale = 0.0f;
        //Set skillpoints here
        SetUnusedLevels(GameManager.inst.gpManager.player.skillpoints);
        unusedLevelsTMP.text = "Unspent Skill-Points: " + unusedLevels;
        upgradePanel.SetActive(true);

        CanvasGroup cg = upgradePanel.GetComponent<CanvasGroup>();
        cg.DOFade(1f, 0.3f).SetAutoKill(true).SetUpdate(true);

        //Disable mouse looking
        GameManager.inst.gpManager.player.mouseLook.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //upgradePanel.transform.DOMoveY(Screen.height / 2, 0.6f).SetUpdate(true).SetEase(Ease.OutBack).SetAutoKill(true);
    }
}
