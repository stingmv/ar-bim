using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingStoreyObject : MonoBehaviour
{
    [SerializeField] private GameObject go;
    [SerializeField] private TextMeshProUGUI _nameItem;
    [SerializeField] private Toggle _toggle;

    private void Start()
    {
        _toggle.isOn = go.activeInHierarchy;
    }

    public TextMeshProUGUI NameItem
    {
        get => _nameItem;
        set => _nameItem = value;
    }
    public GameObject GO
    {
        get => go;
        set => go = value;
    }

    public void SetBool(bool value)
    {
        go.SetActive(value);
    }
}
