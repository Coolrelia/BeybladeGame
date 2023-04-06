using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Animator sceneChangeAnimator;

    public void Campaign()
    {
        StartCoroutine(ChangeScene("Singleplayer"));
    }

    private IEnumerator ChangeScene(string scene)
    {
        sceneChangeAnimator.SetTrigger("Change");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
