using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoursesManager : MonoBehaviour
{
    [SerializeField] private DataUser _dataUser;
    [SerializeField] private SelectionUser _selectionUser;
    [SerializeField] private CourseItem _coursePrefab;
    [SerializeField] private RectTransform _container;

    public void Start()
    {
        Debug.Log("Start Course Manager");
        // var user = _dataUser.Users.Find(x => x.Id == _selectionUser.userSelectionIndex);
        /*foreach (var dataUserUser in _dataUser.Users)
        {
            Debug.Log(dataUserUser.Name);
        }*/
        for (int i = 0; i < _dataUser.Courses.courses.Count; i++)
        {
            var course = Instantiate(_coursePrefab, _container);
            course.SetData(null,_dataUser.Courses.courses[i].name, _dataUser.Courses.courses[i].id);
            course.LoadImage.GetImgeFromDataUserConfig(_dataUser.Courses.courses[i].image);
        }
    }
}
