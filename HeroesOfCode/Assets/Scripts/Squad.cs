using System.Collections;
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

    [HideInInspector] public Animator animator;

    public Squad Opponent { get; set; }

    public virtual void Awake()
    {
        DisplayNumberOfUnits();
        IsDead = false;
        CalculateDealingDamage();
        CalculateSquadHP();
        animator = GetComponentInChildren<Animator>();
        // Makes animations start from a random frame
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
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
                TakeHit();
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

    void TakeHit()
    {
        Debug.Log($"{this} is taking hit");
        CalculateNumberOfUnitsFromHP();
        CalculateDealingDamage();
        DisplayNumberOfUnits();
        animator.SetTrigger("Hit");
    }

    void Die()
    {
        numberOfUnitsText.SetText("0");
//        StartCoroutine(DeathAnimation());
//        gameObject.SetActive(false);

        animator.SetBool("IsDead", true);
        IsDead = true;
    }

    public override string ToString()
    {
        return unit.name + " " + numberOfUnits + " " + HP + " " + ID;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void DealDamage()
    {
        // Play attack animation
        Opponent.ReceiveDamage(this.DealingDamage);
        //
        CalculateDealingDamage();
        // Recalculate Dealing Damage
    }

    public virtual void FinishMoveOfSquadWhoHitMe()
    {
        Debug.Log($"{this} finishes player's move");
        FindObjectOfType<BattleSystem>().FinishPlayerTurn();
    }
//
//    public virtual void PlayAttackAnimation(Squad playerSquadToHit)
//    {
//        StartCoroutine(AttackAnimation(playerSquadToHit));
//    }
//
//    IEnumerator AttackAnimation(Squad playerSquadToHit)
//    {
//        animator.SetTrigger("Attack");
//
//
//        //Wait until Animator is done playing
//        print(animator.GetCurrentAnimatorStateInfo(0).length);
//        print(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
//        // TODO: Why multiplied by 2?
////        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 2);
//        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
//
//        //        +anim.GetCurrentAnimatorStateInfo(0).normalizedTime
//
//        playerSquadToHit.animator.SetTrigger("Hit");
//
//        yield return new WaitForSeconds(playerSquadToHit.animator.GetCurrentAnimatorStateInfo(0).length);
//
//        FindObjectOfType<BattleSystem>().FinishEnemyTurn(playerSquadToHit);
//    }
//
////    public void PlayDeathAnimation()
////    {
////        StartCoroutine(DeathAnimation());
////    }
//
//    IEnumerator DeathAnimation()
//    {
//        animator.SetBool("IsDead", true);
//
//
//        //Wait until Animator is done playing
//        print(animator.GetCurrentAnimatorStateInfo(0).length);
//        print(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
//        // TODO: Why multiplied by 2?
//        //        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 2);
//        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
//
//        //        +anim.GetCurrentAnimatorStateInfo(0).normalizedTime
//    }
}