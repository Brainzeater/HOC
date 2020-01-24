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

    private List<GameData.UnitSquad> squads;

    void Start()
    {
        slotsArray = slots.GetComponentsInChildren<Transform>();
        squads = new List<GameData.UnitSquad>();

    }

    public void UpdateSlots()
    {
        Transform currentTransform = slotsArray[1];
        currentTransform.transform.localScale = Vector3.one;
        Instantiate(skeleton, currentTransform);

    }

    public void AddSquad(UnitNames name)
    {
        if (squads.Count < GameData.playerMaxArmySize)
        {
            switch (name)
            {
                case UnitNames.Skeleton:
                    GameData.UnitSquad newSquad = new GameData.UnitSquad();
                    newSquad.squadUnitPrefab = skeleton;
//                    newSquad.squadUnits = skeleton.GetComponent<Squad>().numberOfUnits;

                    squads.Add(newSquad);
//                    skeleton.GetComponentInChildren<TextMeshProUGUI>().text = "BOO";

                    Transform currentTransform = slotsArray[squads.Count];
                    currentTransform.transform.localScale = Vector3.one;
                    Instantiate(skeleton, currentTransform);
                    break;
            }
        }
    }

}
