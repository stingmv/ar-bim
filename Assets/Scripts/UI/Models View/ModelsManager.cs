using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AR.Constant;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
[Serializable]
public class Root
{
    public List<Model> models;
}
public class ModelsManager : MonoBehaviour
{
    [SerializeField] private DataUser _dataUser;
    [SerializeField] private SelectionUser _selectionUser;
    [SerializeField] private ModelItem _modelItem;
    [SerializeField] private RectTransform _containerMyModels;
    [SerializeField] private RectTransform _containerCourseModels;
    [SerializeField] private TextMeshProUGUI _title;

    private string _link;
    IEnumerator Start()
    {
        ModelGLTF.Instance.Delete3DModel();
        ModelGLTF.Instance.objs.Clear();
        _title.text = _dataUser.courseName;
        var models_list=_dataUser.Models.models.Where(x => x.courseId == _selectionUser.courseSelectionIndex && x.userId == _selectionUser.userSelectionIndex);

        foreach ( var model in models_list)
        {
            var course = Instantiate(_modelItem, _containerMyModels);
            course.SetData(model.name, model.id, model.link);
            course.LoadImage.GetImgeFromDataUserConfig(model.image);
            course.Model = model;
            course = Instantiate(_modelItem, _containerCourseModels);
            course.SetData(model.name, model.id, model.link);
            course.LoadImage.GetImgeFromDataUserConfig(model.image);
            course.Model = model;

        }


        yield return null;
    }

    public void ReturnToCoursesView()
    {
        ModelGLTF.Instance._haveProcessRunning = true;
        ModelGLTF.Instance.ActualProcess = Constants.ActualProcess.GetCourses;
    }
}
