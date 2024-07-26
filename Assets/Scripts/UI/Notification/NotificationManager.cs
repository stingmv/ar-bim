using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _notificationText;
    [SerializeField] private Image _notificationIcon;
    [SerializeField] private Image _notificationContent;
    [SerializeField] private GameObject _contenedorText;
    
    [SerializeField] private UnityEvent _endNotification;
    
    public void ShowNotification(string notification)
    {
        CancelInvoke(nameof(ClearText));
        _contenedorText.SetActive(true);
        _notificationText.text = notification;
    }

    public void ChangeIconNotification(Sprite sprite)
    {
        _notificationIcon.sprite = sprite;
    }

    public void ChangeNotificationColor(Color color)
    {
        _notificationContent.color = color;
    }
    public void HideNotification()
    {
        ClearText();
    }
    public void ShowNotificationAndHideWithDelay(string notification)
    {
        CancelInvoke(nameof(ClearText));
        _contenedorText.SetActive(true);
        _notificationText.text = notification;
        Invoke(nameof(ClearText), 3f);
    }

    public void ClearText()
    {
        _contenedorText.SetActive(false);
        _notificationText.text = "";
        _endNotification?.Invoke();
    }
}