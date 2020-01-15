using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public Unit unit;
    public int numberOfUnits;

    private int DealingDamage { get; set; }
//    private int ReceivedDamage { get; set; }
    private int HP { get; set; }

    public TMP_Text numberOfUnitsText;

//    private bool usedActiveSkill;

    public virtual void Start()
    {
        numberOfUnitsText.text = numberOfUnits.ToString();
    }

    void CalculateDealingDamage()
    {
        DealingDamage = numberOfUnits * unit.damage;
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
            numberOfUnits -= killedUnits;
        }
    }

    void CalculateSquadHP()
    {
        HP = numberOfUnits * unit.hp;
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
