using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;

    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            transition.SetTrigger("NotMainMenu");
        }
    }

    public void ChangeScene(int scene)
    {
        StartCoroutine(Transition(scene));
    }

    public void IncrementScene()
    {
        StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator Transition(int scene)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(scene);
    }
}
