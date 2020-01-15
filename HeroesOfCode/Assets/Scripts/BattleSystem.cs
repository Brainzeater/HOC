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
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattlePosition;
    public Transform enemyBattlePosition;

    private PlayerSquad player;
    private EnemySquad enemy;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        BattleEvents.current.OnTargetSelected += OnTargetChosen;
        state = BattleState.START;
//        StartCoroutine(SetupBattle());
        SetupBattle();
    }

//    IEnumerator SetupBattle()
    void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattlePosition);
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattlePosition);
        player = playerGO.GetComponent<PlayerSquad>();
        enemy = enemyGO.GetComponent<EnemySquad>();


//        yield return new WaitForSeconds(1f);
        print(player.DealingDamage);
        print(enemy.DealingDamage);
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
        }
        else if(state == BattleState.LOST)
        {
            // Load GameOver Scene
        }
    }
}