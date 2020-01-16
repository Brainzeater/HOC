using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}



public class BattleSystem : MonoBehaviour
{
    public GameObject playerSkeleton;
    public GameObject playerKnightBlob;
    public GameObject playerGoblin;
    public GameObject playerShootingBlob;

    public GameObject enemyPrefab;

//    public Transform playerBattlePosition;
    public Transform enemyBattlePosition;

    public GameObject playerArmyPosition;
    private Transform[] playerSquadPositionsArray;

    private PlayerSquad player;
    private EnemySquad enemy;

    public BattleState state;

    private Queue<Squad> playerArmyQueue;
    private Queue<Squad> enemyArmyQueue;

    
    void Start()
    {
        playerSquadPositionsArray = playerArmyPosition.GetComponentsInChildren<Transform>();
        playerArmyQueue = new Queue<Squad>();

        BattleEvents.current.OnTargetSelected += OnTargetChosen;
        state = BattleState.START;
//        StartCoroutine(SetupBattle());
        SetupBattle();
    }

//    IEnumerator SetupBattle()
    void SetupBattle()
    {
        int i = 0;
        GameData data = GameObject.FindWithTag("GameData").GetComponent<GameData>();
        foreach (int goblin in data.goblins)
        {
            GameObject playerGO = Instantiate(playerGoblin, playerSquadPositionsArray[i+1]);
            player = playerGO.GetComponent<PlayerSquad>();
            player.SetSquadHP(goblin);
            playerArmyQueue.Enqueue(playerGO.GetComponent<PlayerSquad>());
            i++;
        }
        foreach (int knightBlob in data.knightBlobs)
        {
            GameObject playerGO = Instantiate(playerKnightBlob, playerSquadPositionsArray[i + 1]);
            player = playerGO.GetComponent<PlayerSquad>();
            player.SetSquadHP(knightBlob);
            playerArmyQueue.Enqueue(playerGO.GetComponent<PlayerSquad>());
            i++;
        }
        foreach (int shootingBlob in data.shootingBlobs)
        {
            GameObject playerGO = Instantiate(playerShootingBlob, playerSquadPositionsArray[i + 1]);
            player = playerGO.GetComponent<PlayerSquad>();
            player.SetSquadHP(shootingBlob);
            playerArmyQueue.Enqueue(playerGO.GetComponent<PlayerSquad>());
            i++;
        }
        foreach (int skeleton in data.skeltons)
        {
            GameObject playerGO = Instantiate(playerSkeleton, playerSquadPositionsArray[i + 1]);
            player = playerGO.GetComponent<PlayerSquad>();
            player.SetSquadHP(skeleton);
            playerArmyQueue.Enqueue(playerGO.GetComponent<PlayerSquad>());
            i++;
        }
        //        GameObject playerGO = Instantiate(playerSkeleton, playerSquadPositionsArray[1]);
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattlePosition);
//        player = playerGO.GetComponent<PlayerSquad>();
        enemy = enemyGO.GetComponent<EnemySquad>();


//        yield return new WaitForSeconds(1f);
//        print(player.DealingDamage);
//        print(enemy.DealingDamage);
        state = BattleState.PLAYERTURN;
//        PlayerTurn();
    }

//    void PlayerTurn()
//    {
//    }

    public void OnTargetChosen()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        
        enemy.ReceiveDamage(player.DealingDamage);
        if (enemy.IsDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    void EnemyTurn()
    {
        // Do we need any strategy here?!

        if (state != BattleState.ENEMYTURN)
            return;
        player.ReceiveDamage(enemy.DealingDamage);
        if (player.IsDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
//            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            // If that was not the last knight, then
            // Destroy the defeated knight and Load Map
            // else Load WIN SCENE!
            SceneLoader.LoadMapScene();
        }
        else if(state == BattleState.LOST)
        {
            // Load GameOver Scene
        }
    }
}