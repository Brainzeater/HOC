using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public Unit unit;
    public int numOfUnits;

    private int DealingDamage { get; set; }
//    private int ReceivedDamage { get; set; }
    private int HP { get; set; }

    public TMP_Text numOfUnitsText;
    public TMP_Text unitHPText;
    public TMP_Text unitDamageText;

//    private bool usedActiveSkill;

    void Start()
    {
        numOfUnitsText.text = numOfUnits.ToString();
        unitHPText.text = unit.hp.ToString();
        unitDamageText.text = unit.damage.ToString();
    }

    void CalculateDealingDamage()
    {
        DealingDamage = numOfUnits * unit.damage;
    }

    void ReceiveDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            int killedUnits = damage / unit.hp;
            numOfUnits -= killedUnits;
        }
    }

    void CalculateSquadHP()
    {
        HP = numOfUnits * unit.hp;
    }

    void CalculateNumberOfUnitsFromHP()
    {

    }

    void SetSquadHP(int HP)
    {

    }

    void Die()
    {
        Debug.Log("Squad Destroyed");
    }
    
}
