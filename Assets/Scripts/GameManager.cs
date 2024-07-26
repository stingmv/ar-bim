using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _onFinishPlaneDetection;
    [SerializeField] private UnityEvent _onFinishPutModel;
    [SerializeField] private ARPlaneManager _planeManager;
    [SerializeField] private ARTrackedImageManager _imageManager;

    [SerializeField] private UnityEvent _onNewEnvironment;
    [SerializeField] private UnityEvent _onLoadEnvironment;
    [SerializeField] private GameObject _buttonImageComponent;

    public Transform imageTransform;
    public static GameManager instance;
    private GameObject _objectSelected;

    public GameObject ObjectSelected
    {
        get => _objectSelected;
        set => _objectSelected = value;
    }
    private void Awake()
    {
        if(instance!=null&&instance!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        InitConfiguration();
        if (ModelGLTF.Instance.modelInfo.positioningCreated)
        {
            _buttonImageComponent.SetActive(true);
        }
    }

    public void FinishPlaneDetection()
    {
        _planeManager.requestedDetectionMode = PlaneDetectionMode.None;
        _onFinishPlaneDetection?.Invoke();
    }

    public void FinishPutModel()
    {
        _onFinishPutModel?.Invoke();
        DeleteTrackables();
    }

    private void DeleteTrackables()
    {
        foreach (var trackable in _planeManager.trackables)
        {
            Destroy(trackable.gameObject);
        }
    }

    public void NewEnvironment()
    {
        _planeManager.enabled = true;
        _onNewEnvironment?.Invoke();
    }

    public void LoadEnvironment()
    {
        _imageManager.enabled = true;
        _onLoadEnvironment?.Invoke();
    }

    public void InitConfiguration()
    {
        _imageManager.enabled = false;
        _planeManager.enabled = false;
    }
    // // Start is called before the first frame update
    // void Start()
    // {
    //     MainMenu();
    // }
    //
    // public void MainMenu()
    // {
    //     OnMainMenu?.Invoke();
    //     Debug.Log("Main Menu Activated");
    // }
    // public void ItemsMenu()
    // {
    //     OnItemsMenu?.Invoke();
    //     Debug.Log("Items Menu Activated");
    // }
    //
    // public void ARPosition()
    // {
    //     OnARPosition?.Invoke();
    //     Debug.Log("AR Position Activated");
    //
    // }
    public void CloseAPP()
    {
        Application.Quit();
    }

}
