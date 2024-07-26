using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
public class EventManager : MonoBehaviour
{
    public static event Action TouchEvent;
    Touch touchDetected;

    void Update()
    {
        if (Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            touchDetected = Input.GetTouch(0);

            if (touchDetected.phase == TouchPhase.Began)
            {
                TouchEvent?.Invoke();
            }

        }


    }
}
