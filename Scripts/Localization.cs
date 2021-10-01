using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Localization : MonoBehaviour
{
    [SerializeField] private int id = -1;
    private TextMeshProUGUI shared;
    private bool isGame;
    private LocalizeManager.ChangeLanguageDelegate changeLanguageDelegate;
    private void Start()
    {
        isGame = SceneManager.GetActiveScene().buildIndex == 1;
        shared = GetComponent<TextMeshProUGUI>();
        if (!isGame)
        {
            changeLanguageDelegate = delegate
            {
                shared.text = LocalizeManager.GetLocalizedString(id, isGame);
            };
            if (LocalizeManager.CurrentLanguage == Language.Russian)
                changeLanguageDelegate.Invoke();
            LocalizeManager.AddChangeListener(changeLanguageDelegate);
        }
        else if (LocalizeManager.CurrentLanguage == Language.Russian)
            shared.text = LocalizeManager.GetLocalizedString(id, isGame);
    }
    private void OnDestroy()
    {
        if (!isGame && !LocalizeManager.IsChangeListenersListClear)
        {
            LocalizeManager.ClearChangeListeners();
        }
    }
}
