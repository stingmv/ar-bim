using Fusion;
using UnityEngine;

public class InstanceObject : NetworkBehaviour
{
    [SerializeField] private GameObject prefabForInstance;
    
    public void InstanceDynamic(Vector3 position, string textTitle, string textDescription)
    {
        Runner.Spawn(prefabForInstance, position, Quaternion.identity, Object.InputAuthority,
            (runner, o) =>
            {
                o.GetComponent<NoteObject>().TextTitleNote = textTitle;
                o.GetComponent<NoteObject>().TextDescriptionNote = textDescription;
            });
    }
}
