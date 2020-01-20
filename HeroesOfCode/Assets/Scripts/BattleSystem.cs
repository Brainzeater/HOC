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
    private List<Squad> playerSquadsToHealList;
    private List<Squad> enemyArmyList;

    private PlayerSquad currentPlayerSquad;
    private Squad currentEnemySquad;

    // For "Increased Damage" active skill
    private int lastDealtDamage;


    void Start()
    {
        playerSquadPositionsArray = playerArmyPosition.GetComponentsInChildren<Transform>();
        playerArmyQueue = new Queue<PlayerSquad>();
        playerSquadsToHealList = new List<Squad>();

        enemySquadPositionsArray = enemyArmyPosition.GetComponentsInChildren<Transform>();
        enemyArmyQueue = new Queue<Squad>();

        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();

        BattleEvents.current.OnTargetSelected += OnTargetChosen;
        // Active Skills
        BattleEvents.current.OnTargetSelected += OnHeal;

        BattleEvents.current.OnActiveSkillSelected += OnActiveSkill;
        BattleEvents.current.OnRegularHitSelected += OnRegularHit;

        lastDealtDamage = 0;

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
        currentPlayerSquad.HighlightSquad(true, lastDealtDamage);
    }

    // Activates when the player selects the target enemy to deal regular hit or active skill "Increased Damage"
    public void OnTargetChosen()
    {
        int dealingDamage = 0;
        if (state != BattleState.PlayerRegularTurn)
            if (!(state == BattleState.PlayerActiveSkill &&
                  currentPlayerSquad.GetUnit.activeSkill == ActiveSkill.IncreasedDamage))
            {
                return;
            }
            else
            {
                // When the player chose to use "Increased Damage"
                currentPlayerSquad.DisableRegularHitButton();
                currentPlayerSquad.UsedActiveSkill = true;
                dealingDamage = Unit.increasedConstant + Unit.increasedCoefficient * lastDealtDamage;
            }
        else
        {
            // When the player uses regular hit
            dealingDamage = currentPlayerSquad.DealingDamage;
            if (currentPlayerSquad.GetUnit.activeSkill == ActiveSkill.IncreasedDamage)
            {
                lastDealtDamage = dealingDamage;
            }
        }

        ToggleArmyToSelect(enemyArmyList, false);

        // Unhighlight current squad and put it at the end of the Queue
        currentPlayerSquad.HighlightSquad(false, 0);
        playerArmyQueue.Enqueue(currentPlayerSquad);

        // Find an enemy squad that takes damage and updates its hp
        currentEnemySquad = enemyArmyList.Find(item => item.ID == BattleEvents.current.SelectedTargetID);
        currentEnemySquad.ReceiveDamage(dealingDamage);

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

    // Activates when the player presses Active Skill button
    public void OnActiveSkill()
    {
        currentPlayerSquad.EnableRegularHitButton();
        currentPlayerSquad.DisableActiveSkillButton();
        state = BattleState.PlayerActiveSkill;

        switch (currentPlayerSquad.GetUnit.activeSkill)
        {
            // Allow selecting player's squads to heal
            case ActiveSkill.Heal:
                ToggleArmyToSelect(enemyArmyList, false);
                ToggleArmyToSelect(playerSquadsToHealList, true);
                break;
            case ActiveSkill.IncreasedDamage:

                break;
            case ActiveSkill.DamageAll:

                break;
        }
    }

    // Activates when the player exits Active Skill without using it
    public void OnRegularHit()
    {
        currentPlayerSquad.EnableActiveSkillButton();
        currentPlayerSquad.DisableRegularHitButton();

        // Unihighlight player's squads if the player exited from Heal
        if (currentPlayerSquad.GetUnit.activeSkill == ActiveSkill.Heal)
        {
            ToggleArmyToSelect(enemyArmyList, true);
            ToggleArmyToSelect(playerSquadsToHealList, false);
        }

        state = BattleState.PlayerRegularTurn;
    }

    // Healing Active Skill
    void OnHeal()
    {
        if (!(state == BattleState.PlayerActiveSkill && 
              currentPlayerSquad.GetUnit.activeSkill == ActiveSkill.Heal))
            return;

        // Disable buttons
        currentPlayerSquad.DisableRegularHitButton();
        currentPlayerSquad.UsedActiveSkill = true;
        ToggleArmyToSelect(playerSquadsToHealList, false);

        // Unhighlight current squad and put it at the end of the Queue
        currentPlayerSquad.HighlightSquad(false, 0);
        playerArmyQueue.Enqueue(currentPlayerSquad);
        // Find an player squad that restores hp and updates its hp
        Squad playerSquadToHeal = playerSquadsToHealList.Find(item => item.ID == BattleEvents.current.SelectedTargetID);

        int selectedSquadMaxHp = gameData.playerArmy.Find(item => item.SquadID == playerSquadToHeal.ID).SquadHp;
        // TODO: Tune this thing. It should be Unit's HP
        int healHP = (int) Mathf.Round(selectedSquadMaxHp * Unit.healPercent);
//        print(playerSquadToHeal.HP);
//        print(healHP);
        playerSquadToHeal.ReceiveDamage(-healHP);
//        print(playerSquadToHeal.HP);

        // Not to overheal the Squad
        if (playerSquadToHeal.HP > selectedSquadMaxHp)
        {
            playerSquadToHeal.SetSquadHP(selectedSquadMaxHp);
        }

        state = BattleState.EnemyTurn;
        EnemyTurn();
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

        // TODO: Find out, who hits me when I kill him!
        print(currentEnemySquad);
        // TODO: AI or minimal strategy
        Squad playerSquadToHit = currentPlayerSquad;
        playerSquadToHit.ReceiveDamage(currentEnemySquad.DealingDamage);

        if (playerSquadToHit.IsDead)
        {
            gameData.playerArmy.RemoveAll(item => item.SquadID == playerSquadToHit.ID);
            playerSquadsToHealList.RemoveAll(item => item.ID == playerSquadToHit.ID);
        }
        else
        {
            playerSquadsToHealList.Add(playerSquadToHit);
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