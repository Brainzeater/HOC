using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmCellMenu : MonoBehaviour
{
    public static bool confirmCellMenuIsOn = false;
    public GameObject ConfirmCellUI;

    void Start()
    {
        GameEvents.current.onCellSelected += OnCellSelected;
    }

    void OnCellSelected()
    {
        confirmCellMenuIsOn = true;
        Debug.Log("Hello");
        ConfirmCellUI.SetActive(true);
    }
}