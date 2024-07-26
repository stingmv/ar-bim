using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropertySetItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _namePropertySet;
    [SerializeField] private RectTransform _propertiesContainer;
    [SerializeField] private PropertyItem _propertyItemPrefab;

    public void SetData(string propertyName)
    {
        _namePropertySet.text = propertyName;
    }

    public RectTransform PropertiesContainer
    {
        get => _propertiesContainer;
        set => _propertiesContainer = value;
    }

    public PropertyItem PropertyItemPrefab
    {
        get => _propertyItemPrefab;
        set => _propertyItemPrefab = value;
    }

    public TextMeshProUGUI NamePropertySet
    {
        get => _namePropertySet;
        set => _namePropertySet = value;
    }
}
