using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // TODO: Up to 5 squads in total;
    private const int playerMaxArmySize = 5;

    [Serializable]
    public class UnitSquad
    {
        private static int UnitSquadID = 0;

        [Header("Unit Squad Prefab")] public GameObject squadUnitPrefab;

        [Header("Number of Units in the Squad")]
        public int squadUnits;

        [HideInInspector] public int SquadHp { get; set; }
        [HideInInspector] public int SquadID { get; set; }
        public UnitSquad()
        {
            SquadID = UnitSquadID;
            UnitSquadID++;
        }
        public void CalculateSquadHp()
        {
            this.SquadHp = squadUnits * squadUnitPrefab.GetComponent<Squad>().GetUnit.hp;
        }

    }

    [Header("Player Army")] public List<UnitSquad> playerArmy;

    // and methods for them
    public int EnemyArmiesLeft { get; set; }
    private const int enemyArmiesMaxCount = 3;

    [Header("Enemy Army 1")] public List<UnitSquad> enemyArmy1;
    [Header("Enemy Army 2")] public List<UnitSquad> enemyArmy2;
    [Header("Enemy Army 3")] public List<UnitSquad> enemyArmy3;

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
        InitArmy(enemyArmy1);
    }

    void InitArmy(List<UnitSquad> army)
    {
        if (army.Any())
            foreach (UnitSquad squad in army)
            {
                squad.CalculateSquadHp();
            }
    }
}