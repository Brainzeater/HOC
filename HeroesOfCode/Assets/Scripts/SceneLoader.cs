using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadBattleScene()
    {
        ConfirmCellMenu.confirmCellMenuIsOn = false;
        // TODO: There's other stuff to be finished here before loading. Save player's position!
        SceneManager.LoadScene(2);
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadBadEndingScene()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadGoodEndingScene()
    {
        SceneManager.LoadScene(4);
    }
}