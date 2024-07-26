using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ViewCubeManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _views;
    [SerializeField] private Transform _canvasReference;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _viewCube;
    private float sign;
    private void Start()
    {
        StartCoroutine(CalculateViews());
    }

    private void Update()
    {
        _viewCube.transform.rotation = Quaternion.Inverse(_camera.transform.rotation);
    }

    IEnumerator CalculateViews()
    {
        while (true)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                sign = Vector3.Angle(_canvasReference.transform.forward, _views[i].transform.forward); 
                if (sign<90)
                {
                    // Mostrar cara
                    // Gizmos.color = Color.red;
                    _views[i].SetActive(true);
                }
                else
                {
                    // Ocultar cara
                    // Gizmos.color = Color.green;
                    _views[i].SetActive(false);
                }
            }
            yield return new WaitForSeconds(.03f);
        }
        
    }
}
