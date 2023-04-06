using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBeyblade : MonoBehaviour
{
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rect.eulerAngles += new Vector3(0, 0, 10);
    }
}
