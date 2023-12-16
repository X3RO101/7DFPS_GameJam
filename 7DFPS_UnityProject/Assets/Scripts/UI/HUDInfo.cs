using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HUDInfo : MonoBehaviour
{
    public GameObject crosshairGO = null;               //reference to crosshair in game

    [Header("General Player UI")]
    [SerializeField] private Slider playerHPBar;
    [SerializeField] private Slider playerEXPBar;
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI playerLVText;
    [SerializeField] private TextMeshProUGUI playerEXPText;

    [Header("Elemental UI")]
    [SerializeField] private Image iceCrosshair;
    [SerializeField] private Image fireCrosshair;
    [SerializeField] private Image lightningCrosshair;
    [SerializeField] private List<CanvasGroup> elementAOEContainers;
    [SerializeField] private List<TextMeshProUGUI> elementAmmoText;

    [SerializeField] private List<Image> elementPanelBorders;

    [Header("UI Colors")]
    [SerializeField] private Color panelSelectedColor = Color.yellow;
    [SerializeField] private Color panelDefaultColor = Color.white;
    [SerializeField] private Color damageIndicatorCritColor = Color.yellow;

    [Header("Damage indicator")]
    [SerializeField] private GameObject damageIndicatorPrefab = null;

    [Header("Other UI Info")]
    [SerializeField] private CanvasGroup levelUpPromptContainer = null;
    public UpgradeManager upgradeManager = null;

    public bool isLevelUpPromptEnabled
    {
        get
        {
            return levelUpPromptContainer.gameObject.activeSelf;
        }
    }

    private void FixedUpdate()
    {
        UpdateAOEIndicators();
    }
    private void UpdateAOEIndicators()
    {
        if (GameManager.inst.gpManager.player.combat.CanAOE(GameplayManager.ELEMENTS.ICE))
            UpdateElementAOEContainer(GameplayManager.ELEMENTS.ICE, true);
        else
            UpdateElementAOEContainer(GameplayManager.ELEMENTS.ICE, false);

        if (GameManager.inst.gpManager.player.combat.CanAOE(GameplayManager.ELEMENTS.FIRE))
            UpdateElementAOEContainer(GameplayManager.ELEMENTS.FIRE, true);
        else
            UpdateElementAOEContainer(GameplayManager.ELEMENTS.FIRE, false);

        if (GameManager.inst.gpManager.player.combat.CanAOE(GameplayManager.ELEMENTS.LIGHTNING))
            UpdateElementAOEContainer(GameplayManager.ELEMENTS.LIGHTNING, true);
        else
            UpdateElementAOEContainer(GameplayManager.ELEMENTS.LIGHTNING, false);

        return;
    }
    public void UpdateAmmoCounter(int iceAmmo, int fireAmmo, int lightningAmmo)
    {
        elementAmmoText[(int)GameplayManager.ELEMENTS.ICE].text = iceAmmo.ToString();
        elementAmmoText[(int)GameplayManager.ELEMENTS.FIRE].text = fireAmmo.ToString();
        elementAmmoText[(int)GameplayManager.ELEMENTS.LIGHTNING].text = lightningAmmo.ToString();
    }
    public void UpdateHP(int hp,int maxHP)
    {
        playerHPBar.value = (float)hp / (float)maxHP;
        playerHPText.text = hp.ToString() + "/" + maxHP.ToString();
    }
    public void UpdateEXP(int exp, int maxEXP)
    {
        playerEXPBar.value = (float)exp / (float)maxEXP;
        playerEXPText.text = exp.ToString() + "/" + maxEXP.ToString();
    }
    public void UpdateLevel(int lv)
    {
        playerLVText.text = "LV." + lv.ToString();
    }
    public void UpdateCrosshair(GameplayManager.ELEMENTS elementType)
    {
        switch(elementType)
        {
            case GameplayManager.ELEMENTS.ICE:
                crosshairGO.GetComponent<Image>().sprite = iceCrosshair.sprite;
                break;
            case GameplayManager.ELEMENTS.FIRE:
                crosshairGO.GetComponent<Image>().sprite = fireCrosshair.sprite;
                break;
            case GameplayManager.ELEMENTS.LIGHTNING:
                crosshairGO.GetComponent<Image>().sprite = lightningCrosshair.sprite;
                break;
        }
    }
    public void UpdateElementContainer(GameplayManager.ELEMENTS newElementType, GameplayManager.ELEMENTS previousElementType)
    {
        //Do not tween if new and previous element type are same (occurs when player presses same element button)
        if (newElementType == previousElementType)
            return;

        //Kill any element currently still tweening
        DOTween.Kill(elementPanelBorders[(int)previousElementType]);
        DOTween.Kill(elementPanelBorders[(int)newElementType]);

        elementPanelBorders[(int)previousElementType].DOColor(panelDefaultColor, 0.25f).SetAutoKill(true);
        elementPanelBorders[(int)newElementType].DOColor(panelSelectedColor, 0.25f).SetAutoKill(true);
    }
    public void UpdateElementContainer(GameplayManager.ELEMENTS newElementType)
    {
        DOTween.Kill(elementPanelBorders[(int)newElementType]);

        elementPanelBorders[(int)newElementType].DOColor(panelSelectedColor, 0.25f).SetAutoKill(true);
    }
    public void SetCrosshairEnable(bool enable)
    {
        crosshairGO.SetActive(enable);
    }
    public void UpdateElementAOEContainer(GameplayManager.ELEMENTS elementType, bool enable)
    {
        //Do not tween if its already been tweened to target alpha!
        if ((elementAOEContainers[(int)elementType].alpha == 0 && !enable)
            || (elementAOEContainers[(int)elementType].alpha == 1 && enable))
            return;

        //Kill any element currently still tweening
        DOTween.Kill(elementAOEContainers[(int)elementType]);

        float targetAlpha = 0f;
        Vector3 pos = Vector3.zero;

        if (enable)
        {
            targetAlpha = 1f;
            pos = new Vector3(0, 110f, 0f);
        }

        elementAOEContainers[(int)elementType].DOFade(targetAlpha, 0.1f);
        elementAOEContainers[(int)elementType].transform.DOLocalMoveY(pos.y, 0.25f);
    }
    public void PromptLevelUpText(bool enable)
    {
        if(enable)
        {
            levelUpPromptContainer.gameObject.SetActive(true);
            levelUpPromptContainer.DOFade(1F, 0.25F);
        }
        else
        {
            levelUpPromptContainer.DOFade(0f, 0.25f).OnComplete(() => levelUpPromptContainer.gameObject.SetActive(false));
        }
    }
    public DamageNumber SpawnDamageIndicator()
    {
        DamageNumber damageIndicator = Instantiate(damageIndicatorPrefab).GetComponent<DamageNumber>();
        return damageIndicator;
    }
}
