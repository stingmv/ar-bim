using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideClipPlane : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private ClipPlane _clipPlane;
    
    private void Start()
    {
        _slider.minValue = ModelGLTF.Instance.root.transform.TransformPoint(ModelGLTF.Instance.bounds.min).y;
        _slider.maxValue = ModelGLTF.Instance.root.transform.TransformPoint(ModelGLTF.Instance.bounds.max).y;
        _slider.onValueChanged.AddListener(Call);
        // _slider.value = _slider.maxValue;
        // throw new NotImplementedException();
    }

    private void Call(float arg0)
    {
        // var transformPosition = _clipPlane.transform.position;
        // transformPosition.y = arg0;
        _clipPlane.Height = arg0;
    }
}
