using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class LoadImage : MonoBehaviour
{
    [SerializeField] private UnityEvent<Texture2D> OnImageTextureGot;

    private IEnumerator GetData(string url)
    {
        string urlFromDataUser = url;
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(urlFromDataUser))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                yield return new WaitForSeconds(2f);
                StartCoroutine(GetData(url));
            }
            else
            {
                StartCoroutine(GetImage(request));
            }
        }
    }

    IEnumerator GetImage(UnityWebRequest request)
    {
        // Texture2D texture2D = ((DownloadHandlerTexture)request.downloadHandler).texture;
        Texture2D texture2D = DownloadHandlerTexture.GetContent(request);
        yield return null;
        OnImageTextureGot?.Invoke(texture2D);
    }

    public void GetImgeFromDataUserConfig(string url)
    {
        StartCoroutine(GetData(url));
    }
}
