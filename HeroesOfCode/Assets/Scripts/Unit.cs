using UnityEngine;

public enum ActiveSkill
{
    Heal,
    IncreasedDamage,
    DamageAll
}

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public new string name;
    public int hp;
    public int damage;
    public bool hasActiveSkill;
    public ActiveSkill activeSkill;

    [HideInInspector] public const float healPercent = 0.2f;
    [HideInInspector] public const int increasedConstant = 10;
    [HideInInspector] public const int increasedCoefficient = 2;
    [HideInInspector] public const int fixedDamageAll = 20;
}