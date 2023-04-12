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
        if (beyblade.frozen) return;

        float moveSpeed = 0;
        float beybladeStamina = beyblade.currentStamina / 2;
        if(beybladeStamina > 200)
        {
            moveSpeed = 170;
        }
        else if(beybladeStamina > 150)
        {
            moveSpeed = 140;
        }
        else if(beybladeStamina > 100)
        {
            moveSpeed = 100;
        }
        else if(beybladeStamina <= 80)
        {
            moveSpeed = 75;
        }

        if (moveSpeed == 0) moveSpeed = 100;

        beyblade.rb.AddForce(moveInput * moveSpeed, ForceMode2D.Force);
        print(moveSpeed);
    }
}
