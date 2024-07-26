using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AR.Constant;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LoginManager : MonoBehaviour
{
   [SerializeField] private DataUser _dataUser;
   [SerializeField] private SelectionUser _selectionUser;
   [SerializeField] private InputFieldManager _emailField;
   [SerializeField] private InputFieldManager _passwordField;
   [SerializeField] private Color _colorSuccess;
   [SerializeField] private Color _colorError;

   [SerializeField] private UnityEvent<Color> _onSuccessLogin;
   [SerializeField] private UnityEvent _haveMissingFields;
   [SerializeField] private UnityEvent<Color> _onFailedLogin;

   public bool ComprobeMissFields()
   {
      var emptyEmail = _emailField.HaveError;
      var emptyPassword = _passwordField.HaveError;
      return emptyEmail || emptyPassword;
   }

   public bool ComprobeUser()
   {
      //Logica para buscar usuario
      var email = _emailField.InputField.text;
      var password = _passwordField.InputField.text;
      var data = _dataUser.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
      Debug.Log(data.Id);
      if (data.Id > 0)
      {
         _selectionUser.userSelectionIndex = data.Id;
         Debug.Log("no nulo");
         return true;
      }
      _selectionUser.userSelectionIndex = 0;
         Debug.Log("nulo");
         return false;

   }

   public void Login()
   {
// #if UNITY_EDITOR
//       _selectionUser.userSelectionIndex = 1;
//       ModelGLTF.Instance.ActualProcess = Constants.ActualProcess.GetCourses;
//       ModelGLTF.Instance._haveProcessRunning = true;
//       _onSuccessLogin?.Invoke(_colorSuccess);
//       return;
// #endif
      if (ComprobeMissFields())
      {
         _haveMissingFields?.Invoke();
         return;
      }

      if (ComprobeUser())
      {
         _onSuccessLogin?.Invoke(_colorSuccess);
      }
      else
      {
         _onFailedLogin?.Invoke(_colorError);
      }
      
   }
}
