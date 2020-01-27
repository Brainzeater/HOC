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

    public GameObject armyMenu;
    public GameObject mainMenu;

    private List<GameObject> squadPrefabs;
    private List<GameData.UnitSquad> army;

    void Start()
    {
        slotsArray = slots.GetComponentsInChildren<Transform>();
        squadPrefabs = new List<GameObject>();
        army = new List<GameData.UnitSquad>();
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

    public void AddSquad(UnitNames unitName)
    {
        if (squadPrefabs.Count < GameData.playerMaxArmySize)
        {
            switch (unitName)
            {
                case UnitNames.Skeleton:
                    AddNewSquadGameObject(skeleton, unitName);
                    break;
                case UnitNames.KnightHuman:
                    AddNewSquadGameObject(knightHuman, unitName);
                    break;
                case UnitNames.KnightBlob:
                    AddNewSquadGameObject(knightBlob, unitName);
                    break;
                case UnitNames.ShootingBlob:
                    AddNewSquadGameObject(shootingBlob, unitName);
                    break;
            }
        }
    }


    void AddNewSquadGameObject(GameObject squadType, UnitNames unitName)
    {
//        GameData.UnitSquad newSquad = new GameData.UnitSquad(unitName);
        GameData.UnitSquad newSquad = new GameData.UnitSquad()
        {
            unitName = unitName
        };

        GameObject squadGameObject = Instantiate(squadType, slotsArray[squadPrefabs.Count + 1]);
//        newSquad.squadUnitPrefab = squadGameObject;
//        newSquad.squadUnits = squadGameObject.GetComponent<Squad>().numberOfUnits;
        squadPrefabs.Add(squadGameObject);
        squadGameObject.GetComponentInChildren<DeleteSquadButton>().ID = newSquad.SquadID;

        army.Add(newSquad);
    }

    public void RemoveSquad(int ID)
    {
        print($"Removing ID: {ID}");

        foreach (GameObject prefab in squadPrefabs)
        {
            if (prefab.GetComponentInChildren<DeleteSquadButton>().ID == ID)
                Destroy(prefab);
        }

        army.RemoveAll(squad => squad.SquadID == ID);
        int a = squadPrefabs.RemoveAll(squad => squad.GetComponentInChildren<DeleteSquadButton>().ID == ID);
        print($"removed = {a}");
        UpdateSlots();
    }

    public void SetDefaultArmy()
    {
        foreach (GameObject prefab in squadPrefabs)
        {
            Destroy(prefab);
        }

        squadPrefabs.Clear();
        army.Clear();

        AddNewSquadGameObject(skeleton, UnitNames.Skeleton);
        AddNewSquadGameObject(knightHuman, UnitNames.KnightHuman);
        AddNewSquadGameObject(knightBlob, UnitNames.KnightBlob);
        AddNewSquadGameObject(shootingBlob, UnitNames.ShootingBlob);
    }

    public void EnableArmyMenu()
    {
        mainMenu.SetActive(false);
        armyMenu.SetActive(true);
    }

    public void DisableArmyMenu()
    {
        armyMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ConfirmArmy()
    {
        if (army.Count > 0)
        {
            foreach (GameData.UnitSquad squad in army)
            {
                GameObject prefab = squadPrefabs.Find(squadPrefab =>
                    squadPrefab.GetComponentInChildren<DeleteSquadButton>().ID == squad.SquadID);
                TMP_InputField inputField = prefab.GetComponentInChildren<TMP_InputField>();
                string numberOfUnitsString;

                // Since InputFieldModifier validates input, inputField.text can be empty
                // only when user doesn't change it.
                if (string.IsNullOrEmpty(inputField.text))
                {
                    numberOfUnitsString = inputField.placeholder.GetComponent<TextMeshProUGUI>().text;
                }
                else
                {
                    numberOfUnitsString = inputField.text;
                }

                print(squad.unitName + " " + numberOfUnitsString);
                squad.squadUnits = int.Parse(numberOfUnitsString);
            }

            FindObjectOfType<GameData>().playerArmy = army;
            FindObjectOfType<GameData>().InitializePlayerArmy();
            FindObjectOfType<SceneLoader>().LoadMapScene();
        }
        else
        {
            // TODO: Message for user
            print("At least one squad must be chosen");
        }
    }
}