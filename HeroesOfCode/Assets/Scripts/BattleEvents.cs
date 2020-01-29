using System;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents current;
    public int SelectedTargetID { get; set; }

    private void Awake()
    {
        current = this;
    }

    // Called when the next target squad is selected
    public event Action OnTargetSelected;

    public void TargetSelected(int targetID)
    {
        SelectedTargetID = targetID;
        OnTargetSelected?.Invoke();
    }

    // Called when the player uses active skill
    public event Action OnActiveSkillSelected;

    public void ActiveSkill()
    {
        OnActiveSkillSelected?.Invoke();
    }

    // Called when the player returns to regular hit
    public event Action OnRegularHitSelected;

    public void RegularHit()
    {
        OnRegularHitSelected?.Invoke();
    }

    // Called when the player selects target with "Damage All"
    public event Action OnDamageAllHighlight;

    public void HighlightAll()
    {
        OnDamageAllHighlight?.Invoke();
    }

    public event Action OnDamageAllSquads;

    public void DamageAllSquads()
    {
        OnDamageAllSquads?.Invoke();
    }

    public event Action OnUnhighlight;
    public void Unhighlight()
    {
        OnUnhighlight?.Invoke();
    }
}