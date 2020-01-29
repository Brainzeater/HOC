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

    public void FinishHeal()
    {
        FindObjectOfType<BattleSystem>().FinishHeal();
    }

    // Sound Effects
    public void PlaySword()
    {
        FindObjectOfType<AudioManager>().Play("Sword");
    }

    public void PlayHumanHit()
    {
        FindObjectOfType<AudioManager>().Play("HumanHit");
    }

    public void PlayHumanDeath()
    {
        FindObjectOfType<AudioManager>().Play("HumanDeath");
    }

    public void PlayHalberd()
    {
        FindObjectOfType<AudioManager>().Play("Halberd");
    }

    public void PlayBoneHit()
    {
        FindObjectOfType<AudioManager>().Play("BoneHit");
    }

    public void PlaySkeletonDeath()
    {
        FindObjectOfType<AudioManager>().Play("SkeletonDeath");
    }

    public void PlayCrit()
    {
        FindObjectOfType<AudioManager>().Play("Crit");
    }

    public void PlayBlobSlap()
    {
        FindObjectOfType<AudioManager>().Play("BlobSlap");
    }

    public void PlayBlobHit()
    {
        FindObjectOfType<AudioManager>().Play("BlobHit");
    }

    public void PlayBlobDeath()
    {
        FindObjectOfType<AudioManager>().Play("BlobDeath");
    }

    public void PlayShooting()
    {
        FindObjectOfType<AudioManager>().Play("Shooting");
    }

    public void PlayShootingHit()
    {
        FindObjectOfType<AudioManager>().Play("ShootingHit");
    }

    public void PlayShootingDeath()
    {
        FindObjectOfType<AudioManager>().Play("ShootingDeath");
    }

    public void PlayHeal()
    {
        FindObjectOfType<AudioManager>().Play("Heal");
    }
}