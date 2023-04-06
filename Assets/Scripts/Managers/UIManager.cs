using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject dangerTimeBanner;

    private GameObject criticalHitBanner;
    private GameObject criticalDefendBanner;
    private GameObject parryBanner;
    private GameObject comboSlider;
    private GameObject comboDamage;
    private Text comboNumber;
    private GameObject comboText;
    private GameObject criticalHitBanner2;
    private GameObject criticalDefendBanner2;
    private GameObject parryBanner2;
    private int comboValue = 0;

    private void Start()
    {
        UIEvents.current.onCriticalHit += CriticalHitBanner;
        UIEvents.current.onCriticalDefend += CriticalDefendBanner;
        UIEvents.current.onParry += ParryBanner;
        UIEvents.current.onHit += IncreaseComboMeter;
        UIEvents.current.onDC += DangerTimeBanner;

        criticalHitBanner = transform.GetChild(0).gameObject;
        criticalDefendBanner = transform.GetChild(1).gameObject;
        parryBanner = transform.GetChild(2).gameObject;
        comboSlider = transform.GetChild(3).gameObject;
        dangerTimeBanner = transform.GetChild(4).gameObject;
        comboNumber = transform.GetChild(5).GetComponent<Text>();
        comboText = transform.GetChild(6).gameObject;
        comboDamage = transform.GetChild(7).gameObject;
        criticalHitBanner2 = transform.GetChild(8).gameObject;
        criticalDefendBanner2 = transform.GetChild(9).gameObject;
        parryBanner2 = transform.GetChild(10).gameObject;
    }

    private void IncreaseComboMeter(Beyblade beyblade)
    {
        if (beyblade.tag != "Player") return;

        comboValue += 1;
        if (comboSlider.activeSelf == false)
        {
            comboValue = 1;
            comboSlider.SetActive(true);
            comboText.SetActive(true);
            comboNumber.gameObject.SetActive(true);
            comboDamage.gameObject.SetActive(true);
            comboNumber.text = comboValue.ToString();
        }
        else
        {
            comboSlider.GetComponent<ComboSlider>().Interupt();
            comboNumber.text = comboValue.ToString();
        }
        // if the combo meter is not active, turn it on
        // increment the combo number by 1
        // set the combo text to the combo number
        // when the combo meter is activated, it will decremeant it's slider value until it's 0. Once it's 0 it will deactivate.
    }    

    private void DangerTimeBanner()
    {
        dangerTimeBanner.SetActive(true);
    }
    private void CriticalHitBanner(Beyblade beyblade)
    {
        if(beyblade.tag == "Player"){
            criticalHitBanner.SetActive(true);
            criticalHitBanner.GetComponent<Animator>().SetTrigger("PlayerBanner");
        }
        else{
            criticalHitBanner2.SetActive(true);
            criticalHitBanner2.GetComponent<Animator>().SetTrigger("EnemyBanner");
        }
    }
    private void CriticalDefendBanner(Beyblade beyblade)
    {
        if (beyblade.tag == "Player"){
            criticalDefendBanner.SetActive(true);
            criticalDefendBanner.GetComponent<Animator>().SetTrigger("PlayerBanner");
        }
        else{
            criticalDefendBanner2.SetActive(true);
            criticalDefendBanner2.GetComponent<Animator>().SetTrigger("EnemyBanner");
        }
    }
    private void ParryBanner(Beyblade beyblade)
    {
        if (beyblade.tag == "Player"){
            parryBanner.SetActive(true);
            parryBanner.GetComponent<Animator>().SetTrigger("PlayerBanner");
        }
        else{
            parryBanner2.SetActive(true);
            parryBanner2.GetComponent<Animator>().SetTrigger("EnemyBanner");
        }
    }
}
