using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public GameObject playerPrefab;

    private GameData gameData;

    public Transform playerSpawnPosition;

    public GameObject pauseMenu;
    public bool paused;

    void Awake()
    {
        paused = false;
        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();
        if(gameData.playerSpawnPosition == null)
        {
            gameData.playerSpawnPosition = playerSpawnPosition;
        }

        GameObject player = Instantiate(playerPrefab, gameData.playerSpawnPosition.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().Target = player.transform;
        GameObject.FindGameObjectWithTag("Grid").GetComponent<Pathfinding>().Seeker = player.transform;


        foreach (GameObject armyPrefab in gameData.enemyArmyList)
        {
            EnemyArmy currentArmy = armyPrefab.GetComponent<EnemyArmy>();

            Instantiate(currentArmy, currentArmy.spawnPosition.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(FindObjectOfType<SceneLoader>().isReady)
            {
                if (!paused)
                {
                    paused = true;
                    Time.timeScale = 0f;
                    pauseMenu.SetActive(true);
                }
                else
                {
                    paused = false;
                    Time.timeScale = 1f;
                    FindObjectOfType<GuideButtons>().CloseGuide();
                    pauseMenu.SetActive(false);
                }
            }
        }
    }
}