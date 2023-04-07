using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerTime : MonoBehaviour
{
    public Text startTimer;
    public Text endTimer;
    private int startTime = 3;

    [SerializeField] private GameObject screenDim;
    private Beyblade opponent;
    private Animator anim;
    private bool dangerTimeStarted = false;

    private void Start()
    {
        UIEvents.current.onDC += StartDangerTime;
        anim = GetComponent<Animator>();
    }

    public void UpdateStartTimer()
    {
        if (startTime == 0) startTime = 3;
        startTimer.text = startTime.ToString();
        startTime--;
    }

    private void StartDangerTime(Beyblade beyblade)
    {
        if (dangerTimeStarted) return;
        StartCoroutine(DangerTimeAction(beyblade));
    }

    private IEnumerator DangerTimeAction(Beyblade beyblade)
    {
        dangerTimeStarted = true;

        screenDim.SetActive(true);
        beyblade.frozen = true;
        beyblade.opponent.GetComponent<Beyblade>().frozen = true;
        beyblade.GetComponent<BoxCollider2D>().isTrigger = true;
        beyblade.opponent.GetComponent<BoxCollider2D>().isTrigger = true;
        beyblade.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        beyblade.opponent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        // Disable both beyblade movement and hitbox
        anim.SetTrigger("DangerTime");
        // Trigger the Danger Time animation
        yield return new WaitForSecondsRealtime(2f);
        // Wait for animation to finish
        screenDim.GetComponent<SpriteRenderer>().sortingOrder = 0;

        // Enable both beyblade movement and hitbox
        beyblade.frozen = false;
        beyblade.opponent.GetComponent<Beyblade>().frozen = false;
        beyblade.GetComponent<BoxCollider2D>().isTrigger = false;
        beyblade.opponent.GetComponent<BoxCollider2D>().isTrigger = false;
        beyblade.rb.constraints = RigidbodyConstraints2D.None;
        beyblade.opponent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        beyblade.dangerMode = true;
        beyblade.opponent.GetComponent<Beyblade>().dangerMode = true;

        beyblade.decayRate = 0;
        beyblade.opponent.GetComponent<Beyblade>().decayRate = 0;

        yield return new WaitForSecondsRealtime(5f);
        screenDim.SetActive(false);
        beyblade.dangerMode = false;
        beyblade.opponent.GetComponent<Beyblade>().dangerMode = false;
        beyblade.currentMeter = 0;
        beyblade.opponent.GetComponent<Beyblade>().currentMeter = 0;

        beyblade.decayRate = beyblade.driver.decayRate;
        beyblade.opponent.GetComponent<Beyblade>().decayRate = beyblade.opponent.GetComponent<Beyblade>().driver.decayRate;

        // After 5 Seconds Danger Time ends and cannot be activated again until the next round
    }
}
