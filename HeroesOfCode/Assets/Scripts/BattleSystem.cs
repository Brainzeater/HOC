using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerRegularTurn,
    PlayerActiveSkill,
    EnemyTurn,
    Won,
    Lost
}

public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    public GameObject playerArmyPosition;
    private Transform[] playerSquadPositionsArray;
    public GameObject enemyArmyPosition;
    private Transform[] enemySquadPositionsArray;

    private GameData gameData;

    private Queue<PlayerSquad> playerArmyQueue;
    private Queue<Squad> enemyArmyQueue;
    private List<PlayerSquad> playerArmyList;
    private List<Squad> enemyArmyList;

    private PlayerSquad currentPlayerSquad;
    private Squad currentEnemySquad;


    void Start()
    {
        playerSquadPositionsArray = playerArmyPosition.GetComponentsInChildren<Transform>();
        playerArmyQueue = new Queue<PlayerSquad>();

        enemySquadPositionsArray = enemyArmyPosition.GetComponentsInChildren<Transform>();
        enemyArmyQueue = new Queue<Squad>();

        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();

        BattleEvents.current.OnTargetSelected += OnTargetChosen;
        BattleEvents.current.OnActiveSkillSelected += OnActiveSkill;
        BattleEvents.current.OnRegularHitSelected += OnRegularHit;

        state = BattleState.Start;
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

        // TODO: Better Update it each time as it might contain dead squads
        playerArmyList = playerArmyQueue.ToList();
        enemyArmyList = enemyArmyQueue.ToList();


        //        yield return new WaitForSeconds(1f);
        //        print(player.DealingDamage);
        //        print(enemy.DealingDamage);
        state = BattleState.PlayerRegularTurn;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        ToggleArmyToSelect(enemyArmyList, true);

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
        if (state != BattleState.PlayerRegularTurn)
            return;

        ToggleArmyToSelect(enemyArmyList, false);

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
            state = BattleState.EnemyTurn;
            EnemyTurn();
        }
        else
        {
            state = BattleState.Won;
            EndBattle();
        }
    }

    public void OnActiveSkill()
    {
        currentPlayerSquad.EnableRegularHitButton();
        currentPlayerSquad.DisableActiveSkillButton();
        state = BattleState.PlayerActiveSkill;
        
        switch (currentPlayerSquad.GetUnit.activeSkill)
        {
            case ActiveSkill.Heal:

                break;
            case ActiveSkill.IncreasedDamage:

                break;
            case ActiveSkill.DamageAll:

                break;
        }
    }

    public void OnRegularHit()
    {
        currentPlayerSquad.EnableActiveSkillButton();
        currentPlayerSquad.DisableRegularHitButton();
        state = BattleState.PlayerRegularTurn;
    }

    void EnemyTurn()
    {
        if (state != BattleState.EnemyTurn)
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
            state = BattleState.PlayerRegularTurn;
            PlayerTurn();
        }
        else
        {
            state = BattleState.Lost;
            EndBattle();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            // If that was not the last knight, then
            // Destroy the defeated knight and Load Map
            // else Load WIN SCENE!
            UpdateUnitSquadHP();
            SceneLoader.LoadMapScene();
        }
        else if (state == BattleState.Lost)
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

    // Activate/Deactivate army for selection
    void ToggleArmyToSelect(List<Squad> army, bool active)
    {
        foreach (Squad squad in army)
        {
            squad.gameObject.GetComponentInChildren<SelectTarget>().Active = active;
        }
    }
}