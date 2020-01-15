using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents current;
//    public BattleSystem battleSystem;

    private void Awake()
    {
        current = this;
    }

    // Called when the next target cell is selected
    public event Action OnTargetSelected;

    public void TargetSelected(int targetIndex)
    {
//        battleSystem.
        OnTargetSelected?.Invoke();
    }
}
