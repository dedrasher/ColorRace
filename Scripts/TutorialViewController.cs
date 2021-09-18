using UnityEngine;
using TMPro;
public class TutorialViewController : MonoBehaviour
{
    [SerializeField] private int tutorialIndex;
    private void Start()
    {
        if (LocalizeManager.CurrentLanguage == LocalizeManager.Language.Russian)
            GetComponent<TextMeshProUGUI>().text = tutorialIndex == 1 ? "Для перемещения налево нажимать на левую часть экрана, направо - на правую" : "Цель игры - разбивать кубики в цвет бортиков стен";
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
