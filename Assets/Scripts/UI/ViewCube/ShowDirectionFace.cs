using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDirectionFace : MonoBehaviour
{
    [SerializeField] private Canvas _canvasReference;
    public float sign;
    private void OnDrawGizmos()
    {
        if (_canvasReference)
        {
            Gizmos.DrawRay(_canvasReference.transform.position, _canvasReference.transform.forward);
            sign = Vector3.Angle(_canvasReference.transform.forward, transform.forward); 
            if (sign<90)
            {
                // Mostrar cara
                Gizmos.color = Color.red;
            }
            else
            {
                // Ocultar cara
                Gizmos.color = Color.green;
            }
            Gizmos.DrawRay(transform.position, transform.forward);

        }
    }
}
