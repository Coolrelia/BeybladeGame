using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class BeybladeAbilities : MonoBehaviour
{
    private Beyblade beyblade;

    public InputAction dash;
    public InputAction reflect;
    public InputAction phantomDash;

    public float dashDelay = 0.05f;

    private void Awake()
    {
        beyblade = GetComponent<Beyblade>();
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        if (!beyblade.launched) return;
        if (beyblade.guarding) return;
        if (beyblade.reflecting) return;
        if (beyblade.currentStamina <= 0) return;
        StartCoroutine(DashAction());
    }
    private void Reflect(InputAction.CallbackContext ctx)
    {
        if (!beyblade.launched) return;
        if (beyblade.currentMeter < beyblade.reflectCost) return;
        if (beyblade.reflecting) return;
        if (beyblade.phantomDashing) return;
        if (!beyblade.collision.colliding) return;
        if (beyblade.opponent.GetComponent<Beyblade>().reflecting) return;
        StartCoroutine(ReflectAction());
    }
    private void PhantomDash(InputAction.CallbackContext ctx)
    {
        if (!beyblade.launched) return;
        if (beyblade.currentMeter < beyblade.phantomDashCost) return;
        if (beyblade.phantomDashing) return;
        if (beyblade.opponent.GetComponent<Beyblade>().phantomDashing) return;
        StartCoroutine(PhantomDashAction());
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
        beyblade.rb.AddForce(beyblade.movement.moveInput * 15, ForceMode2D.Impulse);
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
    private IEnumerator ReflectAction()
    {
        // Hit Stop
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1.0f;

        // Knockback Opposing Beyblade
        Vector3 dir = (beyblade.opponent.transform.position - beyblade.transform.position).normalized;
        beyblade.opponent.GetComponent<Beyblade>().rb.AddForce(dir * 50, ForceMode2D.Impulse);

        yield return null;
    }

    private void OnEnable()
    {
        dash.Enable();
        dash.performed += Dash;

        reflect.Enable();
        reflect.performed += Reflect;

        phantomDash.Enable();
        phantomDash.performed += PhantomDash;
    }
    private void OnDisable()
    {
        dash.Disable();
        reflect.Disable();
        phantomDash.Disable();
    }    
}
