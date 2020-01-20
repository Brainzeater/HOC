using UnityEngine;

public enum ActiveSkill
{
    Heal, IncreasedDamage, DamageAll
}

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public new string name;
    public int hp;
    public int damage;
    public bool hasActiveSkill;
    public ActiveSkill activeSkill;

}