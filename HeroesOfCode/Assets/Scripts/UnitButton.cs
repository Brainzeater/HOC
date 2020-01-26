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

    public void SelectSkeleton()
    {
        FindObjectOfType<ArmyManager>().AddSquad(UnitNames.Skeleton);
    }

    public void SelectKnightHuman()
    {
        FindObjectOfType<ArmyManager>().AddSquad(UnitNames.KnightHuman);
    }
    public void SelectKnightBlob()
    {
        FindObjectOfType<ArmyManager>().AddSquad(UnitNames.KnightBlob);
    }
    public void SelectShootingBlob()
    {
        FindObjectOfType<ArmyManager>().AddSquad(UnitNames.ShootingBlob);
    }
}