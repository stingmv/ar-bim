using UnityEngine;
using UnityEngine.UI;

public class KeyboardUIHandler : MonoBehaviour
{
    public RectTransform inputFieldRect; // Arrastra y suelta el RectTransform del InputField aquí
    public RectTransform objectKeyboard; // Arrastra y suelta el RectTransform del botón de enviar aquí
    public Canvas canvas; // Arrastra y suelta el RectTransform del botón de enviar aquí
    public ScrollRect scrollView;
    public float height;
    
    private float originalScrollViewHeight;
    public float yOffset;
    public float contentPosY;
    private RectTransform _rectTransformScroll;
    public RectTransform _rectRelative;
    private float newScrollViewHeight;
    private float keyboardHeight;
    
    private float _duration = 1;
    public float time;
    private void Start()
    {
        originalScrollViewHeight = scrollView.GetComponent<RectTransform>().sizeDelta.y;
        _rectTransformScroll = scrollView.GetComponent<RectTransform>();
        TouchScreenKeyboard.Android.consumesOutsideTouches = true;
        // TouchScreenKeyboard.isInPlaceEditingAllowed = true;
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (TouchScreenKeyboard.visible)
        {
            scrollView.verticalNormalizedPosition = 0;
            keyboardHeight = InputfieldSlideScreen.GetRelativeKeyboardHeight(_rectRelative, true);
            if( time == 0)
            {
                time = Time.time;
            }
#elif UNITY_EDITOR
            float keyboardHeight = height;
#endif
            newScrollViewHeight = Screen.height - keyboardHeight;

            yOffset = (inputFieldRect.sizeDelta.y) * 0.5f;

            var actualOffset = Mathf.SmoothStep(0, keyboardHeight, (Time.time - time) / _duration);
            var positionTemp = new Vector2(scrollView.content.anchoredPosition.x, actualOffset);
            _rectTransformScroll.offsetMin = positionTemp;

#if UNITY_ANDROID
        }
        else
        {
            time = 0;
            _rectTransformScroll.offsetMin =
                new Vector2(scrollView.content.anchoredPosition.x, 0);
        }
#endif
    }
}
