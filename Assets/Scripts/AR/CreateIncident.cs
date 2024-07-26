using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CreateIncident : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleUI, _descriptionUI;
    [SerializeField] private Incident _incidentPrefab;
    [SerializeField] private UpdateModelManager _updateModelManager;
    [SerializeField] private IncidentManager _incidentManager;
    [SerializeField] private UnityEvent _onCreateIncident;
    [SerializeField] private UnityEvent<Vector3, string, string> OnCreatedIncident;

    public void NewIncident()
    {
        Incident incident = Instantiate(_incidentPrefab, _updateModelManager.modelTarget.transform);
        incident.transform.localScale *= ModelGLTF.Instance.transform.lossyScale.x;
        incident.SetData(_titleUI.text, _descriptionUI.text);
        var transformTemp = incident.transform;
        transformTemp.position = _incidentManager.IncidentPosition;
        transformTemp.forward = - _incidentManager.IncidentNormal;
        OnCreatedIncident?.Invoke(_incidentManager.IncidentPosition, _titleUI.text, _descriptionUI.text);
        transformTemp.position = incident.transform.TransformPoint(0, 0, -.1f);
        var rotationTemp = transformTemp.eulerAngles;
        rotationTemp.x = 0;
        transformTemp.eulerAngles = rotationTemp;
        // transformTemp.rotation = Quaternion.Inverse(transformTemp.rotation);
        _incidentManager.AddIncident(incident);
        _onCreateIncident?.Invoke();
    }

    private Vector3 UbicateIncident()
    {
        return _incidentManager.IncidentPosition;
    }
}
