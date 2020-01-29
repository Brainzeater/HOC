using System.Collections;
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
    private PlayerSquad playerSquadToHit;

    private List<Squad> playerSquadsToHealList;
    private Squad playerSquadToHeal;

    // For "Increased Damage" active skill
    private int currentDealingDamage;

    private bool increased;
    public bool damageAll;


    void Start()
    {
        playerSquadPositionsArray = playerArmyPosition.GetComponentsInChildren<Transform>();
        playerArmyQueue = new Queue<PlayerSquad>();
        playerSquadsToHealList = new List<Squad>();

        enemySquadPositionsArray = enemyArmyPosition.GetComponentsInChildren<Transform>();
        enemyArmyQueue = new Queue<Squad>();

        gameData = GameObject.FindWithTag("GameData").GetComponent<GameData>();
        if (gameData.enemyArmyList.Count == 1)
        {
            FindObjectOfType<SceneLoader>().SetFinalBattleMusic();
        }


        BattleEvents.current.OnTargetSelected += OnTargetChosen;
        // Active Skills
        BattleEvents.current.OnTargetSelected += OnHeal;
        BattleEvents.current.OnDamageAllSquads += OnDamageAll;

        BattleEvents.current.OnActiveSkillSelected += OnActiveSkill;
        BattleEvents.current.OnRegularHitSelected += OnRegularHit;

        increased = false;
        damageAll = false;

        state = BattleState.Start;
        SetupBattle();
    }

    void SetupBattle()
    {
        // Sort player army squads by initiative
        gameData.playerArmy =
            gameData.playerArmy.OrderByDescending(item => item.squadUnitPrefab.GetComponent<Squad>().GetUnit.initiative)
                .ToList();

        int i = 1;
        foreach (GameData.UnitSquad squad in gameData.playerArmy)
        {
            GameObject playerGO = Instantiate(squad.squadUnitPrefab, playerSquadPositionsArray[i]);
            currentPlayerSquad = playerGO.GetComponent<PlayerSquad>();
            currentPlayerSquad.ID = squad.SquadID;
            currentPlayerSquad.SetSquadHP(squad.SquadHp);
            playerArmyQueue.Enqueue(currentPlayerSquad);
            i++;
        }

        // Sort enemy army squads by initiative
        List<GameData.UnitSquad> enemyArmy = gameData.GetCurrentEnemyArmy().army;
        enemyArmy = enemyArmy.OrderByDescending(item => item.squadUnitPrefab.GetComponent<Squad>().GetUnit.initiative)
            .ToList();

        i = 1;
        foreach (GameData.UnitSquad squad in enemyArmy)
        {
            GameObject enemyGO = Instantiate(squad.squadUnitPrefab, enemySquadPositionsArray[i]);
            currentEnemySquad = enemyGO.GetComponent<Squad>();
            currentEnemySquad.ID = squad.SquadID;
            currentEnemySquad.SetSquadHP(squad.SquadHp);
            enemyArmyQueue.Enqueue(currentEnemySquad);
            i++;
        }

        // TODO: Better Update it each time as it might contain dead squads
        enemyArmyList = enemyArmyQueue.ToList();

        playerArmyList = playerArmyQueue.ToList();

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
        currentPlayerSquad.HighlightSquad(true, currentPlayerSquad.LastDealtDamage);
    }

    // Activates when the player selects the target enemy to deal regular hit or active skill "Increased Damage"
    public void OnTargetChosen()
    {
        currentDealingDamage = 0;
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
                currentDealingDamage = Unit.increasedConstant +
                                       Unit.increasedCoefficient * currentPlayerSquad.LastDealtDamage;
                increased = true;
            }
        else
        {
            // When the player uses regular hit
            currentDealingDamage = currentPlayerSquad.DealingDamage;
        }

        // Disable enemy squad choosing
        ToggleArmyToSelect(enemyArmyList, false);

        // Unhighlight current squad and put it at the end of the Queue
        currentPlayerSquad.HighlightSquad(false, 0);
        playerArmyQueue.Enqueue(currentPlayerSquad);

        // Find an enemy squad that takes damage
        currentEnemySquad = enemyArmyList.Find(item => item.ID == BattleEvents.current.SelectedTargetID);

        currentPlayerSquad.DealingDamage = currentDealingDamage;
        currentPlayerSquad.Opponent = currentEnemySquad;
        Debug.Log($"Player {currentPlayerSquad}\n attacks {currentEnemySquad}");
        if (!increased)
        {
            currentPlayerSquad.Attack();
        }
        else
        {
            increased = false;
            currentPlayerSquad.DealIncreasedDamage();
        }
    }

    public void FinishPlayerTurn()
    {
        state = NextBattleStateForPlayerTurn(currentEnemySquad);
        switch (state)
        {
            case BattleState.EnemyTurn:
                EnemyTurn();
                break;
            case BattleState.Won:
                EndBattle();
                break;
        }
    }

    // Activates when the player presses Active Skill button
    public void OnActiveSkill()
    {
        // Display correct button as selected
        currentPlayerSquad.SetPressedRegular(false);
        currentPlayerSquad.SetPressedActive(true);

        state = BattleState.PlayerActiveSkill;

        switch (currentPlayerSquad.GetUnit.activeSkill)
        {
            // Allow selecting player's squads to heal
            case ActiveSkill.Heal:
                ToggleArmyToSelect(enemyArmyList, false);
                ToggleArmyToSelect(playerSquadsToHealList, true);
                break;
            case ActiveSkill.DamageAll:
                print("DAMAGE ALL");
                DamageAllPreset();
                break;
        }
    }

    // Activates when the player exits Active Skill without using it
    public void OnRegularHit()
    {
        // Display correct button as selected
        currentPlayerSquad.SetPressedRegular(true);
        currentPlayerSquad.SetPressedActive(false);

        switch (currentPlayerSquad.GetUnit.activeSkill)
        {
            // Unhighlight player's squads if the player exited from Heal
            case (ActiveSkill.Heal):
                ToggleArmyToSelect(enemyArmyList, true);
                ToggleArmyToSelect(playerSquadsToHealList, false);
                break;
            // Unable highlighting all squads
            case (ActiveSkill.DamageAll):
                DamageAllFinish();
                ToggleArmyToSelect(enemyArmyList, true);
                break;
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

        currentPlayerSquad.Heal();
    }

    public void FinishHeal()
    {
        // Find an player squad that restores hp and updates its hp
        playerSquadToHeal = playerSquadsToHealList.Find(item => item.ID == BattleEvents.current.SelectedTargetID);

        int selectedSquadMaxHp = gameData.playerArmy.Find(item => item.SquadID == playerSquadToHeal.ID).SquadHp;
        // TODO: Tune this thing. It should be Unit's HP
        int healHP = (int) Mathf.Round(selectedSquadMaxHp * Unit.healPercent);
        playerSquadToHeal.IncreaseHP(healHP);

        // Not to overheal the Squad
        if (playerSquadToHeal.HP > selectedSquadMaxHp)
        {
            playerSquadToHeal.SetSquadHP(selectedSquadMaxHp);
        }

        state = BattleState.EnemyTurn;
        EnemyTurn();
    }

    // Damage All Active Skill
    void OnDamageAll()
    {
        if (!(state == BattleState.PlayerActiveSkill &&
              currentPlayerSquad.GetUnit.activeSkill == ActiveSkill.DamageAll))
            return;
        DamageAllFinish();
        currentPlayerSquad.DisableRegularHitButton();
        currentPlayerSquad.UsedActiveSkill = true;
        currentDealingDamage = Unit.fixedDamageAll;

        // Unhighlight current squad and put it at the end of the Queue
        currentPlayerSquad.HighlightSquad(false, 0);
        playerArmyQueue.Enqueue(currentPlayerSquad);

        damageAll = true;

        foreach (Squad playerSquad in playerArmyList.ToList())
        {
            playerSquad.ReceiveDamage(currentDealingDamage);
            state = NextBattleStateForEnemyTurn(playerSquad);
        }


        switch (state)
        {
            case BattleState.PlayerRegularTurn:
                foreach (Squad squad in enemyArmyList.ToList())
                {
                    squad.ReceiveDamage(currentDealingDamage);
                    state = NextBattleStateForPlayerTurn(squad);
                }

                // TODO: this is very unreliable
                StartCoroutine(WaitAfterDamageAll(2f));
                break;
            case BattleState.Lost:
                EndBattle();
                break;
        }
    }

    void ChangeStateAfterDamageAll()
    {
        Debug.Log("Checking the state");
        switch (state)
        {
            case BattleState.EnemyTurn:
                damageAll = false;
                EnemyTurn();
                break;
            case BattleState.Won:
                EndBattle();
                break;
        }
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

        enemyArmyQueue.Enqueue(currentEnemySquad);

        // TODO: AI or minimal strategy
        playerSquadToHit = enemyAI();

        currentEnemySquad.Opponent = playerSquadToHit;
        currentEnemySquad.Attack();
    }

    // This is game's AI
    // ¯\_(ツ)_/¯
    PlayerSquad enemyAI()
    {
        int holyRandom = Random.Range(0, 2);
        switch (holyRandom)
        {
            case 1:
                holyRandom = Random.Range(0, 2);
                switch (holyRandom)
                {
                    case 1:
                        PlayerSquad pss = playerArmyList[Random.Range(0, playerArmyList.Count)];
                        Debug.Log($"I chose at random: {pss}");
                        return pss;

                    // Hit the squad back
                    case 0:
                        Debug.Log($"I chose the last one to move: {currentPlayerSquad}");
                        return currentPlayerSquad;
                }

                break;

            // Hit the weakest squad
            case 0:
//                return playerArmyList.Min(item => item.HP);
                PlayerSquad ps = playerArmyList.Find(item => item.HP == playerArmyList.Min(squad => squad.HP));
                Debug.Log($"I chose the weakest: {ps}");
                return playerArmyList.Find(item => item.HP == playerArmyList.Min(squad => squad.HP));
        }

        return null;
    }

    public void FinishEnemyTurn()
    {
        state = NextBattleStateForEnemyTurn(playerSquadToHit);
        switch (state)
        {
            case BattleState.Lost:
                EndBattle();
                break;
            case BattleState.PlayerRegularTurn:
                PlayerTurn();
                break;
        }
    }

    BattleState NextBattleStateForPlayerTurn(Squad enemySquadToHit)
    {
        // Remove enemy squad from the list in case it's dead
        if (enemySquadToHit.IsDead)
        {
            enemyArmyList.RemoveAll(item => item.ID == enemySquadToHit.ID);
        }

        // The battle goes on if an enemy still has squads.
        // Otherwise, the player won this battle.
        if (enemyArmyList.Any())
        {
            return BattleState.EnemyTurn;
        }
        else
        {
            return BattleState.Won;
        }
    }


    BattleState NextBattleStateForEnemyTurn(Squad playerSquadToHit)
    {
        if (playerSquadToHit.IsDead)
        {
            gameData.playerArmy.RemoveAll(item => item.SquadID == playerSquadToHit.ID);
            playerSquadsToHealList.RemoveAll(item => item.ID == playerSquadToHit.ID);
            playerArmyList.RemoveAll(item => item.ID == playerSquadToHit.ID);
        }
        else
        {
            playerSquadsToHealList.Add(playerSquadToHit);
        }

        if (gameData.playerArmy.Any())
        {
            return BattleState.PlayerRegularTurn;
        }
        else
        {
            return BattleState.Lost;
        }
    }

    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            // If that was not the last knight, then
            // Destroy the defeated knight and Load Map
            // else Load WIN SCENE!

            gameData.DestroyCurrentEnemyArmy();
            if (gameData.enemyArmyList.Count != 0)
            {
                UpdateUnitSquadHP();
                FindObjectOfType<SceneLoader>().LoadMapScene();
            }
            else
            {
                print("YOU WON!");
                gameData.GameOver();
                FindObjectOfType<SceneLoader>().LoadGoodEndingScene();
            }
        }
        else if (state == BattleState.Lost)
        {
            print("YOU LOST");
            gameData.GameOver();
            FindObjectOfType<SceneLoader>().LoadBadEndingScene();
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

    void DamageAllPreset()
    {
        foreach (Squad squad in enemyArmyList)
        {
            squad.gameObject.GetComponentInChildren<SelectTarget>().DamageAllSetup();
        }

        foreach (PlayerSquad squad in playerArmyList)
        {
            squad.gameObject.GetComponentInChildren<SelectTarget>().DamageAllSetup();
        }
    }

    void DamageAllFinish()
    {
        foreach (Squad squad in enemyArmyList)
        {
            squad.gameObject.GetComponentInChildren<SelectTarget>().FinishDamageAll();
        }

        foreach (PlayerSquad squad in playerArmyList)
        {
            squad.gameObject.GetComponentInChildren<SelectTarget>().FinishDamageAll();
        }
    }

    IEnumerator WaitAfterDamageAll(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ChangeStateAfterDamageAll();
    }
}