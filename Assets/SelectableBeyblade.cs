using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableBeyblade : MonoBehaviour
{
    [HideInInspector]public Beyblade beyblade;
    public Image beybladeImage;

    private void Start()
    {
        beybladeImage.sprite = beyblade.mWheel.sprite;
    }

    public void SelectBeyblade()
    {
        UIEvents.current.SelectBeyblade(beyblade);
    }
}
