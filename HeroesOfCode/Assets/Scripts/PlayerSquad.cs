using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSquad : Squad
{
    public TMP_Text unitHPText;
    public TMP_Text unitDamageText;

    //    private bool usedActiveSkill;
    
    public override void Awake()
    {
        base.Awake();
        unitHPText.text = unit.hp.ToString();
        unitDamageText.text = unit.damage.ToString();
    }
}
