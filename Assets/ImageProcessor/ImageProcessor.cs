using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ImageProcessor
{
    private string api_url = "https://api.mathpix.com/v3/text";

    public bool updated = false;
    public string text;

    public IEnumerator PostImage(string imageData)
    {
        UnityWebRequest request = new UnityWebRequest(api_url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"src\": \"" + imageData + "\"}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("content-type", "application/json");
        request.SetRequestHeader("app_id", "pmao_uw_edu");
        request.SetRequestHeader("app_key", "619f6d7f1f034acafdeb");

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            MonoBehaviour.print(request.error);
        }
        else
        {
            updated = true;
            text = request.downloadHandler.text;
        }
    }
}
