using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    [ContextMenu(nameof(OnClick))]
    public void OnClick()
    {
        _button.onClick.Invoke();
    }
}
