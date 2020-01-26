using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public GameObject playerPrefab;

    private GameData gameData;

    public Transform playerSpawnPosition;

    void Awake()
    {
        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();
        if(gameData.playerSpawnPosition.Equals(null))
        {
            gameData.playerSpawnPosition = playerSpawnPosition;
        }

        GameObject player = Instantiate(playerPrefab, gameData.playerSpawnPosition.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().Target = player.transform;
        GameObject.FindGameObjectWithTag("Grid").GetComponent<Pathfinding>().Seeker = player.transform;
        GameObject.FindGameObjectWithTag("ConfirmCellMenu").GetComponent<ConfirmCellMenu>().Player = player;


        foreach (GameObject armyPrefab in gameData.enemyArmyList)
        {
            EnemyArmy currentArmy = armyPrefab.GetComponent<EnemyArmy>();

            Instantiate(currentArmy, currentArmy.spawnPosition.position, Quaternion.identity);
        }
    }
}