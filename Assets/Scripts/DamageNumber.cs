using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    public Text damageNumber;

    public void Damage(int damage)
    {
        damageNumber.text = "-" + damage.ToString();
        Invoke("Destroy", 0.8f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }    
}
