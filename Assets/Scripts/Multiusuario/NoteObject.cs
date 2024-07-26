using Fusion;
using TMPro;
using UnityEngine;

public class NoteObject : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI textTitleNoteTMP;
    [SerializeField] private TextMeshProUGUI textDescriptionNoteTMP;
    [Networked(OnChanged = nameof(TextTitleNoteChanged))] public NetworkString<_512> TextTitleNote { get; set; }
    [Networked(OnChanged = nameof(TextDecriptionNoteChanged))] public NetworkString<_512> TextDescriptionNote { get; set; }
    
    private static void TextTitleNoteChanged(Changed<NoteObject> changed)
    {
        changed.Behaviour.textTitleNoteTMP.SetText(changed.Behaviour.TextTitleNote.ToString());
    }
    
    private static void TextDecriptionNoteChanged(Changed<NoteObject> changed)
    {
        changed.Behaviour.textDescriptionNoteTMP.SetText(changed.Behaviour.TextDescriptionNote.ToString());
    }
}
