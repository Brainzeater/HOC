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

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameData");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        EnemyArmiesLeft = enemyArmiesMaxCount;

        InitPlayerArmy();
    }

    void InitPlayerArmy()
    {
        if (playerArmy.Any())
            foreach (UnitSquad squad in playerArmy)
            {
                squad.CalculateSquadHp();
            }
    }
}