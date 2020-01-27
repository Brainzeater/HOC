using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadAnimationEvents : MonoBehaviour
{
    private Squad squad;

    void Awake()
    {
        squad = GetComponentInParent<Squad>();
    }

    public void DealDamage()
    {
        squad.DealDamage();
    }

    public void FinishMove()
    {
        squad.FinishMoveOfSquadWhoHitMe();
    }
}
