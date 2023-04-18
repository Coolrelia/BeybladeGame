﻿using UnityEngine;

[CreateAssetMenu(fileName = "MWheel", menuName = "Beyblade/MWheel")]
public class MetalWheel : ScriptableObject
{
    [Header("Base Stats")]
    public Sprite sprite;
    public string mWheelName;
    public float attack;
    public float defense;
    public int id;

    [Header("Attack Type")]
    public float smashAttack;
    public float upperAttack;

    [Header("Chance Bonuses")]
    public float bonusAttack;
    public float bonusDefense;
    public float critHitChance;
    public float critDefChance;
}
