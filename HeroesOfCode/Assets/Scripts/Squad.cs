using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public Unit unit;
    public int numberOfUnits;

    public int DealingDamage { get; set; }

//    private int ReceivedDamage { get; set; }
    private int HP { get; set; }
    public bool IsDead { get; set; }

    public TMP_Text numberOfUnitsText;

//    private bool usedActiveSkill;

    public virtual void Awake()
    {
        SetNumberOfUnits();
        IsDead = false;
        CalculateDealingDamage();
        CalculateSquadHP();
    }

    void SetNumberOfUnits()
    {
        numberOfUnitsText.text = numberOfUnits.ToString();
    }

    void CalculateSquadHP()
    {
        HP = numberOfUnits * unit.hp;
    }

    void CalculateDealingDamage()
    {
        DealingDamage = numberOfUnits * unit.damage;
    }

    public void ReceiveDamage(int damage)
    {
        if (!IsDead)
        {
            print($"Received D is: {damage}\n");
            HP -= damage;
            print($"Current HP is: {HP}\n");
            if (HP <= 0)
            {
                // DIE
                Die();
            }
            else
            {
                CalculateNumberOfUnitsFromHP();
                CalculateDealingDamage();
                SetNumberOfUnits();
            }
        }
    }

    void CalculateNumberOfUnitsFromHP()
    {
        int survivedUnits = HP / unit.hp;
        if (HP % unit.hp != 0)
        {
            survivedUnits += 1;
        }

        numberOfUnits = survivedUnits;
        print($"{numberOfUnits} units survived\n");
    }

    void SetSquadHP(int HP)
    {
    }

    void Die()
    {
        Debug.Log("Squad Destroyed");
        IsDead = true;
    }
}