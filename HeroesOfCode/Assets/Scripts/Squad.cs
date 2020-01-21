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

    public int ID { get; set; }

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

    void CalculateDealingDamage()
    {
        DealingDamage = numberOfUnits * unit.damage;
    }

    void CalculateSquadHP()
    {
        HP = numberOfUnits * unit.hp;
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

    // Shows the number of alive units in the squad.
    // Used to update the number of units after taking damage
    // and before loading any battle.
    void CalculateNumberOfUnitsFromHP()
    {
        int survivedUnits = HP / unit.hp;
        if (HP % unit.hp != 0)
        {
            survivedUnits += 1;
        }

        SetNumberOfUnits = survivedUnits;
    }

    // Used when the Squad is created from the Game Data
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
        gameObject.SetActive(false);
        IsDead = true;
    }

    public override string ToString()
    {
        return unit.name + " " + numberOfUnits + " " + HP + " " + ID;
    }
}