using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "SelectionUser", menuName = "ScriptableObjects/SelectionUser")]
public class SelectionUser : ScriptableObject
{
    public int userSelectionIndex;
    public int courseSelectionIndex;
    public int modelSelectionIndex;
}
