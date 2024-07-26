using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject _viewDetectPlane;
    [SerializeField] private GameObject _viewTapToPlace;
    [SerializeField] private GameObject _viewMain;
    [SerializeField] private Canvas _canvasIncident;
    [SerializeField] private GameObject _viewIncidentButton;
    [SerializeField] private GameObject _viewIncident;

    // Start is called before the first frame update
    void Start()//crear funciones que van a estar suscritos a los eventos de nuestro gameManager
    {
        
    }

    public void CloseDetectPlaneView()
    {
        _viewDetectPlane.SetActive(false);
    }

    public void OpenDetectPlaneView()
    {
        _viewDetectPlane.SetActive(true);
    }

    public void CloseTapToPlaceView()
    {
        _viewTapToPlace.SetActive(false);
    }

    public void OpenTapToPlaceView()
    {
        _viewTapToPlace.SetActive(true);
    }
    public void CloseMainView()
    {
        _viewMain.SetActive(false);
    }

    public void OpenMainView()
    {
        _viewMain.SetActive(true);
    }

    public void OpenIncidentButtonView()
    {
        _canvasIncident.gameObject.SetActive(true);
        _viewIncidentButton.SetActive(true);
    }

    public void CloseIncidentButtonView()
    {
        _canvasIncident.gameObject.SetActive(false);
        _viewIncidentButton.SetActive(false);

    }

    public void OpenIncidentView()
    {
        _canvasIncident.gameObject.SetActive(true);
        _viewIncident.SetActive(true);

    }

    public void CloseIncidentView()
    {
        _canvasIncident.gameObject.SetActive(false);
        _viewIncident.SetActive(false);

    }
}
