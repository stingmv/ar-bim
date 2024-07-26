using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncidentManager : MonoBehaviour
{
    [SerializeField] private List<Incident> _incidents;
    [SerializeField] private GameObject actualObjectAssociated;
    [SerializeField] private Vector3 _incidentPosition;
    [SerializeField] private Vector3 _incidentNormal;

    public Vector3 IncidentPosition
    {
        get => _incidentPosition;
        set => _incidentPosition = value;
    }

    public Vector3 IncidentNormal
    {
        get => _incidentNormal;
        set => _incidentNormal = value;
    }
    public GameObject ActualObjectAssociated
    {
        get => actualObjectAssociated;
        set
        {
            if (actualObjectAssociated)
            {
                actualObjectAssociated = value;
            }
            else
            {
                actualObjectAssociated = null;
            }
            
        } 
    }

    public void ResetActualObject()
    {
        actualObjectAssociated = null;
    }
    public void SetNormalAndPosition(Vector3 position, Vector3 normal)
    {
        IncidentPosition = position;
        IncidentNormal = normal;
    }
    public void AddIncident(Incident _incident)
    {
        _incidents.Add(_incident);
    }
}
