using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject[] Borders = new GameObject[2];
    [SerializeField] private Toggle FPSToggle;
    private MenuManager menuManager;
    private void Start()
    {
        menuManager = MenuManager.Shared;
        UpdateBorders((int)LocalizeManager.CurrentLanguage);
        FPSToggle.isOn = Preferences.FPSViewEnabled;
    }
    public void ChangeLocalize(int value)
    {
        var language = value;
        LocalizeManager.ChangeLanguage((Language)language);
        menuManager.ChangeStatsLocalization();
        UpdateBorders(language);
    }
    public void SetDisplayFPSFlag(bool value)
    {
        Preferences.FPSViewEnabled = value;
    }
    private void UpdateBorders(int value)
    {
        var language = value;
        Borders[language].SetActive(true);
        Borders[language == 1 ? 0 : 1].SetActive(false);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
