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
    public TMP_Text activeSkillText;

    public GameObject regularHitButton;
    public bool UsedActiveSkill { get; set; }
    public int LastDealtDamage { get; set; }

    public override void Awake()
    {
        base.Awake();
        unitHPText.text = unit.hp.ToString();
        unitDamageText.text = unit.damage.ToString();
        UsedActiveSkill = false;
        LastDealtDamage = 0;
        switch (base.GetUnit.activeSkill)
        {
            case ActiveSkill.IncreasedDamage:
                activeSkillText.text = "Crit";
                break;
            case ActiveSkill.DamageAll:
                activeSkillText.text = "Hit All";
                break;
            case ActiveSkill.Heal:
                activeSkillText.text = "Heal";
                break;
        }
    }

    // Activates Squad's background highlight and "Active Skill" button
    public void HighlightSquad(bool enabled, int lastDealtDamage)
    {
        highlightBackground.SetActive(enabled);
        unitHPText.gameObject.SetActive(enabled);
        unitDamageText.gameObject.SetActive(enabled);
        if (GetUnit.hasActiveSkill)
        {
            if (!UsedActiveSkill)
            {
                if ((base.GetUnit.activeSkill == ActiveSkill.IncreasedDamage && lastDealtDamage == 0) || !enabled)
                {
                    DisableActiveSkillButton();
                    DisableRegularHitButton();
                }
                else if (enabled)
                {
                    EnableActiveSkillButton();
                    EnableRegularHitButton();
                    SetPressedRegular(true);
                    SetPressedActive(false);
                }
            }
            else
            {
                DisableActiveSkillButton();
                DisableRegularHitButton();
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

    public void SetPressedRegular(bool enable)
    {
        regularHitButton.GetComponentInChildren<UseActiveSkillButton>().SetPressed(enable);
    }

    public void SetPressedActive(bool enable)
    {
        activeSkillButton.GetComponentInChildren<UseActiveSkillButton>().SetPressed(enable);
    }

    public override void DealDamage()
    {
        Opponent.ReceiveDamage(this.DealingDamage);
        this.LastDealtDamage = this.DealingDamage;
        // Because it might be improved by Increased Damage
        CalculateDealingDamage();
    }

    public override void FinishMoveOfSquadWhoHitMe()
    {
        if (!FindObjectOfType<BattleSystem>().damageAll)
        {
            Debug.Log($"{this} finishes enemy's move");
            FindObjectOfType<BattleSystem>().FinishEnemyTurn();
        }
        else
        {
            Debug.Log($"{this} waits for damage all to finish");
        }
    }

    public void DealIncreasedDamage()
    {
        animator.SetTrigger("Increased");
    }

    public void Heal()
    {
        animator.SetTrigger("Heal");
    }
}