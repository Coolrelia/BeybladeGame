using UnityEngine;

[CreateAssetMenu(fileName = "Driver", menuName = "Beyblade/Driver")]
public class Driver : ScriptableObject
{
    public enum Material { Plastic, Metal, Rubber}

    [Header("Base Stats")]
    public string driverName;
    public float stamina;
    public float defense;
    public float speed;
    public float decayRate;
    public float lad; // Life After Death
    public Material material;
}
