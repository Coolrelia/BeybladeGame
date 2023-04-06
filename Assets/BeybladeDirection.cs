using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeybladeDirection : MonoBehaviour
{
    private Beyblade beyblade;
    public bool enemy;

    private void Start()
    {
        if (enemy)
        {
            beyblade = FindObjectOfType<EnemyAI>().GetComponent<Beyblade>();
        }
    }

    void Update()
    {
        if (!enemy)
        {
            if(GameObject.FindGameObjectWithTag("Player") != null)
            {
                beyblade = GameObject.FindGameObjectWithTag("Player").GetComponent<Beyblade>();
            }            
        }

        if (!beyblade) return;

        if (enemy)
        {
            transform.position = new Vector3(beyblade.transform.position.x - 0.7f, beyblade.transform.position.y, 0);
        }
        else
        {
            transform.position = new Vector3(beyblade.transform.position.x + 0.5f, beyblade.transform.position.y, 0);
        }
    }
}
