using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MouseHelper;

public class CameraController : MonoBehaviour
{
    public LayerMask nodeLayerMask;
    public float zoomSpeed;
    public float zoomSmoothDampTime;
    public Vector2 cameraLimit;

    Vector2 mouseStartPos = Vector2.one * 20;
    float targetZoom;
    float zoomSmoothDampVelocity;

    public Action onPan;
    public Action onZoom;

    [HideInInspector] public bool inDrag = false;

    void Start()
    {
        targetZoom = GetComponent<Camera>().orthographicSize;
    }

    void Update()
    {
        Pan();
        Zoom();
    }

    public void Pan()
    {
        if (Input.GetMouseButtonDown(0) && ObjectAtMouse(nodeLayerMask) == false)
        {
            inDrag = true;
            mouseStartPos = MousePos();
        }

        if (Input.GetMouseButton(0) && inDrag)
        {
            Vector2 difference = mouseStartPos - MousePos();
            transform.position += (Vector3)difference;
        }

        if (Input.GetMouseButtonUp(0) && inDrag)
        {
            Vector2 difference = mouseStartPos - MousePos();
            transform.position += (Vector3)difference;
            inDrag = false;
            if (onPan != null)
            {
                onPan();
            }
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -cameraLimit.x, cameraLimit.x), Mathf.Clamp(transform.position.y, -cameraLimit.y, cameraLimit.y), transform.position.z);
    }

    public void Zoom()
    {
        Camera camera = GetComponent<Camera>();
        targetZoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * ZoomSpeed();
        targetZoom = Mathf.Clamp(targetZoom, 1, 20);
        camera.orthographicSize = Mathf.SmoothDamp(camera.orthographicSize, targetZoom, ref zoomSmoothDampVelocity, zoomSmoothDampTime);

        if (onZoom != null && Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            onZoom();
        }
    }

    public float ZoomSpeed()
    {
        return Mathf.Min(Mathf.Pow(1.7f, targetZoom), 5);
    }
}
