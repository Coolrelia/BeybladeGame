using UnityEngine;

[CreateAssetMenu(fileName = "CWheel", menuName = "Beyblade/CWheel")]
public class ClearWheel : ScriptableObject
{
    [Header("Base Stats")]
    public Sprite sprite;
    public string cWheelName;
    public float attack;
    public float defense;

    [Header("Chance Bonuses")]
    public float bonusAttack;
    public float bonusDefense;
    public float bonusAttackChance;
    public float bonusDefenseChance;

    [Header("Special Move")]
    public Ability ability;
}
