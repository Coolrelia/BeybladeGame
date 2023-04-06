using UnityEngine;
using UnityEngine.InputSystem;
public class BeybladeMovement : MonoBehaviour
{
    private Beyblade beyblade;
    public InputAction playerMovement;
    
    [HideInInspector]
    public Vector2 moveInput;

    private void OnEnable()
    {
        playerMovement.Enable();
    }

    private void OnDisable()
    {
        playerMovement.Disable();
    }

    private void Awake()
    {        
        beyblade = GetComponent<Beyblade>();
    }


    private void FixedUpdate()
    {
        moveInput = playerMovement.ReadValue<Vector2>();
        Movement();
    }

    private void Movement()
    {
        if (beyblade.currentStamina <= 0) return;
        if (beyblade.phantomDashing) return;

        if (beyblade.guarding){
            beyblade.rb.AddForce(moveInput * (beyblade.currentStamina / 4), ForceMode2D.Force);
        }
        else{
            beyblade.rb.AddForce(moveInput * (beyblade.currentStamina / 2), ForceMode2D.Force);
        }
    }
}
