using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionObjectInCameraCorner : MonoBehaviour
{
    public RectTransform uiObject; // Referencia al objeto UI que utilizaremos para la posición
    public Transform objectToPosition; // Referencia al objeto que queremos posicionar en World Space
    public Camera camara;
    public Vector3 worldPosition;

    private void Update()
    {
        // Posicionar el objeto en World Space basado en la posición del objeto UI
        PositionObject();
    }

    private void PositionObject()
    {
        // Obtener la posición del objeto UI en la pantalla
        Vector3 uiPosition = uiObject.position ;

        // Convertir la posición del objeto UI a World Space
        worldPosition = camara.ScreenToWorldPoint(uiPosition);

        // Asignar la posición calculada al objeto que queremos posicionar
        // objectToPosition.position = worldPosition;
        objectToPosition.position = uiPosition;
    }
}
