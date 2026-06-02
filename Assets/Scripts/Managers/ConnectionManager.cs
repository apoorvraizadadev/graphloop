using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Game.MouseHelper;

public class ConnectionManager : MonoBehaviour
{
    [Header("Regerences")]
    public GameObject currentConnection;
    public GameObject connectionPrefab;

    [Header("Audio")]
    public AudioClip connectSound;

    [Header("Connections")]
    public List<Connection> connections;

    [Header("Layer Masks")]
    public LayerMask nodeMask;
    public LayerMask connectionMask;

    [Header("Limitations")]
    public int connectionLimit;
    public TextMeshProUGUI connectionLimitText;

    public event Action onConnection;
    public event Action onDelete;

    Vector2 connectionVelocity;
    float smoothDampTime = 0.07f;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (connectionLimit == 0) print("Connection limit set to 0");
    }

    // Update is called once per frame
    void Update()
    {
        MouseDown();
        MouseDrag();
        MouseUp();
        DeleteConnections();

        connectionLimitText.text = $"{connectionLimit - count} connections left";
    }

    void MouseDown()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D node = ObjectAtMouse(nodeMask);

            if (node == true && count < connectionLimit)
            {
                print("New connection");

                Vector2 nodePos = (Vector2)node.transform.position;

                currentConnection = Instantiate(connectionPrefab);

                Connection connection = currentConnection.GetComponent<Connection>();
                connection.from = node.transform.gameObject;
                connection.SetTransform(nodePos, Vector2.SmoothDamp(nodePos, MousePos(), ref connectionVelocity, smoothDampTime));
                connection.isDoneEditing = false;
            }
        }
    }

    void MouseDrag()
    {
        if (Input.GetMouseButton(1) && currentConnection != null)
        {
            Connection connection = currentConnection.GetComponent<Connection>();

            Vector2 nodePos = connection.from.transform.position;

            connection.SetTransform(nodePos, Vector2.SmoothDamp(connection.endPos, MousePos(), ref connectionVelocity, smoothDampTime));

            print("Moved connection");
        }
    }

    void MouseUp()
    {
        if (Input.GetMouseButtonUp(1) && currentConnection != null)
        {
            RaycastHit2D node = ObjectAtMouse(nodeMask);

            if (node == false)
            {
                Destroy(currentConnection);

                //print("Connection destroyed");
            }

            else if (node.transform.gameObject == currentConnection.GetComponent<Connection>().from.gameObject)
            {
                Destroy(currentConnection);

                //print("Connection destroyed because of same node");
            }

            else if (CheckForDuplicates(currentConnection.GetComponent<Connection>().from, node.transform.gameObject))
            {
                Destroy(currentConnection);

                //print("Connection destroyed because of duplicate");
            }

            else
            {
                Connection connection = currentConnection.GetComponent<Connection>();

                connection.to = node.transform.gameObject;
                connection.isDoneEditing = true;
                currentConnection = null;

                connectionVelocity = Vector2.zero;
                connections.Add(connection);

                //print("Connection fixed");


                connection.from.GetComponent<Node>().connections.Add(connection);
                connection.to.GetComponent<Node>().connections.Add(connection);

                connection.name = $"Connection {count}";

                count++;
                
                ManagerUtility.soundManager.SetPitch(0.9f);
                ManagerUtility.soundManager.PlaySound(connectSound);

                if (onConnection != null)
                {
                    onConnection();
                }
            }
        }
    }

    void DeleteConnections()
    {
        if (Input.GetMouseButton(2))
        {
            RaycastHit2D connection = ObjectAtMouse(connectionMask);

            if (connection == true)
            {
                Connection c = connection.transform.GetComponent<Connection>();
                c.from.GetComponent<Node>().connections.Remove(c);
                c.to.GetComponent<Node>().connections.Remove(c);

                connections.Remove(connection.transform.GetComponent<Connection>());
                Destroy(connection.transform.gameObject);

                //print("Deleted connection");

                if (onDelete != null)
                {
                    onDelete();
                }

                count--;
            }
        }
    }

    bool CheckForDuplicates(GameObject from, GameObject to)
    {
        if (connections.Count == 0) return false;

        foreach (Connection connection in connections)
        {
            if ((connection.from == from && connection.to == to) || (connection.from == to && connection.to == from))
            {
                return true;
            }
        }

        return false;
    }
}
