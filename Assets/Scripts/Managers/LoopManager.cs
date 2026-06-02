using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LoopManager : MonoBehaviour
{
    public List<Loop> loops;
    public List<Node> nodes;

    void Start()
    {
        loops.Clear();
        nodes = FindObjectsOfType<Node>().ToList<Node>();
    }

    public int CountLoopsForNode(Node startingNode, Node currentNode, List<Node> nodesTravelledTo, List<Connection> connectionsTravelledThrough, int depth)
    {

        int sum = 0;

        nodesTravelledTo.Add(startingNode);

        foreach (var connection in currentNode.connections)
        {
            Node newNode = connection.to.GetComponent<Node>();

            if (newNode == currentNode)
            {
                newNode = connection.from.GetComponent<Node>();
            }

            if (newNode == startingNode && depth > 1)
            {
                List<Connection> updatedConnections = connectionsTravelledThrough.ToList();
                updatedConnections.Add(connection);
                updatedConnections.Sort((x, y) => x.name.CompareTo(y.name));

                if (!IsDuplicate(updatedConnections))
                {
                    loops.Add(new Loop(updatedConnections));

                    sum += 1;
                }
            }

            else if (!nodesTravelledTo.Contains(newNode))
            {
                List<Node> updatedNodes = nodesTravelledTo.ToList();
                updatedNodes.Add(newNode);

                List<Connection> updatedConnections = connectionsTravelledThrough.ToList();
                updatedConnections.Add(connection);

                sum += CountLoopsForNode(startingNode, newNode, updatedNodes, updatedConnections, depth + 1);
            }
        }

        return sum;
    }

    public int CountAllLoops()
    {
        loops.Clear();
        int sum = 0;

        foreach (var node in nodes)
        {
            sum += CountLoopsForNode(node, node, new List<Node>(), new List<Connection>(), 0);
        }

        return sum;
    }

    public bool IsDuplicate(List<Connection> checkLoop)
    {
        foreach (var loop in loops)
        {
            if (checkLoop.SequenceEqual(loop.connections))
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public struct Loop
{
    public List<Connection> connections;

    public Loop(List<Connection> connections)
    {
        this.connections = connections;
    }
}