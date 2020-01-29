using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    private GameData gameData;
    void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: Send its ID! We need to know which army to load
        // TODO: After defeating the player will start from his position
        
        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();
        gameData.CurrentEnemyArmyIndex = gameObject.GetComponentInParent<EnemyArmy>().armyIndex;
        gameData.InitializeBattleArmies();
        FindObjectOfType<AudioManager>().Play("Drum");
        FindObjectOfType<SceneLoader>().LoadBattleScene();
    }
}
