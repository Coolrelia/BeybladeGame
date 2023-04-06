using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSlider : MonoBehaviour
{
    public GameObject comboText;
    public GameObject comboDamageObject;
    public Text comboNumber;
    public Text comboDamage;

    private int comboDamageNumber;

    private Slider comboSlider;
    private bool interupt = false;

    private void Awake()
    {
        comboSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        UIEvents.current.onCombo += StoreComboDamage;
    }

    private void OnEnable()
    {
        StartCoroutine(Countdown());
    }

    private void Update()
    {
        comboDamage.text = "Damage: " + comboDamageNumber.ToString();

        if (comboSlider.value > 0) return;
        interupt = false;
        comboNumber.text = "";
        comboDamage.text = "";
        comboDamageNumber = 0;
        comboText.SetActive(false);
        comboDamageObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void StoreComboDamage(int damage)
    {
        if(comboDamageNumber == 0)
        {
            comboDamageNumber = damage;
        }
        else
        {
            comboDamageNumber += damage;
        }
        comboDamage.text = "Damage: " + damage;
    }

    private IEnumerator Countdown()
    {
        if (interupt) yield break;
        comboSlider.value = comboSlider.maxValue;
        while(comboSlider.value > 0)
        {
            comboSlider.value -= 0.1f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    public void Interupt()
    {
        interupt = true;
        comboSlider.value = comboSlider.maxValue;
    }
}
