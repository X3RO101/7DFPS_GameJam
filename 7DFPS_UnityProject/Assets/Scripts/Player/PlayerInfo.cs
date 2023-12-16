using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerCombat combat;
    public CameraSway camSway;
    public MouseLook mouseLook;
    public bool died = false;
    public bool inUpgradeManager = false;
    public int hp;
    public int maxHP;
    public int lv = 1;
    public int skillpoints = 0;
    public int currExp = 0;
    public int maxExp = 300;
    


    public bool isReadyToLevelUp
    {
        get
        {
            if (currExp >= maxExp)
                return true;
            else
                return false;
        }
    }

    private void Start()
    {
        //Init player stuff
        GameManager.inst.gpManager.hudInfo.UpdateHP(hp, maxHP);
        GameManager.inst.gpManager.hudInfo.UpdateEXP(currExp, maxExp);
        GameManager.inst.gpManager.hudInfo.UpdateLevel(1);
        GameManager.inst.gpManager.hudInfo.UpdateElementContainer(combat.equippedElement);
        GameManager.inst.gpManager.hudInfo.UpdateCrosshair(combat.equippedElement);
        died = false;
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(skillpoints > 0)
            {
                //Show upgrade
                GameManager.inst.gpManager.hudInfo.PromptLevelUpText(false);
                GameManager.inst.gpManager.hudInfo.upgradeManager.OpenUpgradePanel();
                inUpgradeManager = true;
            }
        }

        if(!died)
        {
            if (hp <= 0)
            {
                Time.timeScale = 0;
                GameManager.inst.gpManager.hudInfo.ShowGameOverScreen();
                died = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenuScene");
                Time.timeScale = 1;
            }
        }

    }
}
