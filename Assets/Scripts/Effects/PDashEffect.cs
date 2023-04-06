using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDashEffect : MonoBehaviour
{
    public float timeBeforeDestroy;

    private void Start()
    {
        Invoke("Destroy", timeBeforeDestroy);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
