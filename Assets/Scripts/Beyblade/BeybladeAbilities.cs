using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class BeybladeAbilities : MonoBehaviour
{
    private Beyblade beyblade;

    public InputAction dash;
    public InputAction guard;
    public InputAction parry;
    public InputAction phantomDash;
    public InputAction overdrive;

    public float dashDelay = 0.05f;

    private void Awake()
    {
        beyblade = GetComponent<Beyblade>();
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        if (!beyblade.launched) return;
        if (beyblade.guarding) return;
        if (beyblade.parrying) return;
        if (beyblade.currentStamina <= 0) return;
        StartCoroutine(DashAction());
    }
    private void Guard(InputAction.CallbackContext ctx)
    {
        // THIS IS NOW THE REFLECT ABILITY

        if (!beyblade.launched) return;
        if (beyblade.phantomDashing) return;
        if (beyblade.currentStamina <= 0) return;
        if (beyblade.dashing) return;

        if (ctx.performed)
        {


            /*
            beyblade.decayRate += 0.5f;
            beyblade.defense += 10;
            beyblade.stamina += 10;
            beyblade.guarding = true;
            beyblade.anim.SetBool("Shielding", true);
            */
        }
        if (ctx.canceled)
        {
            beyblade.decayRate -= 0.5f;
            beyblade.defense -= 10;
            beyblade.stamina -= 10;
            beyblade.guarding = false;
            beyblade.anim.SetBool("Shielding", false);
        }
    }
    private void Parry(InputAction.CallbackContext ctx)
    {
        if (!beyblade.launched) return;
        if (beyblade.currentMeter < beyblade.parryCost) return;
        if (beyblade.parrying) return;
        if (beyblade.phantomDashing) return;
        if (!beyblade.collision.colliding) return;
        if (beyblade.opponent.GetComponent<Beyblade>().parrying) return;
        StartCoroutine(ParryAction());
    }
    private void PhantomDash(InputAction.CallbackContext ctx)
    {
        if (!beyblade.launched) return;
        if (beyblade.currentMeter < beyblade.phantomDashCost) return;
        if (beyblade.phantomDashing) return;
        if (beyblade.opponent.GetComponent<Beyblade>().phantomDashing) return;
        StartCoroutine(PhantomDashAction());
    }
    private void Overdrive(InputAction.CallbackContext ctx)
    {
        if (beyblade.currentMeter <= 0) return;
        if (beyblade.currentStamina <= 0) return;
        if (beyblade.phantomDashing) return;
        if (beyblade.parrying) return;
        if (beyblade.overDriven) return;
        StartCoroutine(OverdriveAction());
    }


    public void CancelPhantomDash()
    {
        StopCoroutine(PhantomDashAction());
        beyblade.phantomDashing = false;
        if(Time.timeScale == 0.3f)
        {
            beyblade.currentStamina = (beyblade.currentStamina - 25f) * Time.timeScale;
            Time.fixedDeltaTime = Time.fixedDeltaTime / Time.timeScale;
            Time.timeScale = 1;
            beyblade.rb.velocity = beyblade.rb.velocity / 2;
            beyblade.rb.angularVelocity = beyblade.rb.angularVelocity / 2;
        }
        GameEvents.current.PhantomCancel(beyblade.gameObject);
        print("Phantom Dash Cancelled");
    }

    private IEnumerator DashAction()
    {
        //GameEvents.current.Dash();
        beyblade.dashing = true;
        beyblade.bonusAttack += 2;
        beyblade.attack += beyblade.bonusAttack;
        beyblade.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(dashDelay);
        beyblade.rb.constraints = RigidbodyConstraints2D.None;
        beyblade.rb.AddForce(beyblade.movement.moveInput * 20, ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(0.1f);
        beyblade.dashing = false;
        beyblade.attack -= beyblade.bonusAttack;
        beyblade.bonusAttack = 0;
    }
    private IEnumerator PhantomDashAction()
    {
        beyblade.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        beyblade.currentMeter -= beyblade.phantomDashCost;
        GameEvents.current.PhantomTimer(beyblade.gameObject);
        beyblade.phantomDashing = true;
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
        beyblade.currentStamina = (beyblade.currentStamina / Time.timeScale);
        beyblade.rb.constraints = RigidbodyConstraints2D.None;
        beyblade.rb.AddForce(beyblade.movement.moveInput * (15/Time.timeScale), ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(0.5f);
        beyblade.currentStamina = (beyblade.currentStamina * Time.timeScale) + 15f;
        Time.fixedDeltaTime = Time.fixedDeltaTime / Time.timeScale;
        Time.timeScale = 1;
        beyblade.rb.velocity = beyblade.rb.velocity / 2;
        beyblade.rb.angularVelocity = beyblade.rb.angularVelocity/ 2;
        beyblade.phantomDashing = false;
    }
    private IEnumerator ParryAction()
    {
        beyblade.currentMeter -= beyblade.parryCost;

        Beyblade opponent = beyblade.opponent.GetComponent<Beyblade>();

        opponent.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        
        GameEvents.current.PhantomTimer(beyblade.opponent);
        UIEvents.current.Parry(beyblade);
        
        beyblade.parrying = true;

        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
        beyblade.currentStamina = beyblade.currentStamina / Time.timeScale;
        float originalSpeed = beyblade.currentStamina;
        beyblade.rb.velocity = Vector2.zero;
        beyblade.rb.angularVelocity = 0;
        yield return new WaitForSecondsRealtime(1f);
        float damage = (originalSpeed - beyblade.currentStamina);
        beyblade.currentStamina = (beyblade.currentStamina * Time.timeScale) - damage;
        Time.fixedDeltaTime = Time.fixedDeltaTime / Time.timeScale;
        Time.timeScale = 1;
        
        beyblade.rb.velocity = beyblade.rb.velocity / 2;
        beyblade.rb.angularVelocity = beyblade.rb.angularVelocity / 2;
        beyblade.parrying = false;
        
        opponent.rb.constraints = RigidbodyConstraints2D.None;
        opponent.rb.AddForce(opponent.storedDamage * 0.3f, ForceMode2D.Impulse);
        opponent.storedDamage = Vector2.zero;
    }
    private IEnumerator OverdriveAction()
    {
        //you're stopped, the screen dims, and an effect plays
        print("overdrive");
        beyblade.overDriven = true;
        beyblade.overdriving = true;
        beyblade.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        beyblade.bonusAttack += 10;
        beyblade.attack += beyblade.bonusAttack;
        GameObject effect = Instantiate(beyblade.overdriveEffect);
        effect.transform.position = beyblade.transform.position;
        UIEvents.current.Overdrive(0.5f);
        yield return new WaitForSeconds(0.2f);
        beyblade.rb.constraints = RigidbodyConstraints2D.None;

        while (beyblade.currentMeter > 0)
        {
            beyblade.currentMeter -= 1;
            beyblade.currentStamina += 0.5f;
            yield return new WaitForSeconds(0.05f);
        }
        beyblade.attack -= beyblade.bonusAttack;
        beyblade.bonusAttack = 0;
        beyblade.overdriving = false;
        print("end overdrive");
    }

    private void OnEnable()
    {
        dash.Enable();
        dash.performed += Dash;

        guard.Enable();
        guard.performed += Guard;
        guard.canceled += Guard;

        parry.Enable();
        parry.performed += Parry;

        phantomDash.Enable();
        phantomDash.performed += PhantomDash;

        overdrive.Enable();
        overdrive.performed += Overdrive;
    }
    private void OnDisable()
    {
        dash.Disable();
        guard.Disable();
        parry.Disable();
        phantomDash.Disable();
        overdrive.Disable();
    }    
}
