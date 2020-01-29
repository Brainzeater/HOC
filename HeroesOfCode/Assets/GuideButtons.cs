using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideButtons : MonoBehaviour
{
    public GameObject guide;
    public GameObject mainMenu;

    public void OpenGuide()
    {
        mainMenu.SetActive(false);
        guide.SetActive(true);
    }

    public void CloseGuide()
    {
        guide.SetActive(false);
        mainMenu.SetActive(true);
    }
}