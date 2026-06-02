using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<string> texts;
    public int dialogueNumber = 0;
    public float textAppearTime = 0.5f;

    //First part
    public Node startingNode;

    //Second part
    public Node nodePrefab;
    public Node secondNode;

    //Third part
    public Node thirdNode;

    //Fourth part
    public Animator score;

    //Fifth part
    public Animator connectionsLeft;

    //Last part
    public GameObject doneButton;

    TextMeshProUGUI tmp;
    Animator animator;
    CameraController cameraController;

    bool inProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        tmp = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        cameraController = FindObjectOfType<CameraController>();

        dialogueNumber = 0;

        startingNode.onMove += OnMove;
        startingNode.active = false;

        ManagerUtility.connectionManager.onConnection += OnConnection;
        ManagerUtility.connectionManager.onDelete += OnDelete;

        cameraController.onPan += OnPan;
        cameraController.onZoom += OnZoom;

        StartCoroutine(StartTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = texts[dialogueNumber];

        if (Input.GetKeyDown(KeyCode.Space) && dialogueNumber == 3 && !inProgress)
        {
            connectionsLeft.SetTrigger("Show");
            StartCoroutine(Next());
        }

        if (Input.GetKeyDown(KeyCode.Space) && dialogueNumber == 4 && !inProgress)
        {
            StartCoroutine(Next());
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            FindObjectOfType<SceneChanger>().ChangeScene(1);
        }
    }

    void OnMove()
    {
        if (dialogueNumber == 0 && !inProgress)
        {
            AddNode(ref secondNode, new Vector2(2, 0));
            StartCoroutine(Next());
        }
    }

    void OnConnection()
    {
        if (dialogueNumber == 1 && !inProgress)
        {
            AddNode(ref thirdNode, new Vector2(1, 1));
            StartCoroutine(Next());
        }

        else if (dialogueNumber == 2 && startingNode.connections.Count + secondNode.connections.Count + thirdNode.connections.Count == 6 && !inProgress)
        {
            score.SetTrigger("Show");
            StartCoroutine(Next());
        }
    }

    void OnDelete()
    {
        if (dialogueNumber == 5 && !inProgress)
        {
            StartCoroutine(Next());
        }
    }

    void OnPan()
    {
        if (dialogueNumber == 6 && !inProgress)
        {
            StartCoroutine(Next());
        }
    }

    void OnZoom()
    {
        if (dialogueNumber == 7 && !inProgress)
        {
            doneButton.SetActive(true);
            StartCoroutine(Next());
        }
    }

    IEnumerator StartTutorial()
    {
        yield return new WaitForSecondsRealtime(textAppearTime);

        startingNode.active = true;
    }

    IEnumerator Next()
    {
        inProgress = true;
        animator.ResetTrigger("Done");
        animator.SetTrigger("Next");

        yield return new WaitForSecondsRealtime(textAppearTime);

        dialogueNumber++;

        yield return new WaitForSecondsRealtime(textAppearTime);

        animator.ResetTrigger("Next");
        animator.SetTrigger("Done");
        inProgress = false;
    }

    public void AddNode(ref Node nodeVar, Vector2 pos)
    {
        nodeVar = Instantiate(nodePrefab, pos, Quaternion.identity).GetComponent<Node>();
        nodeVar.AddComponent<SmoothGrow>().duration = textAppearTime;
    }
}
