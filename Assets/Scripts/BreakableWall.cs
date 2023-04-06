using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private SpriteRenderer rend;

    public int hitsBeforeBreak;
    public bool breakable;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (!breakable) return;
        if (hitsBeforeBreak > 0) return;
        hitsBeforeBreak = 3;
    }

    private void Update()
    {
        switch (hitsBeforeBreak)
        {
            case 3:
                rend.color = new Color32(41, 159, 0, 255);
                break;
            case 2:
                rend.color = new Color32(207, 212, 0, 255);
                break;
            case 1:
                rend.color = new Color32(160, 5, 0, 255);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!breakable) return;

        if(hitsBeforeBreak == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            hitsBeforeBreak--;
            if(hitsBeforeBreak == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
