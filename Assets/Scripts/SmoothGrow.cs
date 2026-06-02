using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothGrow : MonoBehaviour
{
    public float duration = 0.5f;

    float startTime;
    Vector3 startScale;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = startScale * SineOut((Time.time - startTime) * (1 / duration));
    }

    float SineOut(float x)
    {
        x = Mathf.Clamp01(x);
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
}
