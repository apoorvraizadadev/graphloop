using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MouseHelper;

public class Node : MonoBehaviour
{
    public NodeType nodeType;
    public List<Connection> connections;

    public event Action onMove;
    public bool active = true;
    public bool isValid = false;

    Vector2 offset;
    Vector2 targetPos;

    float smoothDampTime = 0.07f;
    Vector2 smoothDampVelocity;

    void Awake()
    {
        targetPos = transform.position;
    }

    void OnValidate()
    {
        ApplyShader();
    }

    void Start()
    {
        ApplyShader();
    }

    void OnMouseDrag()
    {
        if (active) targetPos = MousePos() + offset;
    }

    void Update()
    {
        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref smoothDampVelocity, smoothDampTime);

        if (connections.Count == 0)
        {
            isValid = false;
        }

        else if (nodeType == NodeType.Square && connections.Count < 4)
        {
            isValid = false;
        }

        else if (nodeType == NodeType.Triangle)
        {
            bool broken = false;
            foreach (var connection in connections)
            {
                Node n = connection.from.GetComponent<Node>();

                if (n == this)
                {
                    n = connection.to.GetComponent<Node>();
                }

                if (n.nodeType == NodeType.Circle)
                {
                    isValid = false;
                    broken = true;
                    break;
                }
            }

            if (!broken)
            {
                isValid = true;
            }
        }

        else
        {
            isValid = true;
        }
    }

    void OnMouseDown()
    {
        if (active) offset = (Vector2)transform.position - MousePos();
    }

    void OnMouseUp()
    {
        if (onMove != null && active)
        {
            onMove();
        }
    }

    void ApplyShader()
    {
        string shaderPath = "";

        switch (nodeType)
        {
            case NodeType.Circle:
                shaderPath = "Shapes/Circle";
                break;
            case NodeType.Square:
                shaderPath = "Shapes/Rectangle";
                break;
            case NodeType.Triangle:
                shaderPath = "Shapes/Triangle";
                break;
            default:
                break;
        }

        Material mat = new Material(Shader.Find(shaderPath));

        if (nodeType == NodeType.Square)
        {
            mat.SetVector("_Size", new Vector4(transform.localScale.x, transform.localScale.y, transform.localScale.y / 4));
        }

        if (nodeType == NodeType.Triangle)
        {
            mat.SetFloat("_Rad", 0.2f);
            mat.SetFloat("_Scale", 0.8f);
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        mat.SetColor("_Color", spriteRenderer.color);
        spriteRenderer.material = mat;
    }
}


public enum NodeType
{
    Circle, Square, Triangle
}