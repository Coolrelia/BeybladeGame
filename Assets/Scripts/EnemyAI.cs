using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Beyblade beyblade;
    private BeybladeCollision collision;
    private Rigidbody2D rb;
    private bool knockedBack;
    private float moveSpeed = 7;

    public Transform centerPoint;
    public float rotationDegree;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        beyblade = GetComponent<Beyblade>();
        collision = GetComponent<BeybladeCollision>();
    }

    void FixedUpdate()
    {
        if (beyblade.currentStamina <= 0) return;
        if (beyblade.opponent == null) return;

        ParryCheck();
        if (beyblade.reflecting) return;

        if (collision.colliding && !beyblade.reflecting){
            StartCoroutine(KnockbackWait());
        }
        if (knockedBack) return;

        PhantomDashCheck();
        if (beyblade.phantomDashing) return;

        DashCheck();
        if (beyblade.dashing) return;

        Vector3 direction = centerPoint.position - transform.position;
        direction = Quaternion.Euler(0, 0, rotationDegree) * direction;
        float distanceThisFrame = (beyblade.currentStamina / 2) * Time.fixedDeltaTime;
        rb.AddForce(direction.normalized * distanceThisFrame * (beyblade.currentStamina / 4) * Time.timeScale, ForceMode2D.Force);
    }

    private IEnumerator KnockbackWait()
    {
        knockedBack = true;
        yield return new WaitForSeconds(0.8f);
        knockedBack = false;
        if (moveSpeed > 1 && moveSpeed < 10){
            moveSpeed++;
        }
    }

    private void PhantomDashCheck()
    {
        if (beyblade.currentStamina <= 0) return;
        if (beyblade.phantomDashing) return;
        if (beyblade.reflecting) return;
        if (beyblade.dashing) return;
        if (beyblade.opponent.GetComponent<Beyblade>().phantomDashing) return;
        if (beyblade.opponent.GetComponent<Beyblade>().reflecting) return;

        if(Vector2.Distance(beyblade.transform.position, beyblade.opponent.transform.position) <= 2.5)
        {
            int phantomChance = Random.Range(0, 100);
            if (phantomChance > 1) return;
            if (beyblade.currentMeter < beyblade.phantomDashCost) return;
            Vector3 dir = (transform.position - beyblade.opponent.transform.position).normalized;
            if (beyblade.phantomDashing) return;
            StartCoroutine(PhantomDashAction(dir));
        }
        return;
    }
    private void ParryCheck()
    {
        if (beyblade.currentStamina <= 0) return;

        int parryChance = Random.Range(0, 120);
        if (parryChance > 1) return;

        if (beyblade.reflecting) return;
        if (beyblade.phantomDashing) return;
        if (!beyblade.collision.colliding) return;
        if (beyblade.currentMeter < beyblade.parryCost) return;
        if (beyblade.opponent.GetComponent<Beyblade>().reflecting) return;

        Vector3 dir = (transform.position - beyblade.opponent.transform.position).normalized;
        StartCoroutine(ParryAction(dir));
    }
    private void DashCheck()
    {
        if (beyblade.currentStamina <= 0) return;
        if (beyblade.opponent.GetComponent<Beyblade>().reflecting) return;

        if (Vector2.Distance(transform.position, beyblade.opponent.transform.position) <= 4f)
        {
            int dashChance = Random.Range(0, 25);
            if (dashChance > 1) return;

            if (beyblade.reflecting) return;
            if (beyblade.phantomDashing) return;
            if (beyblade.collision.colliding) return;
            Vector3 dir = (transform.position - beyblade.opponent.transform.position).normalized;
            StartCoroutine(DashAction(-dir));
        }
        return;
    }

    private IEnumerator PhantomDashAction(Vector3 direction)
    {
        beyblade.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        beyblade.currentMeter -= beyblade.phantomDashCost;
        GameEvents.current.PhantomTimer(beyblade.gameObject);
        beyblade.phantomDashing = true;
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
        beyblade.currentStamina = (beyblade.currentStamina / Time.timeScale);
        beyblade.rb.constraints = RigidbodyConstraints2D.None;
        beyblade.rb.AddForce(direction * (15 / Time.timeScale), ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(0.5f);
        beyblade.currentStamina = (beyblade.currentStamina * Time.timeScale) + 10f;
        Time.fixedDeltaTime = Time.fixedDeltaTime / Time.timeScale;
        Time.timeScale = 1;
        beyblade.rb.velocity = beyblade.rb.velocity / 2;
        beyblade.rb.angularVelocity = beyblade.rb.angularVelocity / 2;
        beyblade.phantomDashing = false;
    }
    private IEnumerator ParryAction(Vector3 direction)
    {
        beyblade.currentMeter -= beyblade.parryCost;

        Beyblade opponent = beyblade.opponent.GetComponent<Beyblade>();

        opponent.rb.constraints = RigidbodyConstraints2D.FreezePosition;

        GameEvents.current.PhantomTimer(beyblade.opponent);
        UIEvents.current.Parry(beyblade);

        beyblade.reflecting = true;

        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
        beyblade.currentStamina = beyblade.currentStamina / Time.timeScale;
        float originalSpeed = beyblade.currentStamina;
        beyblade.rb.velocity = Vector2.zero;
        beyblade.rb.angularVelocity = 0;
        beyblade.rb.AddForce(-direction * (30 / Time.timeScale), ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(1f);
        float damage = (originalSpeed - beyblade.currentStamina) /2;
        beyblade.currentStamina = beyblade.currentStamina * Time.timeScale;
        beyblade.currentStamina -= damage;
        Time.fixedDeltaTime = Time.fixedDeltaTime / Time.timeScale;
        Time.timeScale = 1;

        beyblade.rb.velocity = beyblade.rb.velocity / 2;
        beyblade.rb.angularVelocity = beyblade.rb.angularVelocity / 2;
        beyblade.reflecting = false;

        opponent.rb.constraints = RigidbodyConstraints2D.None;
        opponent.rb.AddForce(opponent.storedDamage * 0.17f, ForceMode2D.Impulse);
        opponent.storedDamage = Vector2.zero;
    }
    private IEnumerator DashAction(Vector3 direction)
    {
        //GameEvents.current.Dash();
        beyblade.dashing = true;
        beyblade.bonusAttack += 2;
        beyblade.attack += beyblade.bonusAttack;
        beyblade.rb.AddForce(direction * 15, ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(0.1f);
        beyblade.attack -= beyblade.bonusAttack;
        beyblade.bonusAttack = 0;
        yield return new WaitForSeconds(0.5f);
        beyblade.dashing = false;
    }
}
