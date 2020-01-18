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
        Debug.Log(this + " destroyed");
        gameObject.SetActive(false);
        IsDead = true;
    }

    // Activates Squad's background highlight and "Active Skill" button
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
                // TODO: Highlight only in case the active skill wasn't used
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