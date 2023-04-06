using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperMeter : MonoBehaviour
{
    public int playerMeter;
    public Image fill;

    private Slider meterSlider;
    private Beyblade player;

    private bool increasing = false;
    private bool decreasing = false;
    private bool setupDone = false;

    private void Awake()
    {
        meterSlider = GetComponent<Slider>();
    }

    private void Setup()
    {
        if (setupDone) return;
        
        if(playerMeter == 1){
            if (!GameObject.FindGameObjectWithTag("Player")) return;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Beyblade>();
        }
        else{
            player = FindObjectOfType<EnemyAI>().GetComponent<Beyblade>();
        }

        if (player == null) return;
        meterSlider.maxValue = player.maxMeter;
        meterSlider.value = 0;
        setupDone = true;
    }

    private void Update()
    {
        Setup();
        if (!setupDone) return;

        if (!player.overdriving){
            fill.color = new Color32(145, 253, 32, 255);
        }
        else{
            fill.color = new Color32(32, 253, 241, 255);
        }

        if (!increasing && !decreasing)
        {
            StartCoroutine(IncreaseValue());
        }
        if(meterSlider.value != player.currentMeter && !increasing && !decreasing)
        {
            StartCoroutine(DecreaseValue());
        }
    }

    private IEnumerator IncreaseValue()
    {
        increasing = true;
        while(meterSlider.value < player.currentMeter)
        {
            meterSlider.value += 1f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        increasing = false;
    }
    private IEnumerator DecreaseValue()
    {
        decreasing = true;
        while (meterSlider.value > player.currentMeter)
        {
            meterSlider.value -= 1f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        decreasing = false;
    }
}
