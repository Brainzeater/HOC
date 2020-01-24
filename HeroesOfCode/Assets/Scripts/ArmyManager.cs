using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public enum UnitNames
{
    Skeleton,
    KnightHuman,
    KnightBlob,
    ShootingBlob
}

public class ArmyManager : MonoBehaviour
{
    public GameObject skeleton;
    public GameObject knightHuman;
    public GameObject knightBlob;
    public GameObject shootingBlob;

    public GameObject slots;
    private Transform[] slotsArray;

//    private List<GameData.UnitSquad> squads;
    private List<GameObject> squadPrefabs;

    void Start()
    {
        slotsArray = slots.GetComponentsInChildren<Transform>();
//        squads = new List<GameData.UnitSquad>();
        squadPrefabs = new List<GameObject>();
    }

    public void UpdateSlots()
    {
        int i = 1;
        foreach (GameObject prefab in squadPrefabs)
        {
            prefab.transform.position = slotsArray[i].position;
            i++;
        }
    }

    public void AddSquad(UnitNames name)
    {
        if (squadPrefabs.Count < GameData.playerMaxArmySize)
        {
            switch (name)
            {
                case UnitNames.Skeleton:
                    AddNewSquadGameObject(skeleton);
                    break;
                case UnitNames.KnightHuman:
                    print("HUMAN");
                    AddNewSquadGameObject(knightHuman);
                    break;
                case UnitNames.KnightBlob:
                    AddNewSquadGameObject(knightBlob);
                    break;
                case UnitNames.ShootingBlob:
                    AddNewSquadGameObject(shootingBlob);
                    break;
            }
        }
    }

    public void RemoveSquad(int ID)
    {
        print($"Removing ID: {ID}");

        foreach (GameObject prefab in squadPrefabs)
        {
            if (prefab.GetComponentInChildren<DeleteSquadButton>().ID == ID)
                Destroy(prefab);
        }

        int a = squadPrefabs.RemoveAll(squad => squad.GetComponentInChildren<DeleteSquadButton>().ID == ID);
        print($"removed = {a}");
        UpdateSlots();
    }

    void AddNewSquadGameObject(GameObject squadType)
    {
        GameData.UnitSquad newSquad = new GameData.UnitSquad();
        GameObject squadGameObject = Instantiate(squadType, slotsArray[squadPrefabs.Count + 1]);
        newSquad.squadUnitPrefab = squadGameObject;
//        newSquad.squadUnits = squadGameObject.GetComponent<Squad>().numberOfUnits;
        squadPrefabs.Add(squadGameObject);
        squadGameObject.GetComponentInChildren<DeleteSquadButton>().ID = newSquad.SquadID;
        print($"New ID = {newSquad.SquadID}");
    }
}