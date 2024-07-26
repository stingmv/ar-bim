using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardManager : MonoBehaviour
{
    private RectTransform _inputFieldRectTransform;
    private TMP_InputField _inputField;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _rectRelative;
    [SerializeField] private RectTransform test;
    [SerializeField] private float _move;

    private RectTransform _rectTransformScroll;
    private float _keyboardHeight;
    private float _time;
    private float _timeWithoutKeyboard;
    private float _duration = .3f;
    private Vector3[] coorners = new Vector3[4];
    // private TouchScreenKeyboard _keyboard;
    private InputFieldManager _inputFieldManager;
    private void Start()
    {
// #if UNITY_ANDROID
//         TouchScreenKeyboard.Android.consumesOutsideTouches = true;
// #endif
        _rectTransformScroll = _scrollRect.GetComponent<RectTransform>();
        // TouchScreenKeyboard.hideInput = false;
    }

    public void SetInputField(TMP_InputField rectTransform)
    {
        _inputFieldRectTransform = rectTransform.GetComponent<RectTransform>();
        _inputFieldManager = rectTransform.GetComponent<InputFieldManager>();
        _inputField = rectTransform;
        // if (_keyboard != null)
        // {
        //     _keyboard.active = false;
        //     TouchScreenKeyboard.hideInput = true;
        // }
        // Invoke(nameof(GetKeyboard), .3f);
    }

    // public void GetKeyboard()
    // {
    //     if (!_inputFieldManager.IsEmail)
    //     {
    //         _keyboard = TouchScreenKeyboard.Open(_inputField.text, TouchScreenKeyboardType.Default, false, _inputField.multiLine, true,
    //             false, "", _inputField.characterLimit);
    //     }
    //     else
    //     {
    //         _keyboard = TouchScreenKeyboard.Open(_inputField.text, TouchScreenKeyboardType.EmailAddress);
    //     }
    //     _keyboard.text = _inputField.text;
    //     _inputField.onValueChanged.AddListener(Call);
    // }
    private void Call(string arg0)
    {
        _inputField.caretPosition = 0;
        if (_inputFieldManager.IsEmail)
        {
            _inputFieldManager.ComproveEmailFormat(arg0);
            return;
        }
        _inputFieldManager.ComprovePasswordFormat(arg0);
    }
// #if UNITY_ANDROID
//     private void LateUpdate()
//     {
//         if (_keyboard != null && _keyboard.status == TouchScreenKeyboard.Status.Visible)
//         {
//             _inputField.text = _keyboard.text;
//         }
//
//         
//     }
// #endif
    private void Update()
    {
        

        if (TouchScreenKeyboard.visible )
        {
            _timeWithoutKeyboard = 0;
            // _scrollRect.verticalNormalizedPosition = 0;
#if UNITY_ANDROID
            
            if (_keyboardHeight == 0)
            {
                _keyboardHeight = InputfieldSlideScreen.GetRelativeKeyboardHeight(_rectRelative, true);
            }
#elif UNITY_IOS
            _keyboardHeight = TouchScreenKeyboard.area.height;
#elif UNITY_EDITOR
            _keyboardHeight = _move;
#endif
            if( _time == 0)
            {
                _time = Time.time;
            }
            var actualOffset = Mathf.SmoothStep(0, _keyboardHeight, (Time.time - _time) / _duration);
            var positionTemp = new Vector2(_scrollRect.content.anchoredPosition.x, actualOffset);
            
            _rectTransformScroll.offsetMin = positionTemp;
            // _scrollRect.verticalNormalizedPosition += (Screen.height - (_keyboardHeight + _inputFieldRectTransform )/ Screen.height;
            // Debug.Log(_keyboardHeight + " " + _scrollRect.verticalNormalizedPosition + " " + _inputFieldRectTransform.position.y + " " + Screen.height + " " + Screen.safeArea.height + " " + Screen.safeArea.yMin + " " + Screen.safeArea.yMax);
            _rectTransformScroll.GetWorldCorners(coorners);
            var diff = coorners[0].y - (_inputFieldRectTransform.position.y - _inputFieldRectTransform.rect.height);
            if (diff >= 0)
            {
                _scrollRect.verticalNormalizedPosition -= Time.deltaTime;
            
            }
        }
        else
        {
            _time = 0;

            if( _timeWithoutKeyboard == 0)
            {
                _timeWithoutKeyboard = Time.time;
            }
            var actualOffset = Mathf.SmoothStep(_keyboardHeight, 0, (Time.time - _timeWithoutKeyboard) / _duration);
            var positionTemp = new Vector2(_scrollRect.content.anchoredPosition.x, actualOffset);
            _rectTransformScroll.offsetMin =positionTemp;

        }

    }
}
