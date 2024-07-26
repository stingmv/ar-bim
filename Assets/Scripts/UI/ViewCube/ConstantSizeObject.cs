using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSizeObject : MonoBehaviour
{
    public Camera mainCamera; // Referencia a la cámara que verá el objeto

    public float constantSize = 1.0f; // Tamaño constante del objeto

    private void Update()
    {
        // Calcular la distancia del objeto a la cámara
        float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        // Calcular la escala que se debe aplicar al objeto para mantener su tamaño constante
        float scaleFactor = constantSize / distanceToCamera;

        // Aplicar la escala al objeto
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        // transform.localScale =Vector3.one * distanceToCamera * constantSize;
    }
}
