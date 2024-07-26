using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBuildingStorey : MonoBehaviour
{
    
    private List<GameObject> _list;
    [SerializeField] private GameObject container;
    [SerializeField] private Toggle prefab;
    // [SerializeField] private ToggleGroup toggleGroup;
    private void Start()
    {
        _list = ModelGLTF.Instance._loadBimData.IfcBuildingStorey;
        for (int i = 0; i < _list.Count; i++)
        {
            var obj = Instantiate(prefab, container.transform);
            var bso = obj.GetComponent<BuildingStoreyObject>();
            bso.GO = _list[i].gameObject;
            bso.NameItem.text = _list[i].GetComponent<IFCData>().STEPName;
            // obj.group = toggleGroup;
        }
    }

    
}
