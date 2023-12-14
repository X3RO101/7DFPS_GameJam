using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerCombat combat;
    public CameraSway camSway;
    public MouseLook mouseLook;
    public int hp;
    public int maxHP;
    public int lv = 1;
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
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(isReadyToLevelUp)
            {
                //Show upgrade
                GameManager.inst.gpManager.hudInfo.PromptLevelUpText(false);
            }
        }
    }
}
