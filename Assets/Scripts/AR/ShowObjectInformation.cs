using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowObjectInformation : MonoBehaviour
{
    [SerializeField] private RectTransform _propertiesContainer;
    [SerializeField] private RectTransform _section1;
    [SerializeField] private RectTransform _section2;
    [SerializeField] private RectTransform _contentBody;
    [SerializeField] private PropertySetItem _prefabPropertieSetPrefab;

    [SerializeField] private TextMeshProUGUI _typeValue;
    [SerializeField] private TextMeshProUGUI _nameValue;
    [SerializeField] private TextMeshProUGUI _idValue;
    [SerializeField] private TextMeshProUGUI _layerValue;

    private List<GameObject> objs = new List<GameObject>();
    public void GetInformation()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i].gameObject);
        }

        var info = GameManager.instance.ObjectSelected.GetComponent<IFCData>();
        if (info.IFCClass != null)
        {
            _typeValue.text = info.IFCClass;
        }
        if (info.STEPName != null)
        {
            _nameValue.text = info.STEPName;
        }

        if (info.ObjectType != null)
        {
            _idValue.text = info.STEPId;
        }
        if (info.IFCLayer != null)
        {
            _layerValue.text = info.IFCLayer;
        }

        var heighPropertiesContainer = 0f;
        for (int i = 0; i < info.propertySets.Count; i++)
        {
            var ps = info.propertySets[i];
            var psp =Instantiate(_prefabPropertieSetPrefab, _propertiesContainer);
            objs.Add(psp.gameObject);
            psp.SetData(ps.propSetName);
            var height = 0f;
            for (int j = 0; j < ps.properties.Count; j++)
            {
                var pip = Instantiate(psp.PropertyItemPrefab, psp.PropertiesContainer);
                pip.SetData($"{ps.properties[j].propName} : {ps.properties[j].propValue}");
                height += pip.Height + 5;
            }
            var rectTransform = psp.PropertiesContainer;
            var rect =rectTransform.sizeDelta;
            rect.y = height;
            rectTransform.sizeDelta  = rect;

            rectTransform = psp.GetComponent<RectTransform>();
            rect = rectTransform.sizeDelta;
            rect.y = psp.NamePropertySet.rectTransform.rect.height + height;
            rectTransform.sizeDelta  = rect;
            heighPropertiesContainer += rect.y + 5;
        }
        var propertiesContainerSizeDelta = _propertiesContainer.sizeDelta;
        propertiesContainerSizeDelta.y = heighPropertiesContainer;
        _propertiesContainer.sizeDelta = propertiesContainerSizeDelta;
        
        propertiesContainerSizeDelta = _section2.sizeDelta;
        propertiesContainerSizeDelta.y = heighPropertiesContainer + 46.0299f;
        _section2.sizeDelta = propertiesContainerSizeDelta;

        var contentBodySizeDelta = _contentBody.sizeDelta;
        contentBodySizeDelta.y = _section2.rect.height + _section1.rect.height + 12;
        _contentBody.sizeDelta = contentBodySizeDelta;
    }
}
