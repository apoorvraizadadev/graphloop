using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public void SeeCredits()
    {
        GetComponent<Animator>().SetTrigger("Credits");
    }

    public void LevelSelect()
    {
        GetComponent<Animator>().SetTrigger("LevelSelect");
    }

    public void MainMenu()
    {
        GetComponent<Animator>().SetTrigger("MainMenu");
    }
}
