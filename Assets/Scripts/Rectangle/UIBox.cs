using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBox : MonoBehaviour
{
    public float radius;

    Image image;
    Material mat;
    RectTransform rect;

    void OnValidate()
    {
        SetProperties();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetProperties();
    }

    // Update is called once per frame
    void Update()
    {
        SetProperties();
    }

    void SetProperties()
    {
        if (mat == null) mat = new Material(Shader.Find("Shapes/Rectangle"));
        if (rect == null) rect = GetComponent<RectTransform>();
        if (image == null) image = GetComponent<Image>();

        mat.SetVector("_Size", new Vector4(rect.sizeDelta.x, rect.sizeDelta.y, radius, 0));
        mat.SetColor("_Color", image.color);

        image.material = mat;
    }
}
