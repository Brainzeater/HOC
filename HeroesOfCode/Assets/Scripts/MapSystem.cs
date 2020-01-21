using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    private GameData gameData;

    void Start()
    {
        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();
        foreach (GameObject armyPrefab in gameData.enemyArmyList)
        {
            EnemyArmy currentArmy = armyPrefab.GetComponent<EnemyArmy>();

            Instantiate(currentArmy, currentArmy.spawnPosition.position, Quaternion.identity);
        }
    }
}