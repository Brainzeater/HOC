using System.Collections.Generic;
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
    public GameObject enemyPrefab;

//    public Transform playerBattlePosition;
    public Transform enemyBattlePosition;

    public GameObject playerArmyPosition;
    private Transform[] playerSquadPositionsArray;

    private GameData gameData;
//    private List<GameData.UnitSquad> playerArmy;
    private Squad currentPlayerSquad;
    private EnemySquad enemy;

    public BattleState state;

    private Queue<Squad> playerArmyQueue;
    private Queue<Squad> enemyArmyQueue;


    void Start()
    {
        playerSquadPositionsArray = playerArmyPosition.GetComponentsInChildren<Transform>();
        playerArmyQueue = new Queue<Squad>();
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
            currentPlayerSquad = playerGO.GetComponent<PlayerSquad>();
            currentPlayerSquad.ID = squad.SquadID;
            // TODO: This is just ridiculous!
            currentPlayerSquad.SetSquadHP(squad.SquadHp);
            playerArmyQueue.Enqueue(currentPlayerSquad);
            i++;
        }
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattlePosition);
//        player = playerGO.GetComponent<PlayerSquad>();
        enemy = enemyGO.GetComponent<EnemySquad>();


//        yield return new WaitForSeconds(1f);
//        print(player.DealingDamage);
//        print(enemy.DealingDamage);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        foreach (Squad squad in playerArmyQueue)
        {
            print(squad);
        }
        currentPlayerSquad = playerArmyQueue.Dequeue();
        while (currentPlayerSquad.IsDead)
        {
            currentPlayerSquad = playerArmyQueue.Dequeue();
        }
        currentPlayerSquad.HighlightSquad(true);
    }

    public void OnTargetChosen()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        currentPlayerSquad.HighlightSquad(false);
        playerArmyQueue.Enqueue(currentPlayerSquad);
        enemy.ReceiveDamage(currentPlayerSquad.DealingDamage);
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

        if (state != BattleState.ENEMYTURN)
            return;
        // Do we need any strategy here?!
        currentPlayerSquad.ReceiveDamage(enemy.DealingDamage);

        if (currentPlayerSquad.IsDead)
        {
            print($"Id: {currentPlayerSquad.ID}");
            gameData.playerArmy.RemoveAll(item => item.SquadID == currentPlayerSquad.ID);
        }

        if (gameData.playerArmy.Any())
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else { 
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

    void UpdateUnitSquadHP()
    {
        while (playerArmyQueue.Count > 0)
        {
            currentPlayerSquad = playerArmyQueue.Dequeue();
            gameData.playerArmy.Find(item => item.SquadID == currentPlayerSquad.ID).SquadHp = currentPlayerSquad.HP;
        }
    }
}