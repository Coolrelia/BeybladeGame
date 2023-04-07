using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomTimer : MonoBehaviour
{
    public GameObject dashEffect;
    public GameObject parryEffect;

    [SerializeField] private AudioClip timerSFX = null;
    [SerializeField] private GameObject screenDim = null; 
    private Animator anim;
    private Beyblade beyblade;
    private GameObject opponent;
    private AudioSource audioSource;

    private GameObject dashObject = null;
    private GameObject parryObject = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameEvents.current.onPhantomTimer += Timer;
        GameEvents.current.onPhantomCancel += StopTimer;        
    }

    private void Update()
    {
        if (opponent == null) return;
        Vector2 offsetPos = new Vector2(opponent.transform.position.x + 1, opponent.transform.position.y + 1);
        transform.position = offsetPos;

        if(dashObject != null && beyblade.phantomDashing){
            dashObject.transform.position = beyblade.transform.position;
        }
        else if (parryObject != null && beyblade.reflecting){
            parryObject.transform.position = beyblade.transform.position;
        }
    }

    private void ScreenDim(float time)
    {
        StartCoroutine(ScreenDimming(time));
    }

    private IEnumerator ScreenDimming(float time)
    {
        screenDim.SetActive(true);
        yield return new WaitForSeconds(time);
        if (beyblade.dangerMode) yield break;
        screenDim.SetActive(false);
    }

    private void Timer(GameObject target)
    {
        StartCoroutine(StartTimer(target));
        beyblade = target.GetComponent<Beyblade>();        
    }

    private void StopTimer(GameObject beyblade)
    {
        StopCoroutine(StartTimer(beyblade));
        Reset(new Vector2(0, 10));
    }

    private IEnumerator StartTimer(GameObject beyblade)
    {
        audioSource.clip = timerSFX;
        audioSource.Play();
        opponent = beyblade.GetComponent<Beyblade>().opponent;
        Vector2 originalPos = transform.position;
        screenDim.SetActive(true);

        Vector2 spawnPoint = new Vector2(0, 20);
        dashObject = Instantiate(dashEffect, spawnPoint, Quaternion.identity);
        parryObject = Instantiate(parryEffect, spawnPoint, Quaternion.identity);

        anim.SetTrigger("Timer");
        yield return new WaitForSecondsRealtime(0.5f);
        Reset(originalPos);
    }

    private void Reset(Vector2 originalPos)
    {
        opponent = null;
        transform.position = originalPos;
        screenDim.SetActive(false);
    }
}
