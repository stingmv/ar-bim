using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModelItem : MonoBehaviour
{
    [SerializeField] private SelectionUser _selectionUser;
    [SerializeField] private TextMeshProUGUI _titleCourse;
    [SerializeField] private LoadImage _loadImage;
    [SerializeField] private UnityEvent<string> _onGetUrl;

    private int _id;
    private string _url;
    private Model model;

    public Model Model
    {
        get => model;
        set => model = value;
    }
    public LoadImage LoadImage => _loadImage;

    public void SetData(string title, int id, string url)
    {
        _titleCourse.text = title;
        _id = id;
        _url = url;
    }
    public void SetIdModel()
    {
        _selectionUser.modelSelectionIndex = _id;
        ModelGLTF.Instance.modelInfo = model;
    }

    public void GetUrlAndLoad()
    {
        _onGetUrl?.Invoke(_url);
    }
}
