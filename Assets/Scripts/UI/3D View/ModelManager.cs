using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    [SerializeField] private DataUser _dataUser;
    [SerializeField] private SelectionUser _selectionUser;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private bool _enableModelInStart;

    private void Start()
    {
        // var modelInfo = _dataUser.Users.Find(x => x.Id == _selectionUser.userSelectionIndex).Courses.Find(x => x.id == _selectionUser.courseSelectionIndex).Models.Find(x => x.Id == _selectionUser.modelSelectionIndex);
        // if (_title)
        // {
        //     _title.text = modelInfo.Name;
        // } 
        // if (_description)
        // {
        //     _description.text = modelInfo.Description;
        // }
        ModelGLTF.Instance.ResetScale();

        if (_enableModelInStart)
        {
            ModelGLTF.Instance.EnableModel();
        }
        else
        {
            ModelGLTF.Instance.DisableModel();
        }
    }

    public void IsIfcWall(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcWall = value;
    }
    public void IsIfcWindow(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcWindow = value;

    }
    public void IsIfcDoor(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcDoor = value;

    }
    public void IsIfcColumn(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcColumn = value;

    }
    public void IsIfcRailing(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcRailing = value;

    }
    public void IsIfcStair(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcStair = value;

    }
    public void IsIfcRoof(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcAnnotation = value;

    }

    public void IsIfcBuildingStorey(bool value)
    {
        ModelGLTF.Instance._loadBimData.IsIfcBuilding = value;
    }
}
