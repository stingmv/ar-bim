using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCast : MonoBehaviour
{
    [SerializeField] GameObject centralPointer;
    public RaycastHit hit;
    int range = 20;

    private void Update()
    {
        centralPointer.SetActive(false);

        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, range))
        {
            centralPointer.transform.position = gameObject.transform.position + (gameObject.transform.forward).normalized * .4f;

            if (hit.collider)
            {
                centralPointer.SetActive(true);
                Vector3 normal = hit.normal;
                Quaternion rotation = Quaternion.LookRotation(normal);

                centralPointer.transform.rotation = rotation;
                //Debug.DrawRay(centralPointer.transform.position, hit.normal * range, Color.green);
                //if (Input.GetMouseButtonDown(0))
                //{
                //    Debug.Log("Colision fue en: " + hit.point);
                //}
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.forward * range);

    }


}
