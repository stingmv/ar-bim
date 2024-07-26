using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary>
/// Load scene using name, or reload the active scene
/// </summary>
public class LoadScene : MonoBehaviour
{
    private AsyncOperation loadingOperation;
    [SerializeField] private bool _disableModelExisted;
    [SerializeField] private UnityEvent _onBeforeLoadScene;
    private bool _initLoadAsync;
    public void LoadSceneUsingName(string sceneName)
    {
        if (_disableModelExisted)
        {
            ModelGLTF.Instance.DisableModel();
        }
        _onBeforeLoadScene?.Invoke();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void LoadSceneAscync(string sceneName)
    {
        if (_disableModelExisted)
        {
            ModelGLTF.Instance.DisableModel();
        }
        _onBeforeLoadScene?.Invoke();

        LoadingScreen.SceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingScene");

        // loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        // loadingOperation.allowSceneActivation = false;
        // _initLoadAsync = true;
    }

    // private void Update()
    // {
    //     if (!_initLoadAsync)
    //     {
    //         return;
    //     }
    //     if (loadingOperation.progress >= .9f )
    //     {
    //         loadingOperation.allowSceneActivation = true;
    //     }
    // }
}
