using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BeybladeCollision : MonoBehaviour
{
    private Beyblade beyblade;

    private bool criticalHit = false;
    private bool criticalDefend = false;
    private bool phantomDashCancelled = false;
    public bool colliding = false;
    private Vector2 storedDamage;
    private bool dangerTime = false;

    private void Awake()
    {
        beyblade = GetComponent<Beyblade>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Blast Zone")
        {
            beyblade.decayRate = 30;
            Invoke("StopSpinning", 0.1f);
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Beyblade>() == null) return;

        if(gameObject.tag == "Player"){
            UIEvents.current.Hit(beyblade);
        }

        colliding = true;

        // Danger Time Check
        int dangerChance = 50;
        int randomNumber = Random.Range(1, 100);
        if(randomNumber <= dangerChance)
        {
            StartDangerTime();
        }

        Beyblade opponent = col.gameObject.GetComponent<Beyblade>();
        Vector3 dir = (col.gameObject.transform.position - gameObject.transform.position).normalized;

        // if we're phantom dashing, cancel it
        if (beyblade.phantomDashing && beyblade.tag == "Player"){
            beyblade.abilites.CancelPhantomDash();
            phantomDashCancelled = true;
            // stop sound effect
        }

        //Knockback(-dir, opponent);
        NewKnockback(-dir, opponent.GetComponent<BeybladeCollision>());
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Beyblade>() == null) return;
        Invoke("NotColliding", 0.01f);
    }

    /*
    private void Knockback(Vector3 direction, Beyblade opponent)
    {
        // SPARK EFFECT
        GameObject spark = Instantiate(beyblade.sparkEffect, new Vector2(0, 0), Quaternion.identity);
        spark.transform.position = beyblade.transform.position;
        if (criticalHit)
        {
            var particleSystem = spark.GetComponent<ParticleSystem>();
            particleSystem.Stop();
            var color = particleSystem.colorOverLifetime;
            color.enabled = true;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
            color.color = grad;
            particleSystem.Play();
        }
        
        // SOUND EFFECTS
        if (!criticalHit){
            GameEvents.current.Hit();
        }

        if (beyblade.guarding || opponent.guarding){
            GameEvents.current.Guard();
        }        

        // DAMAGE CALCULATION
        float damage = 0;
        ChanceBonuses();
        damage = opponent.attack + ((opponent.currentStamina / 4) * Time.timeScale) - (beyblade.defense + beyblade.stamina);        

        // KNOCKBACK CALCULATION
        Vector2 knockback = Vector2.zero;                
        if (criticalDefend){
            knockback = (direction * (damage * 0.75f));
            criticalDefend = false;
        }
        else{
            knockback = (direction * damage / 2);
        }

        if (opponent.phantomDashing){
            knockback = direction * 20;
        }

        if (opponent.parrying){
            beyblade.storedDamage += knockback;
            knockback = Vector2.zero;
        }

        if (beyblade.parrying){
            knockback = direction * 10;
        }        

        // APPLY DAMAGE
        if (damage <= 0) return;
        
        if (beyblade.parrying){
            beyblade.currentStamina -= damage * (0.25f * Time.timeScale);            
        }
        else if (opponent.parrying){
            beyblade.currentStamina -= damage * (0.01f * Time.timeScale);
        }
        else if (opponent.phantomDashing){
            beyblade.currentStamina -= damage * (0.1f * Time.timeScale);
        }
        else if (phantomDashCancelled){
            beyblade.currentStamina -= damage / 2;
            phantomDashCancelled = false;
        }
        else{
            beyblade.currentStamina -= damage * 0.3f;
        }
        if (criticalHit){
            knockback = (direction * 10);
        }
        ResetBonuses();

        // SHOW PLAYER DAMAGE
        if (beyblade.gameObject.tag != "Player"){
            UIEvents.current.Combo((int)damage);
        }

        // APPLY KNOCKBACK
        if(!opponent.parrying && beyblade.storedDamage == Vector2.zero){
            beyblade.rb.AddForce(knockback, ForceMode2D.Impulse);
        }
        else if(opponent.parrying){
            beyblade.rb.AddForce(knockback * 2, ForceMode2D.Impulse);
        }
        ResetBonuses();
        if (beyblade.overdriving) return;
        if (beyblade.currentMeter >= beyblade.maxMeter) return;
        beyblade.currentMeter += (int)(damage * 0.35f);
    }
    */

    private void NewKnockback(Vector3 direction, BeybladeCollision opponent)
    {
        ResetBonuses();
        ChanceBonuses();

        // Spark Effects
        GameObject spark = Instantiate(beyblade.sparkEffect, new Vector2(0, 0), Quaternion.identity);
        spark.transform.position = beyblade.transform.position;
        if (criticalHit)
        {
            var particleSystem = spark.GetComponent<ParticleSystem>();
            particleSystem.Stop();
            var color = particleSystem.colorOverLifetime;
            color.enabled = true;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
            color.color = grad;
            particleSystem.Play();
        }

        // Sound Effects
        if (!criticalHit) GameEvents.current.Hit();

        // Knockback Calculation
        // You deal no knockback while Phantom Dashing
        // If you are hit while Phantom Dashing you take full knockback.

        Vector3 knockback;
        if(opponent.criticalHit)
        {
            StartCoroutine(HitStop());
            knockback = direction * 40f;
        }
        else if(criticalDefend)
        {
            StartCoroutine(HitStop());
            knockback = direction * 10f;
        }
        else if (opponent.GetComponent<Beyblade>().phantomDashing)
        {
            knockback = direction * 0.0f;
        }
        else
        {
            knockback = direction * 20f;
        }

        //knockback = direction * 0;
        beyblade.rb.AddForce(knockback, ForceMode2D.Impulse);

        if (beyblade.currentMeter >= beyblade.maxMeter) return;
        beyblade.currentMeter += (int)(10);
    }

    private IEnumerator HitStop()
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1.0f;
    }
    private void ChanceBonuses()
    {
        if (beyblade.mWheel.bonusAttack != 0)
        {
            int roll = Random.Range(0, 100);
            if (roll >= beyblade.mWheel.bonusAttackChance) return;
            beyblade.bonusAttack = beyblade.mWheel.bonusAttack;
            beyblade.attack += beyblade.bonusAttack;
            GameEvents.current.CriticalHit();
            UIEvents.current.CriticalHit(beyblade);
            print(gameObject.name + " Critical Hit!");
            criticalHit = true;
        }
        if (beyblade.mWheel.bonusDefense != 0)
        {
            int roll = Random.Range(0, 100);
            if (roll >= beyblade.mWheel.bonusDefenseChance) return;
            beyblade.bonusDefense = beyblade.mWheel.bonusDefense;
            beyblade.defense += beyblade.bonusDefense;
            print(gameObject.name + " Critical Defend!");
            GameEvents.current.CriticalGuard();
            UIEvents.current.CriticalDefend(beyblade);
            criticalDefend = true;
        }
    }
    private void ResetBonuses()
    {
        beyblade.attack -= beyblade.bonusAttack;
        beyblade.bonusAttack = 0;

        beyblade.defense -= beyblade.bonusDefense;
        beyblade.bonusDefense = 0;

        criticalHit = false;
        criticalDefend = false;
    }    
    private void NotColliding()
    {
        colliding = false;
    }
    private void StopSpinning()
    {
        beyblade.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void StartDangerTime()
    {
        print("Start Danger Time");
        dangerTime = true;
        UIEvents.current.DangerTime();

        // Disable both beyblade movement and hitbox
        // Dim the screen
        // Red glow appears from top and bottom of screen
        // Warning flashes onto screen
        // Countdown from 3 starts before 
        // Enable both beyblade movement and hitbox

        // Meter is infinite 
        // Beyblades slowly increase in stamina instead of decay
        // Attacks deal double damage and reduced knockback
        // Beyblades have access to their super moves

        // After 5 Seconds Danger Time ends and cannot be activated again until the next round
    }

    private void EndDangerTime()
    {
        dangerTime = false;
    }
}
