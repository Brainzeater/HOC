using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSquad : Squad
{
    public TMP_Text unitHPText;
    public TMP_Text unitDamageText;

    public GameObject highlightBackground;
    public GameObject activeSkillButton;
    public GameObject regularHitButton;
    private bool usedActiveSkill;

    public override void Awake()
    {
        base.Awake();
        unitHPText.text = unit.hp.ToString();
        unitDamageText.text = unit.damage.ToString();
        usedActiveSkill = false;
        //        if(GetUnit.hasActiveSkill)
    }

    // Activates Squad's background highlight and "Active Skill" button
    public void HighlightSquad(bool enabled)
    {
        highlightBackground.SetActive(enabled);

        if (GetUnit.hasActiveSkill)
        {
            if (!usedActiveSkill)
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

    public void Heal()
    {
    }
}