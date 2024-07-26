using System;
using System.Collections;
using System.Collections.Generic;
using AR.Constant;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldManager : MonoBehaviour
{
    #region serializeFields variables

    [SerializeField] private Sprite _spriteError;
    [SerializeField] private Sprite _spriteNormal;
    [SerializeField] private Sprite _spriteNormalSelect;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _erorNotificaation;
    [SerializeField] private string _placeholderTextDefault;
    [SerializeField] private bool _isEmail;
    #endregion

    #region private variables

    private TMP_InputField _inputField;
    private TextMeshProUGUI _placeholderText;
    private bool haveError;
    #endregion

    #region public variables

    public bool IsEmail
    {
        get => _isEmail;
        set => _isEmail = value;
    }

    public TMP_InputField InputField
    {
        get => _inputField;
        set => _inputField = value;
    }

    public bool HaveError
    {
        get => haveError;
        set => haveError = value;
    }

    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
        _placeholderText = _inputField.placeholder.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region public Methods

    public void CheckNullField()
    {
        if (_inputField.text == "")
        {
            // Dejo campos vacios
            SetErrorVisual();
            _placeholderText.text = _placeholderTextDefault;
            _label.gameObject.SetActive(false);
            _erorNotificaation.gameObject.SetActive(true);
            if (_isEmail)
            {
                _erorNotificaation.text = Constants.emailFieldEmpty;
            }
            else
            {
                _erorNotificaation.text = Constants.passwordFieldEmpty;
            }
        }
        else
        {
            if (!haveError)
            {
                _erorNotificaation.gameObject.SetActive(false);
                _inputField.image.sprite = _spriteNormal;
            }
            
        }
    }

    public void ShowLabel()
    {
        _label.gameObject.SetActive(true);
        _placeholderText.text = "";
    }
    public void ResetColorText()
    {
        _inputField.textComponent.color = Color.black;
        _placeholderText.color = Color.black;
    }

    public void ComprovePasswordFormat(string textToComporove)
    {
        if (!IsEmptyField(textToComporove))
        {
            SetNormalVisual();
            _erorNotificaation.gameObject.SetActive(false);
            haveError = false;

        }
    }

    public bool IsEmptyField(string textToComporove)
    {
        if (textToComporove.Length == 0)
        {
            SetErrorVisual();
            _erorNotificaation.gameObject.SetActive(true);
            if (_isEmail)
            {
                _erorNotificaation.text = Constants.emailFieldEmpty;
            }
            else
            {
                _erorNotificaation.text = Constants.passwordFieldEmpty;
            }
            return true;
        }
        return false;
    }
    public void ComproveEmailFormat(string textToComporove)
    {
        IsEmptyField(textToComporove);
        var indexAtSign = textToComporove.IndexOf("@", StringComparison.Ordinal);
        if (indexAtSign != -1 && indexAtSign != 0 && indexAtSign != textToComporove.Length -1)
        {
            SetNormalVisual();
            _erorNotificaation.gameObject.SetActive(false);
            haveError = false;
        }
        else
        {
            haveError = true;
            SetErrorVisual();
            _erorNotificaation.text = Constants.errorFormatEmailField;
            _erorNotificaation.gameObject.SetActive(true);
        }
    }

    public void SetErrorVisual()
    {
        SpriteState spriteState = new SpriteState()
        {
            selectedSprite = _spriteError,
        };
        _inputField.spriteState = spriteState;
        _inputField.image.sprite = _spriteError;
        _inputField.textComponent.color = Color.red;
        _placeholderText.color = Color.red;
        _label.color = Color.red;
        _erorNotificaation.color = Color.red;
    }

    public void SetNormalVisual()
    {
        SpriteState spriteState = new SpriteState()
        {
            selectedSprite = _spriteNormalSelect,
        };
        _inputField.spriteState = spriteState;
        _inputField.image.sprite = _spriteNormal;
        _inputField.textComponent.color = Color.black;
        _label.color = Color.black;
    }
    #endregion

    #region private Methods
    
    
        
    #endregion
}
