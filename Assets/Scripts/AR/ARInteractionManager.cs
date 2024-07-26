using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TouchPhase = UnityEngine.TouchPhase;


public class ARInteractionManager : MonoBehaviour
{
    //creando campos
    [SerializeField] private ARRaycastManager _aRRaycastManager;
    [SerializeField] private UpdateModelManager _updateModelManager;
    [SerializeField] private UnityEvent onPlaceModel;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>(); //recolectando los hit points
    private GameObject item3DModel;
    private bool isOverUI; //permitira verificar si el toque fue sobre la interfaz

    void LateUpdate()
    {

        if (Input.touchCount > 0) //verificar si ha habido un toque en la pantalla
        {
            Touch touchOne = Input.GetTouch(0); //crear variable que almacene el toque en pantalla que ha ocurrido
            if (touchOne.phase == TouchPhase.Began) //es decir cuando ha comenzado el touch
            {
                var positionScreen = Pointer.current.position.ReadValue();
                isOverUI = EventSystem.current.IsPointerOverGameObject();
                if (_aRRaycastManager.Raycast(positionScreen, _hits,
                        TrackableType
                            .Planes)) //para verificar si el toque ha sido dentro de los planos que se han detectado
                {
                    if (!isOverUI) //si el touch no ha sido en la interfaz
                    {
                        ModelGLTF.Instance.SeparateModel();
                        var instantiated = ModelGLTF.Instance.GetAssetUnloader().gameObject;
                        ModelGLTF.Instance.ReduceScale1to50();
                        ModelGLTF.Instance.InitialScale = instantiated.transform.localScale.x; 
                        ModelGLTF.Instance.EnableModelAsync();
                        instantiated.transform.position = _hits[0].pose.position;
                        ModelGLTF.Instance.InitialPosition = instantiated.transform.position;
                        instantiated.AddComponent<ARAnchor>();
                        _updateModelManager.modelTarget = instantiated;
                        // AddCollidersToObjects(instantiated);
                        onPlaceModel?.Invoke();
                    }
                }
            }
        }
    }
    private void AddCollidersToObjects(GameObject objectTemp)
    {
        // Busca todos los objetos en la escena con el componente de malla (MeshRenderer o MeshFilter).
        MeshRenderer[] meshRenderers = objectTemp.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            // Obtiene el GameObject que contiene el componente de malla.
            GameObject modelObject = meshRenderer.gameObject;

            // Comprueba si el objeto ya tiene un collider.
            if (modelObject.GetComponent<Collider>() == null)
            {
                // Agrega un collider al objeto con malla.
                modelObject.AddComponent<MeshCollider>();
            }
        }
    }
    public void DeleteItem()
    {
        Destroy(item3DModel);
    }

    public void ResetModel()
    {
        ModelGLTF.Instance.JoinModel();
        ModelGLTF.Instance.GetAssetUnloader().transform.position = Vector3.zero;
        Destroy(ModelGLTF.Instance.GetAssetUnloader().gameObject.GetComponent<ARAnchor>());
    }
}
