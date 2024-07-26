using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

public class UpdateModelManager : MonoBehaviour
{
    [SerializeField] private float _moveFactorDrag = 50;
    [SerializeField] private float _scaleFactorDrag = 50;

    public GameObject modelTarget;
    public void UpdateXMove(float moveValue)
    {
        modelTarget.transform.position += modelTarget.transform.right * moveValue * Time.deltaTime / _moveFactorDrag;
    }
    public void UpdateYMove(float moveValue)
    {
        modelTarget.transform.position += modelTarget.transform.up * moveValue * Time.deltaTime / _moveFactorDrag;

    }
    public void UpdateZMove(float moveValue)
    {
        modelTarget.transform.position += modelTarget.transform.forward * moveValue * Time.deltaTime / _moveFactorDrag;
    }
    public void RotateXAxis(float valueRotation)
    {
        modelTarget.transform.RotateAround(modelTarget.transform.position, modelTarget.transform.right, valueRotation * Time.deltaTime);
    }
    public void RotateYAxis(float valueRotation)
    {
        modelTarget.transform.RotateAround(modelTarget.transform.position, modelTarget.transform.up, valueRotation * Time.deltaTime);
    }
    public void RotateZAxis(float valueRotation)
    {
        modelTarget.transform.RotateAround(modelTarget.transform.position, modelTarget.transform.forward, valueRotation * Time.deltaTime);
    }
    public void Scale(float valueScale)
    {
        var targetScale = modelTarget.transform.localScale.x + valueScale * Time.deltaTime * _scaleFactorDrag;
        var clampScale = Mathf.Clamp(targetScale, .001f, targetScale);
        modelTarget.transform.localScale =  Vector3.one * clampScale;
    }

    public void DisableAnchor()
    {
        if ( modelTarget.TryGetComponent(typeof(ARAnchor), out var response))
        {
            Debug.Log("arancohr");
            Destroy(response);
        }
    }

    public void ResetScale()
    {
        modelTarget.transform.localScale = Vector3.one * ModelGLTF.Instance.InitialScale;
        modelTarget.transform.position =  ModelGLTF.Instance.InitialPosition;
    }
        
    public void ToRealScale()
    {
        modelTarget.transform.localScale = Vector3.one;
    }

    public void EnableAnchor()
    {
        if (!modelTarget.TryGetComponent(typeof(ARAnchor), out var response))
        {
            modelTarget.AddComponent<ARAnchor>();
        }
    }
}
