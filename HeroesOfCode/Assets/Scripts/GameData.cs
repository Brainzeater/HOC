using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    // TODO: Up to 5 squads in total;
    [HideInInspector] public const int playerMaxArmySize = 5;

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

        public UnitNames unitName;

        public UnitSquad()
//        public UnitSquad(UnitNames unitName)
        {
//            this.unitName = unitName;
            SquadID = UnitSquadID;
            UnitSquadID++;
        }

        // And that is the last time we use squadUnits.
        public void CalculateSquadHp()
        {
            this.SquadHp = squadUnits * squadUnitPrefab.GetComponent<Squad>().GetUnit.hp;
        }
    }

    public GameObject skeleton;
    public GameObject knightHuman;
    public GameObject knightBlob;
    public GameObject shootingBlob;

    [Header("Player Army")] public List<UnitSquad> playerArmy;

    public int CurrentEnemyArmyIndex { get; set; }

    public List<GameObject> enemyArmyList;

    [HideInInspector] public Transform playerSpawnPosition;

//    public bool gameIsOver;


    void Awake()
    {
        // This object should be unique in a scene
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameData");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(this.gameObject);
        

        // TODO: This might be called to load custom army during development
        //        InitArmy(playerArmy);
    }

    // Called by ArmyManager after player army confirmation
    public void InitializePlayerArmy()
    {
        foreach (UnitSquad squad in playerArmy)
        {
            switch (squad.unitName)
            {
                case UnitNames.Skeleton:
                    squad.squadUnitPrefab = skeleton;
                    break;
                case UnitNames.KnightHuman:
                    squad.squadUnitPrefab = knightHuman;
                    break;
                case UnitNames.KnightBlob:
                    squad.squadUnitPrefab = knightBlob;
                    break;
                case UnitNames.ShootingBlob:
                    squad.squadUnitPrefab = shootingBlob;
                    break;
            }
        }

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

    public void GameOver()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}