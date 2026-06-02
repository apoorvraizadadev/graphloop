using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class MouseHelper
    {
        static Camera mainCamera;

        public static Vector2 MousePos()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            return mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        public static RaycastHit2D ObjectAtMouse()
        {
            return Physics2D.Raycast(MousePos(), Vector2.zero);
        }

        public static RaycastHit2D ObjectAtMouse(LayerMask layerMask)
        {
            return Physics2D.Raycast(MousePos(), Vector2.zero, 100f, layerMask);
        }
    }
}