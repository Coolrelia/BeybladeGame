using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIEvents : MonoBehaviour
{
    public static UIEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<Beyblade> onHit;
    public event Action<int> onCombo;
    public event Action<Beyblade> onDC;
    public event Action<Beyblade> onCriticalHit;
    public event Action<Beyblade> onCriticalDefend;
    public event Action<Beyblade> onParry;
    public event Action<Beyblade> onSelect;
    public event Action<Beyblade> onReady;

    public void DangerTime(Beyblade beyblade)
    {
        if (onDC == null) return;
        onDC(beyblade);
    }

    public void Hit(Beyblade beyblade)
    {
        if (onHit == null) return;
        onHit(beyblade);
    }
    public void Combo(int damage)
    {
        if (onCombo == null) return;
        onCombo(damage);
    }
    public void CriticalHit(Beyblade beyblade)
    {
        if (onCriticalHit == null) return;
        onCriticalHit(beyblade);
    }
    public void CriticalDefend(Beyblade beyblade)
    {
        if (onCriticalDefend == null) return;
        onCriticalDefend(beyblade);
    }
    public void Parry(Beyblade beyblade)
    {
        if (onParry == null) return;
        onParry(beyblade);
    }

    public void SelectBeyblade(Beyblade beyblade)
    {
        if (onSelect == null) return;
        onSelect(beyblade);
    }
    public void Ready(Beyblade beyblade)
    {
        if (onReady == null) return;
        onReady(beyblade);
    }
}
