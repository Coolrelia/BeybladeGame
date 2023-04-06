using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkEffect : MonoBehaviour
{
    private void Start()
    {
        Invoke("Destroy", 0.3f);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
