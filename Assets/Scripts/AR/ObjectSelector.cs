using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ObjectSelector : MonoBehaviour
{
    
    [SerializeField] private Camera _camera;
    [SerializeField] private UnityEvent<Vector3, Vector3> _onSelectedObject;
    [SerializeField] private UnityEvent _onUnselectedObject;
    [SerializeField] private Material _materialToChange;
    private MeshRenderer selectedObjectRender;
    private Material _oldMaterial;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            // Lanza un rayo desde la posición del mouse en la escena
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Realiza una intersección con el modelo 3D
            if (Physics.Raycast(ray, out hit))
            {
                if (selectedObjectRender)
                {
                    ResetMaterial();
                }
                // Obtiene el objeto colisionado (objeto del modelo 3D)
                var selectedObject = hit.transform.gameObject; // _onSelectedObject?.Invoke(selectedObject, hit.point);
                selectedObjectRender = selectedObject.GetComponent<MeshRenderer>();
                _oldMaterial = selectedObjectRender.sharedMaterial;
                selectedObjectRender.sharedMaterial = _materialToChange;
                GameManager.instance.ObjectSelected = selectedObject;

                _onSelectedObject?.Invoke(hit.point, hit.normal);
                // Imprime el nombre del objeto seleccionado
            }
            else
            {
                GameManager.instance.ObjectSelected = null;
                _onUnselectedObject?.Invoke();
            }
        }
    }

    public void ResetMaterial()
    {
        if (selectedObjectRender)
        {
            selectedObjectRender.sharedMaterial = _oldMaterial;

        }
    }
    
}
