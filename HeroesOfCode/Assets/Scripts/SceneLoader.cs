using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void LoadBattleScene()
    {
        ConfirmCellMenu.confirmCellMenuIsOn = false;
        // TODO: There's other stuff to be finished here before loading. Save player's position!
        SceneManager.LoadScene(1);
    }

    public static void LoadMapScene()
    {
        SceneManager.LoadScene(0);
    }
}