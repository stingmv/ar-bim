using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AR.Constant;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{        
    AsyncOperation _asyncOperation;
    [SerializeField] private DataUser _dataUser;
    [SerializeField] private SelectionUser _selectionUser;
    public static string SceneToLoad { get; set; }
    public float waitTime = 3;
    
    private void Start()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        StartCoroutine(ChargeAsyncScene());
        StartCoroutine(RunProcess());
    }

    private void Update()
    {
        waitTime -= Time.deltaTime;
        if (_asyncOperation != null)
        {
            Debug.Log(_asyncOperation.progress);

            if (_asyncOperation.progress >= 0.9f && waitTime <=0 && !ModelGLTF.Instance._haveProcessRunning)
            {
               
                _asyncOperation.allowSceneActivation = true;
            }
        }
    }

    IEnumerator RunProcess()
    {
        // UnityWebRequest request;
        // string link = "";
        //
        // switch (ModelGLTF.Instance.ActualProcess)
        // {
        //     /*case Constants.ActualProcess.None:
        //         ModelGLTF.Instance._haveProcessRunning = false;
        //         yield break;*/
        //     case Constants.ActualProcess.GetCourses:
        //         link = $"https://arbim.azurewebsites.net/api/User/{_selectionUser.userSelectionIndex}/cursos";
        //         break;
        //     case Constants.ActualProcess.GetModels:
        //         link = $"https://arbim.azurewebsites.net/api/User/{_selectionUser.userSelectionIndex}/cursos/{_selectionUser.courseSelectionIndex}/modelos";
        //         break;
        //     default:
        //         ModelGLTF.Instance._haveProcessRunning = false;
        //         yield break;
        // }
        // request = UnityWebRequest.Get(link);
        //
        // yield return request.SendWebRequest();
        // string jsonData = "";
        // if (request.responseCode < 400)
        // {
        //     jsonData = request.downloadHandler.text;
        //     switch (ModelGLTF.Instance.ActualProcess)
        //     {
        //         case Constants.ActualProcess.GetCourses:
        //             jsonData = "{\"courses\" : " + jsonData + "}";
        //             // Debug.Log(jsonData);
        //             _dataUser.Courses = JsonUtility.FromJson<DataUser.CourseContainer>(jsonData);
        //             break;
        //         case Constants.ActualProcess.GetModels:
        //             jsonData = "{\"models\" : " + jsonData + "}";
        //             // Debug.Log(jsonData);
        //             _dataUser.Models = JsonUtility.FromJson<DataUser.ModelContainer>(jsonData);
        //             break;
        //     }
        // }
        yield return null;
        ModelGLTF.Instance._haveProcessRunning = false;

    }
    IEnumerator ChargeAsyncScene()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);
        _asyncOperation.allowSceneActivation = false;
        yield return  _asyncOperation;
        Debug.Log("termino");

    }
}

