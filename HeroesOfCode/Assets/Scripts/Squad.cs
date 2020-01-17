using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Squad : MonoBehaviour
{
    public Unit unit;
    public int numberOfUnits;

    public int DealingDamage { get; set; }

    public int HP { get; set; }
    public bool IsDead { get; set; }

    public TMP_Text numberOfUnitsText;

    private const string highlightTag = "HighlightSquad";
    public int ID { get; set; }

//    private bool usedActiveSkill;

    public virtual void Awake()
    {
        DisplayNumberOfUnits();
        IsDead = false;
        CalculateDealingDamage();
        CalculateSquadHP();
    }

    void DisplayNumberOfUnits()
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
            HP -= damage;
            if (HP <= 0)
            {
                // DIE
                Die();
            }
            else
            {
                CalculateNumberOfUnitsFromHP();
                CalculateDealingDamage();
                DisplayNumberOfUnits();
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

        SetNumberOfUnits = survivedUnits;
        print($"{numberOfUnits} units survived\n");
    }

    // Used when the Squad is created from the data
    public void SetSquadHP(int HP)
    {
        this.HP = HP;
        CalculateNumberOfUnitsFromHP();
        CalculateDealingDamage();
        DisplayNumberOfUnits();
    }

    public int SetNumberOfUnits
    {
        set { this.numberOfUnits = value; }
    }

    public Unit GetUnit
    {
        get { return unit; }
    }

    void Die()
    {
        Debug.Log(this + " destroyed");
        gameObject.SetActive(false);
        IsDead = true;
    }

    public void HighlightSquad(bool enabled)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.CompareTag(highlightTag))
            {
                child.gameObject.SetActive(enabled);
            }
            foreach (Transform grandChild in child.transform)
            {
                if (grandChild.CompareTag(highlightTag))
                {
                    grandChild.gameObject.SetActive(enabled);
                }
            }
        }
    }

    public override string ToString()
    {
        return unit.name + " " + numberOfUnits + " " + HP + " " + ID;
    }
}