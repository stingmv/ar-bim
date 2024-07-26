using Fusion;
using UnityEngine;

public class FollowXROrigin : NetworkBehaviour
{
    private Transform _xrOrigin;
    private Transform _xrOriginCamera;
    private InstanceObject _instanceObject;
    
    public override void Spawned()
    {
        CheckAuthority();
        SetReferenceInstanceObject();
    }

    private void CheckAuthority()
    {
        if (!HasInputAuthority)
            return;
        _xrOrigin = GameObject.Find("XR Origin").transform;
        _xrOriginCamera = _xrOrigin.GetComponentInChildren<Camera>().transform;
    }

    private void SetReferenceInstanceObject()
    {
        if (!HasInputAuthority)
            return;
        _instanceObject = GetComponent<InstanceObject>();
        _xrOrigin.GetComponent<ReferenceInstanceObject>().InstanceObjectLocal = _instanceObject;
    }

    public override void FixedUpdateNetwork()
    {
        Follow();
    }

    private void Follow()
    {
        if (!HasInputAuthority)
            return;
        transform.position = _xrOriginCamera.position;
        transform.rotation = _xrOriginCamera.rotation;
        
    }

}
