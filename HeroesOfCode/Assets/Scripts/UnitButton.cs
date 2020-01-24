using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public Unit unit;
    public Image highlight;

    public void Highlight()
    {
        if (!highlight.enabled)
            highlight.enabled = true;
    }

    public void Unhighlight()
    {
        if (highlight.enabled)
            highlight.enabled = false;
    }

//    public void SelectUnit()
//    {
//        print(unit.name);
//        FindObjectOfType<ArmyManager>().UpdateSlots(); 
//    }

    public void SelectSkeleton()
    {
        FindObjectOfType<ArmyManager>().AddSquad(UnitNames.Skeleton);
    }
}