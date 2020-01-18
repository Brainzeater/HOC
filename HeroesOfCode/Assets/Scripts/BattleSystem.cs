﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public BattleState state;

    public GameObject playerArmyPosition;
    private Transform[] playerSquadPositionsArray;
    public GameObject enemyArmyPosition;
    private Transform[] enemySquadPositionsArray;

    private GameData gameData;

    private Queue<Squad> playerArmyQueue;
    private Queue<Squad> enemyArmyQueue;
    private List<Squad> enemyArmyList;

    private Squad currentPlayerSquad;
    private Squad currentEnemySquad;


    void Start()
    {
        playerSquadPositionsArray = playerArmyPosition.GetComponentsInChildren<Transform>();
        playerArmyQueue = new Queue<Squad>();

        enemySquadPositionsArray = enemyArmyPosition.GetComponentsInChildren<Transform>();
        enemyArmyQueue = new Queue<Squad>();

        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();

        BattleEvents.current.OnTargetSelected += OnTargetChosen;
        state = BattleState.START;
//        StartCoroutine(SetupBattle());
        SetupBattle();
    }

//    IEnumerator SetupBattle()
    void SetupBattle()
    {
        int i = 1;
        foreach (GameData.UnitSquad squad in gameData.playerArmy)
        {
            GameObject playerGO = Instantiate(squad.squadUnitPrefab, playerSquadPositionsArray[i]);
            currentPlayerSquad = playerGO.GetComponent<Squad>();
            currentPlayerSquad.ID = squad.SquadID;
            // TODO: This is just ridiculous!
            currentPlayerSquad.SetSquadHP(squad.SquadHp);
            playerArmyQueue.Enqueue(currentPlayerSquad);
            i++;
        }
//        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattlePosition);
//        enemy = enemyGO.GetComponent<EnemySquad>();

        i = 1;
        foreach (GameData.UnitSquad squad in gameData.enemyArmy1)
        {
            GameObject enemyGO = Instantiate(squad.squadUnitPrefab, enemySquadPositionsArray[i]);
            currentEnemySquad = enemyGO.GetComponent<Squad>();
            currentEnemySquad.ID = squad.SquadID;
            // TODO: This is just ridiculous!
            currentEnemySquad.SetSquadHP(squad.SquadHp);
            enemyArmyQueue.Enqueue(currentEnemySquad);
            i++;
        }

        enemyArmyList = enemyArmyQueue.ToList();


        //        yield return new WaitForSeconds(1f);
        //        print(player.DealingDamage);
        //        print(enemy.DealingDamage);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        // Get the next player squad from the Queue
        currentPlayerSquad = playerArmyQueue.Dequeue();
        // Cleaning the queue from the dead squads
        while (currentPlayerSquad.IsDead)
        {
            currentPlayerSquad = playerArmyQueue.Dequeue();
        }
        // Highlight current squad
        currentPlayerSquad.HighlightSquad(true);
    }

    // Called when the player selects the target enemy to hit
    public void OnTargetChosen()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        // Unhighlight current squad and put it at the end of the Queue
        currentPlayerSquad.HighlightSquad(false);
        playerArmyQueue.Enqueue(currentPlayerSquad);

        // Find an enemy squad that takes damage and updates its hp
        currentEnemySquad = enemyArmyList.Find(item => item.ID == BattleEvents.current.SelectedTargetID);
        currentEnemySquad.ReceiveDamage(currentPlayerSquad.DealingDamage);
        
        // Remove enemy squad from the game data in case it's dead
        if (currentEnemySquad.IsDead)
        {
            gameData.enemyArmy1.RemoveAll(item => item.SquadID == currentEnemySquad.ID);
        }

        // The battle goes on if an enemy still has squads.
        // Otherwise, the player won this battle.
        if (gameData.enemyArmy1.Any())
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
        else
        {
            state = BattleState.WON;
            EndBattle();
        }
    }

    void EnemyTurn()
    {
        if (state != BattleState.ENEMYTURN)
            return;
        currentEnemySquad = enemyArmyQueue.Dequeue();
        while (currentEnemySquad.IsDead)
        {
            currentEnemySquad = enemyArmyQueue.Dequeue();
        }
        
        // TODO: AI or minimal strategy
        currentPlayerSquad.ReceiveDamage(currentEnemySquad.DealingDamage);

        if (currentPlayerSquad.IsDead)
        {
            gameData.playerArmy.RemoveAll(item => item.SquadID == currentPlayerSquad.ID);
        }

        if (gameData.playerArmy.Any())
        {
            enemyArmyQueue.Enqueue(currentEnemySquad);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            state = BattleState.LOST;
            EndBattle();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            // If that was not the last knight, then
            // Destroy the defeated knight and Load Map
            // else Load WIN SCENE!
            UpdateUnitSquadHP();
            SceneLoader.LoadMapScene();
        }
        else if (state == BattleState.LOST)
        {
            // Load GameOver Scene
            print("YOU LOST");
        }
    }

    // Save each squad's hp in the Game Data
    void UpdateUnitSquadHP()
    {
        while (playerArmyQueue.Count > 0)
        {
            currentPlayerSquad = playerArmyQueue.Dequeue();
            gameData.playerArmy.Find(item => item.SquadID == currentPlayerSquad.ID).SquadHp = currentPlayerSquad.HP;
        }
    }
}