using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceInstanceObject : MonoBehaviour
{
    public InstanceObject InstanceObjectLocal;

    public void Instance(Vector3 position, string textTitle, string textDescription)
    {
        InstanceObjectLocal.InstanceDynamic(position, textTitle, textDescription);
    }
}
