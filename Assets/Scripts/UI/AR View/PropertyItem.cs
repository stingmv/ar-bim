using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropertyItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _propertyName;

    private float _height;

    public float Height
    {
        get => _height;
        set => _height = value;
    }
    public void SetData(string name)
    {
        _propertyName.text = name; 
        // var rectTransform = _propertyName.GetComponent<RectTransform>();
        Height = _propertyName.rectTransform.rect.height;
        Debug.Log(Height);
    }
}
