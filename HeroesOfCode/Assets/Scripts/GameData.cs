using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // TODO: Up to 5 squads in total;
    private const int playerMaxArmySize = 5;

    // Holds the Data about each squad in the army throughout the whole Game
    [Serializable]
    public class UnitSquad
    {
        // ID is used to update the data of Squads during the Battle and after it
        private static int UnitSquadID = 0;

        [Header("Unit Squad Prefab")] public GameObject squadUnitPrefab;

        // This field holds the info about the initial number of units in a squad.
        // However, only SquadHp matters for the Battle System as it
        // calculates the number of units according to squad's hp.
        [Header("Number of Units in the Squad")]
        public int squadUnits;

        [HideInInspector] public int SquadHp { get; set; }
        [HideInInspector] public int SquadID { get; set; }

        public UnitSquad()
        {
            SquadID = UnitSquadID;
            UnitSquadID++;
        }

        // And that is the last time we use squadUnits.
        public void CalculateSquadHp()
        {
            this.SquadHp = squadUnits * squadUnitPrefab.GetComponent<Squad>().GetUnit.hp;
        }
    }

    [Header("Player Army")] public List<UnitSquad> playerArmy;

    private const int enemyArmiesMaxCount = 3;

    // and methods for them
    public int EnemyArmiesLeft { get; set; }

    public int CurrentEnemyArmyIndex { get; set; }

//    [Header("Enemy Army 1")] public List<UnitSquad> enemyArmy1;
//    [Header("Enemy Army 2")] public List<UnitSquad> enemyArmy2;
//    [Header("Enemy Army 3")] public List<UnitSquad> enemyArmy3;

    public List<GameObject> enemyArmyList;

    public Transform playerSpawnPosition;

    void Awake()
    {
        // This object should be unique in a scene
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameData");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        EnemyArmiesLeft = enemyArmiesMaxCount;

        InitArmy(playerArmy);
    }

    public void InitializeBattleArmies()
    {
        List<UnitSquad> currentEnemy = GetCurrentEnemyArmy().army;
        InitArmy(currentEnemy);
        print(CurrentEnemyArmyIndex);
    }

    void InitArmy(List<UnitSquad> army)
    {
        if (army.Any())
            foreach (UnitSquad squad in army)
            {
                squad.CalculateSquadHp();
            }
    }

    public EnemyArmy GetCurrentEnemyArmy()
    {
        return enemyArmyList
            .Find(item => item.gameObject.GetComponent<EnemyArmy>().armyIndex == CurrentEnemyArmyIndex)
            .gameObject.GetComponent<EnemyArmy>();
    }

    public void DestroyCurrentEnemyArmy()
    {
        playerSpawnPosition = GetCurrentEnemyArmy().spawnPosition;
        enemyArmyList
            .RemoveAll(item => item.GetComponent<EnemyArmy>().armyIndex == CurrentEnemyArmyIndex);
    }
}