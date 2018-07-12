using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLogic : MonoBehaviour
{
    Vector3 lastMousePos;

    void TouchRotate()
    {
        if (Input.GetMouseButton(0))
        {
            float moveX = (Input.mousePosition - lastMousePos).x;
            transform.Rotate(0, -moveX, 0);
        }
    }

    void Update()
    {
        TouchRotate();
        lastMousePos = Input.mousePosition;
    }
}
