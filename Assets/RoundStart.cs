using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundStart : MonoBehaviour
{
    public Beyblade player;
    public Beyblade enemy;
    public Text roundStartText;
    public GameObject[] walls;
    [SerializeField] private Transform beybladeParent = null;

    public bool english = false;

    private void Start()
    {
        UIEvents.current.onReady += StartRound;
        enemy = FindObjectOfType<EnemyAI>().GetComponent<Beyblade>();
    }

    public void StartRound(Beyblade playerBeyblade)
    {
        GameObject playersBeyblade = Instantiate(playerBeyblade.gameObject, beybladeParent);
        player = playersBeyblade.GetComponent<Beyblade>();
        player.transform.position = new Vector2(-2, 0);

        enemy.opponent = player.gameObject;
        player.opponent = enemy.gameObject;

        StartCoroutine(RoundSetup());
    }
    private IEnumerator RoundSetup()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        enemy.rb.constraints = RigidbodyConstraints2D.FreezeAll;

        roundStartText.text = "3";
        yield return new WaitForSeconds(1f);

        roundStartText.text = "2";
        yield return new WaitForSeconds(1f);

        roundStartText.text = "1";
        yield return new WaitForSeconds(1f);

        if(english){
            roundStartText.text = "LET IT RIP!!!";
        }
        else{
            roundStartText.text = "GO SHOOT!!!";
        }

        yield return new WaitForSeconds(0.5f);

        player.rb.constraints = RigidbodyConstraints2D.None;
        enemy.rb.constraints = RigidbodyConstraints2D.None;

        player.LaunchBeyblade();
        enemy.LaunchBeyblade();

        roundStartText.text = "";
    }

    public void ResetRound()
    {
        StartCoroutine(RoundReset());
    }
    private IEnumerator RoundReset()
    {
        yield return new WaitForSeconds(1f);

        player.dashing = false;
        player.phantomDashing = false;
        player.parrying = false;
        enemy.dashing = false;
        enemy.phantomDashing = false;
        enemy.parrying = false;


        player.launched = false;
        enemy.launched = false;
        player.currentStamina = 0;
        enemy.currentStamina = 0;
        GameEvents.current.PhantomCancel(player.gameObject);
        GameEvents.current.PhantomCancel(enemy.gameObject);
        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        enemy.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        player.decayRate = player.driver.decayRate;
        enemy.decayRate = enemy.driver.decayRate;

        player.transform.position = new Vector2(-2, 0);
        enemy.transform.position = new Vector2(2, 0);

        foreach(GameObject wall in walls)
        {
            wall.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(RoundSetup());
    }
}
