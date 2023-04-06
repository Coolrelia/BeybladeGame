using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    private Beyblade target;
    private Text beybladeName;
    private bool setupDone = false;

    private float maxSpeed = 0f;
    private float speed = 0f;

    public int playerSpeedometer = 0;
    public float minSpeedArrowAngle = 0f;
    public float maxSpeedArrowAngle = 0f;

    [Header("UI")]
    public Text speedLabel;
    public RectTransform arrow;

    private void Setup()
    {
        if (setupDone) return;

        if (playerSpeedometer == 1)
        {
            if (GameObject.FindGameObjectWithTag("Player") == null) return;
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Beyblade>();
        }
        else
        {
            target = FindObjectOfType<EnemyAI>().GetComponent<Beyblade>();
        }

        if (target == null) return;
        maxSpeed = target.launchSpeed;
        beybladeName = transform.GetChild(2).GetComponent<Text>();
        beybladeName.text = target.mWheel.name;
        setupDone = true;
    }

    private void Update()
    {
        Setup();
        if (!setupDone) return;

        speed = target.currentStamina;

        if (speedLabel != null){
            speedLabel.text = ((int)speed) * 60 + "\nRPM";
        }   
        if(arrow != null)
        {
            arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
        }
    }
}
