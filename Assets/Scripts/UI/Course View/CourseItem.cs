using System.Collections;
using System.Collections.Generic;
using AR.Constant;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CourseItem : MonoBehaviour
{
    [SerializeField] private SelectionUser _selectionUser;
    [SerializeField] private TextMeshProUGUI _titleCourse;
    [SerializeField] private LoadImage _loadImage;
    private int _id;

    public LoadImage LoadImage => _loadImage;
    public void SetData(Sprite sprite, string title, int id)
    {
        _titleCourse.text = title;
        _id = id;

    }
    public void SetIdCourse()
    {
        _selectionUser.courseSelectionIndex = _id;
    }

    public void RunProcess()
    {
        ModelGLTF.Instance._haveProcessRunning = true;
        ModelGLTF.Instance.ActualProcess = Constants.ActualProcess.GetModels;
        ModelGLTF.Instance.userConfig.courseName = _titleCourse.text;
    }
}
