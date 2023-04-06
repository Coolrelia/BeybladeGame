using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeybladeSelect : MonoBehaviour
{
    private Beyblade selectedBeyblade = null;
    private Transform beybladePoolParent = null;
    private Animator anim;
    [SerializeField] private SelectableBeyblade selectableBeybladePrefab = null;
    [SerializeField] private GameObject[] beybladePrefab = null;

    [Header("UI")]
    public Text beybladeName;
    public Image beybladeImage;
    public Text beybladeAttack;
    public Text beybladeDefense;
    public Text beybladeStamina;
    public Text beybladeLAD;
    public Text beybladeDecayRate;

    private void Awake()
    {
        beybladePoolParent = transform.GetChild(0);
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        UIEvents.current.onSelect += SelectBeyblade;
        //populate the beyblade pool with all the selectable beyblade prefabs
        for (int i = 0; i < beybladePrefab.Length; i++)
        {
            SelectableBeyblade beyblade = Instantiate(selectableBeybladePrefab, beybladePoolParent);
            beyblade.beyblade = beybladePrefab[i].GetComponent<Beyblade>();
        }
    }

    private void SelectBeyblade(Beyblade beyblade)
    {
        if(selectedBeyblade == beyblade){
            selectedBeyblade = null;
            beybladeName.text = "";
            beybladeImage.sprite = null;
            beybladeAttack.text = "";
            beybladeDefense.text = "";
            beybladeStamina.text = "";
            beybladeLAD.text = "";
            beybladeDecayRate.text = "";
        }
        else{
            selectedBeyblade = beyblade;
            beybladeName.text = selectedBeyblade.mWheel.name;
            beybladeImage.sprite = selectedBeyblade.mWheel.sprite;
            beybladeAttack.text = "Attack: " + selectedBeyblade.mWheel.attack;
            beybladeDefense.text = "Defense: " + (selectedBeyblade.mWheel.defense + selectedBeyblade.driver.defense);
            beybladeStamina.text = "Stamina: " + selectedBeyblade.driver.stamina;
            beybladeLAD.text = "LAD: " + selectedBeyblade.driver.lad;
            beybladeDecayRate.text = "Decayrate: " + selectedBeyblade.driver.decayRate;
        }
    }

    public void Ready()
    {
        if (selectedBeyblade == null) return;
        UIEvents.current.Ready(selectedBeyblade);
        anim.SetTrigger("Exit");
    }
}
