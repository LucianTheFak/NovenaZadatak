using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GalleryPhotoData : MonoBehaviour
{
    public GalleryPhoto photo;
    public Image photoImage;

    private UnityWebRequest uwr;
    public static string GetFileLocation(string relativePath)
    {
        return "file://" + Path.Combine(Application.streamingAssetsPath, relativePath);
    }

    public IEnumerator UpdateGalleryItem()
    {
        using (uwr = UnityWebRequestTexture.GetTexture(GetFileLocation(photo.fileName + ".png")))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                photoImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
