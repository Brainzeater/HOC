using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    // Called when the next target cell is selected
    public event Action OnCellSelected;

    public void CellSelected()
    {
        OnCellSelected?.Invoke();
    }

    // Called when the chosen target cell is cancelled
    public event Action OnCellCancelled;

    public void CellCancelled()
    {
        OnCellCancelled?.Invoke();
    }

    // Called when the player's movement is over
    public event Action OnMovementFinished;

    public void MovementFinished()
    {
        OnMovementFinished?.Invoke();
    }
}
