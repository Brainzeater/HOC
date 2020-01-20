using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;

public class PlayerSquad : Squad
{
    public TMP_Text unitHPText;
    public TMP_Text unitDamageText;

    public GameObject highlightBackground;
    public GameObject activeSkillButton;
    public GameObject regularHitButton;
    public bool UsedActiveSkill { get; set; }

    public override void Awake()
    {
        base.Awake();
        unitHPText.text = unit.hp.ToString();
        unitDamageText.text = unit.damage.ToString();
        UsedActiveSkill = false;
        //        if(GetUnit.hasActiveSkill)
    }

    // Activates Squad's background highlight and "Active Skill" button
    public void HighlightSquad(bool enabled)
    {
        highlightBackground.SetActive(enabled);

        if (GetUnit.hasActiveSkill)
        {
            if (!UsedActiveSkill)
            {
                activeSkillButton.SetActive(enabled);
            }
            else
            {
                DisableActiveSkillButton();
            }
        }
    }

    public void DisableActiveSkillButton()
    {
        activeSkillButton.SetActive(false);
    }

    public void EnableActiveSkillButton()
    {
        activeSkillButton.SetActive(true);
    }

    public void DisableRegularHitButton()
    {
        regularHitButton.SetActive(false);
    }

    public void EnableRegularHitButton()
    {
        regularHitButton.SetActive(true);
    }
}