using UnityEngine;

[CreateAssetMenu(fileName = "Track", menuName = "Beyblade/Track")]
public class Track : ScriptableObject
{
    [Header("Base Stats")]
    public string trackName;
    public float attack;
    public float defense;
    public float stamina;
    public int height;

    [Header("Chance Bonuses")]
    public float bonusAttack;
    public float bonusDefense;
    public float critHitChance;
    public float critDefChance;
}
