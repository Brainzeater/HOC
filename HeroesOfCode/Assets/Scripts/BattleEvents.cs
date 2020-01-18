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
}