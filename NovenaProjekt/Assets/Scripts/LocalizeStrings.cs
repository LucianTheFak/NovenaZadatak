using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizeStrings : MonoBehaviour
{
    public UIManager uiManager;
    public enum Language
    {
        Croatian,
        English
    }
    public Language currentLanguage = Language.Croatian;

    public void ChangeLanguage(int indexOfLanguage)
    {
        currentLanguage = (Language)indexOfLanguage;
        if (currentLanguage == Language.Croatian)
        {
            if(uiManager.openGalleryPhoto != null) uiManager.galleryMenuText.text = uiManager.openGalleryPhoto.photo.photoNameHRV;
            uiManager.graditeljText.text = uiManager.mainPhoto.mainDescriptionHRV;
        }
        else
        {
            if (uiManager.openGalleryPhoto != null) uiManager.galleryMenuText.text = uiManager.openGalleryPhoto.photo.photoNameENG;
            uiManager.graditeljText.text = uiManager.mainPhoto.mainDescriptionENG;
        }
        StartCoroutine(SetLanguage(indexOfLanguage));
    }

    public IEnumerator SetLanguage(int index)
    {
        yield return LocalizationSettings.InitializationOperation;

        int i = index;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
    }
}
