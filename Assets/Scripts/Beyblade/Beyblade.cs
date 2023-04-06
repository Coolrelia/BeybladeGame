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
    [HideInInspector] public bool parrying = false;
    [HideInInspector] public bool phantomDashing = false;
    [HideInInspector] public bool dashing = false;
    [HideInInspector] public bool overdriving = false;
    [HideInInspector] public bool overDriven = false;
    [HideInInspector] public Vector2 storedDamage;

    [Header("Core Stats")]
    public float attack;
    public float defense;
    public float stamina;
    public int maxMeter;
    public int currentMeter;
    public int phantomDashCost;
    public int parryCost;

    [Header("Other Stats")]
    public float launchSpeed;
    public float currentStamina;
    public float lad;
    public float decayRate;
    public float parryTime;
    
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
    public bool launched = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        movement = GetComponent<BeybladeMovement>();
        abilites = GetComponent<BeybladeAbilities>();
        collision = GetComponent<BeybladeCollision>();
        rend = GetComponent<SpriteRenderer>();

        rend.sprite = mWheel.sprite;
        attack = mWheel.attack;
        defense = mWheel.defense + driver.defense;
        stamina = driver.stamina;
        lad = driver.lad;
        decayRate = driver.decayRate;
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

    private IEnumerator Launch()
    {
        ResetBeyblade();
        launched = true;
        overDriven = false;
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
