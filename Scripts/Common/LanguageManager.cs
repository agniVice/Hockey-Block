using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public enum Languange
{
    English,
    German
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    public Languange CurrentLanguage;

    public List<LocalizedString> LocalizedStrings = new List<LocalizedString>();

    [SerializeField] private List<string> _english;
    [SerializeField] private List<string> _german;

    /* KEYS
     * 0 BEST SCORE
     * 1 Language
     * 2 SETTINGS
     * 3 LOSE!
     * 4 PAUSE
    */

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        Initialize();
    }
    private void Initialize()
    {
        CurrentLanguage = (Languange)PlayerPrefs.GetInt("Language", 0);
    }
    public void ToggleLanguange()
    {
        if (CurrentLanguage == Languange.English)
            CurrentLanguage = Languange.German;
        else if(CurrentLanguage == Languange.German)
            CurrentLanguage = Languange.English;

        PlayerPrefs.SetInt("Language", (int)CurrentLanguage);

        foreach (LocalizedString localizedString in LocalizedStrings)
        {
            if (localizedString != null)
                localizedString.LocalizeMe();
        }
    }
    public string GetTranslate(int keyIndex)
    {
        if (CurrentLanguage == Languange.English)
        {
            if (_english.Count > keyIndex)
                return _english[keyIndex];
            else
                return "Unknown";
        }
        else if(CurrentLanguage == Languange.German)
        {
            if (_german.Count > keyIndex)
                return _german[keyIndex];
            else
                return "Unknown";
        }
        return "Unknown";
    }
    public void ChangeLanguage(int id)
    {
        CurrentLanguage = (Languange)id;

        FindObjectOfType<MenuUserInterface>().UpdateLanguageIcon();

        PlayerPrefs.SetInt("Language", (int)CurrentLanguage);

        foreach (LocalizedString localizedString in LocalizedStrings)
        {
            if (localizedString != null)
                localizedString.LocalizeMe();
        }
    }
}
