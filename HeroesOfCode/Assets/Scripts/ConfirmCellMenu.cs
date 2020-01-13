using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmCellMenu : MonoBehaviour
{
    public static bool confirmCellMenuIsOn = false;
    public GameObject ConfirmCellUI;
    public GameObject Player;
    public GameObject AStar;

    void Start()
    {
        GameEvents.current.OnCellSelected += OnCellSelected;
        GameEvents.current.OnMovementFinished += FinishMovement;
    }

    void OnCellSelected()
    {
        confirmCellMenuIsOn = true;
        ConfirmCellUI.SetActive(true);
    }

    public void Confirm()
    {
        ConfirmCellUI.SetActive(false);
        Vector3[] path = AStar.GetComponent<Grid>().worldPath;
        Player.GetComponent<Movement>().BeginMovement(path);
    }

    public void Cancel()
    {
        confirmCellMenuIsOn = false;
        ConfirmCellUI.SetActive(false);
        GameEvents.current.CellCancelled();
    }

    void FinishMovement()
    {
        confirmCellMenuIsOn = false;
    }
}