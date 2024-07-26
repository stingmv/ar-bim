using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public float _timeScale;

    private void Update()
    {
        Time.timeScale = _timeScale;
    }
}
