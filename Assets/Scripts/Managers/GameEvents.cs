using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<GameObject> onPhantomTimer;
    public event Action<GameObject> onPhantomCancel;
    public event Action<int> onHit;
    public event Action<int> onCriticalHit;
    public event Action<int> onGuard;
    public event Action<int> onCriticalGuard;
    public event Action<int> onDash;
    public event Action<int> onParry;
    
    public void PhantomTimer(GameObject player)
    {
        if (onPhantomTimer == null) return;
        onPhantomTimer(player);
    }
    public void PhantomCancel(GameObject player)
    {
        if (onPhantomCancel == null) return;
        onPhantomCancel(player);
    }
    public void Hit()
    {
        if (onHit == null) return;
        onHit(0);
    }
    public void CriticalHit()
    {
        if (onCriticalHit == null) return;
        onCriticalHit(1);
        // display critical hit banner
    }
    public void Guard()
    {
        if (onGuard == null) return;
        onGuard(2);
    }
    public void CriticalGuard()
    {
        if (onCriticalGuard == null) return;
        onCriticalGuard(3);
        // display critical guard banner
    }
    public void Dash()
    {
        if (onDash == null) return;
        onDash(4);
    }
    public void Parry()
    {
        if (onParry == null) return;
        onParry(5);
        // display parry banner
    }
}
