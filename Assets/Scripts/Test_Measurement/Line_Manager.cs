using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;
using UnityEngine.UI;

public class Line_Manager : MonoBehaviour
{
    [SerializeField] GameObject guidePoint;
    [SerializeField] RayCast _rayCast;
    //[SerializeField] Button _button;
    [SerializeField] GameObject measurementBoxNotContinuous;//para almacenar medidas
    [SerializeField] GameObject measurementBoxContinuous;
    [SerializeField] TextMeshProUGUI measurementText;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] TextMeshPro mText;
    private int pointCount = 0;
    LineRenderer line;
    bool continuous = false;
    //public TextMeshProUGUI buttonText;
    TextMeshPro distText;


    void OnEnable()
    {
        EventManager.TouchEvent += DrawLine;
    }
    public void ToggleBetweenDiscreteAndContinuous()
    {
        continuous = !continuous;
        pointCount = 0;

        //if (continuous)
        //{
        //    _button.image.color = new Color(0.5f, 1f, 0.4f);
        //}
        //else
        //{
        //    _button.image.color = Color.white;
        //}
    }
    void DrawLine()
    {
        Vector3 normalGuide = _rayCast.hit.normal;
        Quaternion rotationGuide = Quaternion.LookRotation(normalGuide);
        GameObject _guidePoint = Instantiate(guidePoint, _rayCast.hit.point, rotationGuide);
        if (continuous)
        {
            _guidePoint.transform.parent = measurementBoxContinuous.transform;
        }
        else
        {
            _guidePoint.transform.parent = measurementBoxNotContinuous.transform;

        }

        //Debug.Log("Leido desde DrawLine:" + _rayCast.hit.point);
        pointCount++;

        if (pointCount < 2)//si pointCount es menor que 2
        {
            line = Instantiate(lineRenderer);

            if (continuous)
            {
                line.transform.parent = measurementBoxContinuous.transform;
            }
            else
            {
                line.transform.parent = measurementBoxNotContinuous.transform;
            }

            line.positionCount = 1;
        }

        else//si pointCount es mayor igual que 2
        {

            line.positionCount = pointCount;
            if (!continuous)//si la linea no es continua
            {
                pointCount = 0;
            }
        }

        line.SetPosition(index: line.positionCount - 1, _rayCast.hit.point);

        if (line.positionCount > 1)
        {
            Vector3 pointB = line.GetPosition(line.positionCount - 1);//ultima posicion
            //Debug.Log("pointB: " + pointB);
            Vector3 pointA = line.GetPosition(line.positionCount - 2);//penultima posicion
            //Debug.Log("pointA: " + pointA);
            float dist = Vector3.Distance(pointA, pointB);

            distText = Instantiate(mText);
            if (continuous)
            {
                distText.transform.SetParent(measurementBoxContinuous.transform, false);
            }
            else
            {
                distText.transform.SetParent(measurementBoxNotContinuous.transform, false);
            }
            distText.text = dist.ToString("F2") + "m";

            Vector3 directionVector = (pointB - pointA);
            //Debug.Log("Vector direccion: " + directionVector);
            Vector3 normal = _rayCast.hit.normal;
            //Debug.Log("normal: " + normal);

            Vector3 upd = Vector3.Cross(directionVector, normal).normalized;
            Debug.Log("Producto punto: " + Vector3.Dot(directionVector, normal));
            //Debug.Log("producto cruz normalizado: " + upd);
            Quaternion rotation = Quaternion.LookRotation(-normal, upd);

            distText.transform.rotation = rotation;
            distText.transform.position = (pointA + directionVector * 0.5f) + upd * 0.06f + normal * 0.004f;//se ubica a la mitad del vector direccion y se a�ade offsets en direccion de vectores normales

            measurementText.text = "Medición: " + distText.text;
        }

    }
    public void CleanAllMeasurements()
    {
        for (int i = 0; i < measurementBoxNotContinuous.transform.childCount; i++)
        {
            Destroy(measurementBoxNotContinuous.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < measurementBoxContinuous.transform.childCount; i++)
        {
            Destroy(measurementBoxContinuous.transform.GetChild(i).gameObject);
        }
        measurementText.text = "";
        pointCount = 0;
    }

    public void CleanLastMeasurement()
    {
        GameObject[] lastMeasurement;

        if (continuous)//SI ES CONTINUA
        {
            //-----ASIGNAR------
            lastMeasurement = new GameObject[measurementBoxContinuous.transform.childCount];
            for (int i = 0; i < measurementBoxContinuous.transform.childCount; i++)//asignar en array lastMeasurement
            {
                lastMeasurement[i] = measurementBoxContinuous.transform.GetChild(i).gameObject;
            }

            //---- ELIMINAR-----

            if (measurementBoxContinuous.transform.childCount == 4)//SI SOLO QUEDAN 4 HIJOS EN CAJA
            {
                for (int i = 0; i < measurementBoxContinuous.transform.childCount; i++)//eliminarlos
                {
                    Destroy(lastMeasurement[i]);
                }
                pointCount = 0;
            }
            else
            {
                if (measurementBoxContinuous.transform.childCount != 0)
                {
                    for (int i = measurementBoxContinuous.transform.childCount - 2; i < measurementBoxContinuous.transform.childCount; i++)//eliminar 2 ultimos hijos de measurementBoxContinuous
                    {
                        Destroy(lastMeasurement[i]);
                    }

                    //---Buscar el ultimo line renderer en box continuous----
                    LineRenderer[] lineBox = measurementBoxContinuous.GetComponentsInChildren<LineRenderer>();//obtener todos los linerenderer en boxcontinuous
                    LineRenderer lineMax = lineBox[0];//inicializar con primera posicion

                    for (int i = 0; i < lineBox.Length; i++)
                    {
                        if (lineBox[i].GetComponent<LineRenderer>())
                        {
                            lineMax = lineBox[i];//actualizar maximo
                        }
                    }
                    line = lineMax;//volver a asignar el line renderer continuo                    
                    line.positionCount--;
                }
            }

            //----VISUALIZACION DE ULTIMA MEDIDA EN UI-----            
            if (measurementBoxContinuous.transform.childCount != 4 && measurementBoxContinuous.transform.childCount != 0)
            {
                measurementText.text = "Medición: " + lastMeasurement[measurementBoxContinuous.transform.childCount - 3].GetComponent<TextMeshPro>().text;
            }
            else
            {
                measurementText.text = "";
            }

        }

        else//SI NO ES CONTINUA
        {

            //-----ASIGNAR------
            lastMeasurement = new GameObject[measurementBoxNotContinuous.transform.childCount];
            for (int i = 0; i < measurementBoxNotContinuous.transform.childCount; i++)
            {
                lastMeasurement[i] = measurementBoxNotContinuous.transform.GetChild(i).gameObject;
            }

            //------ELIMINAR-----
            if (measurementBoxNotContinuous.transform.childCount != 0)
            {
                for (int i = measurementBoxNotContinuous.transform.childCount - 4; i < measurementBoxNotContinuous.transform.childCount; i++)//eliminamos las ultima 4 hijos de measurementBoxNotContinuous
                {
                    Destroy(lastMeasurement[i]);
                }
            }

            //----VISUALIZACION DE ULTIMA MEDIDA EN UI-----            
            if (measurementBoxNotContinuous.transform.childCount != 4 && measurementBoxNotContinuous.transform.childCount != 0)//si el tamaño de measurementBoxNotContinuous no es 4, o sea no es la ultima tanda
            {
                measurementText.text = "Medición: " + lastMeasurement[measurementBoxNotContinuous.transform.childCount - 5].GetComponent<TextMeshPro>().text;//colocamos la ultima medida en lectura
            }
            else//si es 4 entonces vaciamos lectura
            {
                measurementText.text = "";

            }
        }
    }

    private void OnDisable()
    {
        EventManager.TouchEvent -= DrawLine;
    }

}
