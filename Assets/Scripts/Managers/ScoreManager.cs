using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI tmp;

    void Start()
    {
        ManagerUtility.connectionManager.onConnection += OnConnection;
        ManagerUtility.connectionManager.onDelete += OnDelete;
    }

    void OnConnection()
    {
        //score = CalculateScore();
    }

    void OnDelete()
    {
        print("Hi");
        //score = CalculateScore();
    }

    public int CalculateScore()
    {
        LoopManager loopManager = ManagerUtility.loopManager;


        loopManager.CountAllLoops();

        int totalScore = 0;

        foreach (Loop loop in loopManager.loops)
        {
            totalScore += 5 + (loop.connections.Count - 3);
        }

        foreach (Node node in loopManager.nodes)
        {
            if (!node.isValid)
            {
                tmp.text = $"Invalid nodes";
                return 0;
            }
        }

        tmp.text = $"{totalScore} points";

        return totalScore;
    }

    public void Update()
    {
        score = CalculateScore();
    }
}
