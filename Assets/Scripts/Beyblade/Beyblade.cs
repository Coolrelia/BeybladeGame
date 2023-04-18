using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beyblade : MonoBehaviour
{
    private SpriteRenderer rend;

    public GameObject sparkEffect;
    public GameObject overdriveEffect;
    public GameObject opponent;

    [HideInInspector] public BeybladeMovement movement;
    [HideInInspector] public BeybladeAbilities abilites;
    [HideInInspector] public BeybladeCollision collision;

    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool guarding = false;
    [HideInInspector] public bool reflecting = false;
    [HideInInspector] public bool phantomDashing = false;
    [HideInInspector] public bool dashing = false;
    [HideInInspector] public bool frozen = false;
    [HideInInspector] public bool dangerMode = false;

    [Header("Core Stats")]
    public float attack;
    public float defense;
    public float stamina;
    public float height;
    public float critHitChance;
    public float critDefChance;
    public float smashAttackChance;
    public float upperAttackChance;

    [Header("Meter")]
    public int currentMeter;
    public int maxMeter;
    public int phantomDashCost;
    public int reflectCost;

    [Header("Other Stats")]
    public float launchSpeed;
    public float currentStamina;
    public float lad;
    public float decayRate;
    
    [Header("Bonus Stats")]
    public float bonusAttack = 0;
    public float bonusDefense = 0;

    public MetalWheel mWheel;
    public ClearWheel cWheel;
    public Track track;
    public Driver driver;
    public GameObject ability;

    private bool ladActivated = false;
    private bool opponentFound = false;
    private bool roundReset = false;
    private bool heightBonusesSet = false;
    public bool launched = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        movement = GetComponent<BeybladeMovement>();
        abilites = GetComponent<BeybladeAbilities>();
        collision = GetComponent<BeybladeCollision>();
        rend = GetComponent<SpriteRenderer>();

        SetStats();

        if (!cWheel.ability) return;
        GameObject abl = Instantiate(cWheel.ability.gameObject, transform);
        ability = abl;
        ability.GetComponent<Ability>().beyblade = GetComponent<Beyblade>();
    }
    private void Start()
    {
        currentMeter = 0;
    }
    private void Update()
    {
        FindOpponent();
        SleepOut();
        ApplyLAD();
    }

    public void LaunchBeyblade()
    {
        roundReset = false;
        StartCoroutine(Launch());
        StartCoroutine(Spin());
    }
    private void FindOpponent()
    {
        if (opponentFound) return;
        
        if(tag == "Player"){
            opponent = FindObjectOfType<EnemyAI>().gameObject;
        }
        else{
            opponent = GameObject.FindGameObjectWithTag("Player");
        }

        opponentFound = true;
    }
    private void SetStats()
    {
        // Core Stats
        rend.sprite = mWheel.sprite;
        attack = mWheel.attack;
        defense = mWheel.defense + driver.defense;
        stamina = driver.stamina;
        lad = driver.lad;
        decayRate = driver.decayRate;
        height = track.height;
        
        // Critical Stats
        critHitChance = mWheel.critHitChance + track.critHitChance;
        critDefChance = mWheel.critDefChance + track.critDefChance;
        bonusAttack = mWheel.bonusAttack + track.bonusAttack;
        bonusDefense = mWheel.bonusDefense + track.bonusDefense;

        // Attack Variant Stats
        smashAttackChance = mWheel.smashAttack;
        upperAttackChance = mWheel.upperAttack;
    }
    private void SetHeightBonuses()
    {
        // if we are shorter than our opponent, increase upper attack
        // the smaller the height difference, the higher the smash attack increase
        if (!opponent) return;
        if (heightBonusesSet) return;

        float heightDifference = height - opponent.GetComponent<Beyblade>().height;
        if (heightDifference < 0) upperAttackChance += Mathf.Abs(heightDifference / 2);

        switch (Mathf.Abs(heightDifference))
        {
            case 0:
                smashAttackChance += 20;
                break;
            case 5:
                smashAttackChance += 15;
                break;
            case 15:
                smashAttackChance += 10;
                break;
            case 20: 
                smashAttackChance += 10;
                break;
            case 25:
                smashAttackChance += 5;
                break;
            case 30: 
                smashAttackChance += 5;
                break;
            case 40:
                smashAttackChance += 5;
                break;
        }

        heightBonusesSet = true;
        print("bonuses set");
    }

    private IEnumerator Launch()
    {
        SetHeightBonuses();
        ResetBeyblade();
        launched = true;
        dangerMode = false;
        frozen = false;
        currentStamina = launchSpeed + driver.speed;
        while (currentStamina > 0)
        {
            currentStamina -= decayRate;
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator Spin()
    {
        float spinSpeed = currentStamina * 10;
        if(spinSpeed > 2000){
            spinSpeed = 2000;
        }

        while(currentStamina > 0)
        {
            transform.Rotate(Vector3.back * spinSpeed * Time.deltaTime);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }
    
    private void ApplyLAD()
    {
        if (launched == false)return ;
        if (ladActivated) return;
        float speedThreshold = launchSpeed * .25f;
        if(currentStamina <= speedThreshold)
        {
            currentStamina += lad;
            ladActivated = true;
            print("LAD activated");
        }
    }
    private void SleepOut()
    {
        if (currentStamina <= 0 && launched && !roundReset)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            FindObjectOfType<RoundStart>().ResetRound();
            roundReset = true;
        }
    }

    private void ResetBeyblade()
    {
        currentMeter = 0;

        attack = mWheel.attack;
        defense = mWheel.defense + driver.defense;
        stamina = driver.stamina;
        lad = driver.lad;
        decayRate = driver.decayRate;

        bonusAttack = 0;
        bonusDefense = 0;
    }
}
