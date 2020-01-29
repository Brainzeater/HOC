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

    // Called when the player's movement is over
    public event Action OnMovementFinished;

    public void MovementFinished()
    {
        OnMovementFinished?.Invoke();
    }
}
