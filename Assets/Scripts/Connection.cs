using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public GameObject from;
    public GameObject to;

    public float thickness;

    [HideInInspector] public bool isDoneEditing = false;
    [HideInInspector] public Vector2 endPos;

    Material mat;

    public void Start()
    {
        mat = new Material(Shader.Find("Shapes/Line"));
        GetComponent<SpriteRenderer>().material = mat;
    }

    public void Update()
    {
        if (isDoneEditing)
        {
            SetTransform(from.transform.position, to.transform.position);
        }
    }

    public void SetTransform(Vector2 point1, Vector2 point2)
    {
        transform.position = (point1 + point2) / 2;
        SetZ(1);
        float angle = Vector2.SignedAngle(Vector2.right, point2 - point1);
        transform.rotation = Quaternion.Euler(angle * Vector3.forward);

        float length = Vector2.Distance(point1, point2);
        transform.localScale = new Vector2(length + thickness, thickness);
        endPos = point2;

        mat.SetColor("_Color", GetComponent<SpriteRenderer>().color);
        mat.SetFloat("_Length", length);
        mat.SetFloat("_Thickness", thickness);
    }

    public void SetZ(float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
