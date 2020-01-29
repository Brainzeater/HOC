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

    public void OpenGuideKeepMenu()
    {
        guide.SetActive(true);
    }

    public void CloseGuide()
    {
        guide.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<GameData>().GameOver();
        FindObjectOfType<SceneLoader>().LoadStartScene();
    }
}