using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Ability
{
    public float vanishDelay = 1f;
    public float reappearDelay = 2f;

    public override void UseAbility()
    {
        // freeze beyblade in place
        // wait a small delay
        // beyblade dissapears from the screen
        // wait a small delay
        // beyblade reappears behind the opponent 
        // beyblade is given increased damage for a small amount of time
        StartCoroutine(TeleportAbility());
    }

    public override void UseSuper()
    {
        base.UseSuper();
    }

    private IEnumerator TeleportAbility()
    {
        beyblade.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(0.2f);
        beyblade.GetComponent<BoxCollider2D>().isTrigger = true;
        beyblade.GetComponent<SpriteRenderer>().sprite = null;
        
        yield return new WaitForSeconds(0.2f);

        beyblade.transform.position = DetermineTeleportLocation();
        beyblade.GetComponent<SpriteRenderer>().sprite = beyblade.mWheel.sprite;
        beyblade.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private Vector2 DetermineTeleportLocation()
    {
        

        return Vector2.zero;
    }
}
