using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // To extend the list of available units, you need to add it here
//    [Header("Enter the number of units in each squad")]
//    public List<int> goblinSquads;
//    public List<int> skeltonSquads;
//    public List<int> shootingBlobSquads;
//    public List<int> knightBlobSquads;

    //    public Unit goblin;
    //    public Unit skeleton;
    //    public Unit shootingBlob;
    //    public Unit knightBlob;

//    [Header("Prefab for each unit")] public GameObject playerGoblin;
//    public GameObject playerSkeleton;
//    public GameObject playerShootingBlob;
//    public GameObject playerKnightBlob;


    // TODO: Up to 5 squads in total;
    [Serializable]
    public class UnitSquads
    {
        [Header("Unit Squad Prefab")] public GameObject unitPrefab;

        [Header("Number of Units in the Squad")]
        public List<int> unitsInSquadList;

        [HideInInspector] public List<int> HpOfSquadList { get; set; }
    }

    [Header("Player Army")] public List<UnitSquads> playerArmy;

    // and methods for them
    public int EnemyArmiesLeft { get; set; }
    private const int enemyArmies = 3;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameData");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        EnemyArmiesLeft = enemyArmies;

        InitPlayerArmy();

//        for (int i = 0; i < goblinSquads.Count; i++)
//        {
//            int goblin
//            goblinSquadHPs.Add(); playerGoblin.GetComponent<Squad>().GetUnit.hp;
//        }
//        for (int i = 0; i < skeltons.Count; i++)
//        {
//            skeltons[i] *= skeleton.hp;
//        }
//        for (int i = 0; i < shootingBlobs.Count; i++)
//        {
//            shootingBlobs[i] *= shootingBlob.hp;
//        }
//        for (int i = 0; i < knightBlobs.Count; i++)
//        {
//            knightBlobs[i] *= knightBlob.hp;
//        }
    }

    void InitPlayerArmy()
    {
        // TODO: Put in UnitSquad's constructor
        if (playerArmy.Any())
            for (int i = 0; i < playerArmy.Count; i++)
            {
                UnitSquads currentUnitSquad = playerArmy[i];
                if (currentUnitSquad.unitsInSquadList.Any())
                    foreach (int unitsNumber in currentUnitSquad.unitsInSquadList)
                    {
                        currentUnitSquad.HpOfSquadList = new List<int>();
                        currentUnitSquad.HpOfSquadList.Add(currentUnitSquad.unitPrefab.GetComponent<Squad>().unit.hp *
                                                           unitsNumber);
                    }
            }
        
    }
}