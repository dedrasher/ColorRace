using UnityEngine;
public class LocalizeSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Borders = new GameObject[2];
    private MenuManager menuManager;
    private void Start()
    {
        menuManager = MenuManager.Shared;
        UpdateBorders((int)LocalizeManager.CurrentLanguage);
    }
    public void ChangeLocalize(int value)
    {
        var language = value;
        LocalizeManager.ChangeLanguage((LocalizeManager.Language)language);
        menuManager.ChangeStatsLocalization();
        UpdateBorders(language);
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
