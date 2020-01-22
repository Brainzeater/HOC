using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the user input in the Confirm Cell Menu.
public class ConfirmCellMenu : MonoBehaviour
{
    public static bool confirmCellMenuIsOn = false;
    public GameObject ConfirmCellUI;
    public GameObject AStar;
    public GameObject Player { get; set; }

    void Start()
    {
        GameEvents.current.OnCellSelected += OnCellSelected;
        GameEvents.current.OnMovementFinished += FinishMovement;
    }

    // Forbids the cell choosing and shows a UI to confirm the selected cell.
    void OnCellSelected()
    {
        confirmCellMenuIsOn = true;
        ConfirmCellUI.SetActive(true);
    }

    // Hides the UI and causes the player to move along the path.
    public void Confirm()
    {
        ConfirmCellUI.SetActive(false);
        Vector3[] path = AStar.GetComponent<Grid>().worldPath;
        Player.GetComponent<Movement>().BeginMovement(path);
    }

    // Allows the following cell choosing, hides the UI and causes the Grid to erase the highlighted path.
    public void Cancel()
    {
        confirmCellMenuIsOn = false;
        ConfirmCellUI.SetActive(false);
        GameEvents.current.CellCancelled();
    }

    // Allows the cell choosing when the player movement is finished.
    void FinishMovement()
    {
        confirmCellMenuIsOn = false;
    }
}