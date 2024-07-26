using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SavePositioningModel : MonoBehaviour
{
    
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] private Button _buttonSave;
    [SerializeField] private SelectionUser _selectionUser;
    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UnityEvent _onFailed;
    
    private void OnEnable()
    {
        _arTrackedImageManager.enabled = true;
        _arTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrackedImagesChanged;
        if (GameManager.instance.imageTransform)
        {
            _buttonSave.gameObject.SetActive(true);
        }
        else
        {
            _buttonSave.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= ArTrackedImageManagerOntrackedImagesChanged;
    }

    private void ArTrackedImageManagerOntrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        Debug.Log("imagen detectada");
        for (int i = 0; i < obj.added.Count; i++)
        {
            GameManager.instance.imageTransform = obj.added[i].transform;
        }
        _buttonSave.gameObject.SetActive(true);
    }

    public void SavePositioning()
    {
        _buttonSave.interactable = false;
        var transformModel = ModelGLTF.Instance.root.transform;
        var localPosition = GameManager.instance.imageTransform.transform.InverseTransformPoint(transformModel.position);
        var localRotation = Quaternion.Inverse(transformModel.rotation ) * GameManager.instance.imageTransform.rotation;
        var scale = transformModel.lossyScale;
        ModelPositioning modelPositioning = new ModelPositioning
        {
            posX = localPosition.x,
            posY = localPosition.y,
            posZ = localPosition.z,
            rotX = localRotation.x,
            rotY = localRotation.y,
            rotZ = localRotation.z,
            rotW = localRotation.w,
            scaleX = scale.x,
            scaleY = scale.y,
            scaleZ = scale.z
        };
        StartCoroutine(SaveInDatabase(modelPositioning));
    }

    IEnumerator SaveInDatabase(ModelPositioning modelPositioning)
    {
        var modelInfo = ModelGLTF.Instance.modelInfo;
        modelInfo.posX = modelPositioning.posX;
        modelInfo.posY = modelPositioning.posY;
        modelInfo.posZ = modelPositioning.posZ;
        modelInfo.rotX = modelPositioning.rotX;
        modelInfo.rotY = modelPositioning.rotY;
        modelInfo.rotZ = modelPositioning.rotZ;
        modelInfo.rotW = modelPositioning.rotW;
        modelInfo.scaleX = modelPositioning.scaleX;
        modelInfo.scaleY = modelPositioning.scaleY;
        modelInfo.scaleZ = modelPositioning.scaleZ;
        var link = $"https://arbim.azurewebsites.net/api/User/modelos/{_selectionUser.modelSelectionIndex}/update";
        UnityWebRequest request = UnityWebRequest.Put(link, JsonUtility.ToJson(modelPositioning));
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.responseCode < 400)
        {
            _onSuccess?.Invoke();
            _buttonSave.interactable = true;
        }
        else
        {
            _onFailed?.Invoke();
        }
    }
}
