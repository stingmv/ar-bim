using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Incident : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleUI;
    [SerializeField] private TextMeshProUGUI _descriptionUI;

    private string _title;
    private string _description;

    public void SetData(string title, string description)
    {
        _title = title;
        _description = description;
        _titleUI.text = _title;
        _descriptionUI.text = _description;
    }
}
