using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;

public class ImageTrackedDetection : MonoBehaviour
{
    [SerializeField] private SelectionUser _selectionUser;
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] private GameObject _buttonImageDetected;
    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UpdateModelManager _updateModelManager;

    private int _idModel;
    private string _link;
    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += ArTrackedImageManagerOntrackedImagesChanged;
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
        _buttonImageDetected.SetActive(true);
    }

    public void LoadModel()
    {
        ModelGLTF.Instance.SeparateModel();
        var instantiated = ModelGLTF.Instance.GetAssetUnloader().gameObject;
        ModelGLTF.Instance.ReduceScale1to50();
        ModelGLTF.Instance.InitialScale = instantiated.transform.localScale.x; 
        ModelGLTF.Instance.EnableModelAsync();
        instantiated.transform.position = GameManager.instance.imageTransform.transform.TransformPoint(ModelGLTF.Instance.modelInfo.GetPosition());
        instantiated.transform.rotation = Quaternion.Inverse(ModelGLTF.Instance.modelInfo.GetRotation()) * GameManager.instance.imageTransform.transform.rotation;
        instantiated.transform.localScale = ModelGLTF.Instance.modelInfo.GetScale();
        ModelGLTF.Instance.InitialPosition = instantiated.transform.position;
        instantiated.AddComponent<ARAnchor>();
        _updateModelManager.modelTarget = instantiated;

        _onSuccess?.Invoke();
    }

    /*IEnumerator GetInforTransform()
    {
        UnityWebRequest request = UnityWebRequest.Get(_link);
        
        yield return 
    }*/ 
}
