using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    public GameObject galleryScreen;
    public GameObject graditeljScreen;
    public TextAsset photoDataJSON;
    public Image graditeljImage;
    public Text graditeljText;
    public List<GalleryPhotoData> galleryPhotoDatas;
    public GameObject galleryMenu;
    public GameObject imageToFollow;
    public Image galleryMenuPhoto;
    public Text galleryMenuText;
    public Image ZoomedInPhoto;

    private void Awake()
    {
        MainPhoto mainPhoto = JsonUtility.FromJson<MainPhoto>(photoDataJSON.text);
        StartCoroutine(ChangeMainImage(mainPhoto.mainFileName));
        graditeljText.text = mainPhoto.mainDescription;
        GalleryPhotos galleryPhotosInJson = JsonUtility.FromJson<GalleryPhotos>(photoDataJSON.text);
        foreach (GalleryPhoto photo in galleryPhotosInJson.galleryPhotos)
        {
            Debug.Log("File Name: " + photo.fileName + "  Photo Name: " + photo.photoName);
        }
        for (int i = 0; i < galleryPhotosInJson.galleryPhotos.Length; i++)
        {
            galleryPhotoDatas[i].photo = galleryPhotosInJson.galleryPhotos[i];
            galleryPhotoDatas[i].StartCoroutine(galleryPhotoDatas[i].UpdateGalleryItem());
        }
    }

    void Start()
    {
        galleryScreen.SetActive(false);
        graditeljScreen.SetActive(false);
    }

    void Update()
    {
        if (galleryMenu.activeInHierarchy)
        {
            galleryMenu.transform.position = imageToFollow.transform.position;
        }
    }

    public void MoveGalleryMenu(GalleryPhotoData clickedPhoto)
    {
        galleryMenu.transform.position = clickedPhoto.transform.position;
        imageToFollow = clickedPhoto.gameObject;
        galleryMenuPhoto.sprite = clickedPhoto.photoImage.sprite;
        galleryMenuText.text = clickedPhoto.photo.photoName;
    }

    public void ZoomInPhoto(int photo)
    {
        if(photo == 1) ZoomedInPhoto.sprite = graditeljImage.sprite;
        else ZoomedInPhoto.sprite = galleryMenuPhoto.sprite;
    }
    private UnityWebRequest uwr;
    public static string GetFileLocation(string relativePath)
    {
        return "file://" + Path.Combine(Application.streamingAssetsPath, relativePath);
    }

    public IEnumerator ChangeMainImage(string fileName)
    {
        using (uwr = UnityWebRequestTexture.GetTexture(GetFileLocation(fileName + ".png")))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                graditeljImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}

[System.Serializable]
public class GalleryPhoto
{
    public string fileName;
    public string photoName;
}

[System.Serializable]
public class GalleryPhotos
{
    public GalleryPhoto[] galleryPhotos;
}

[System.Serializable]
public class MainPhoto
{
    public string mainFileName;
    public string mainDescription;
}
