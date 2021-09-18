using UnityEngine;
using System.Collections;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject gameTutorial, controlTutorial;
    public static TutorialManager shared;
    public bool tutorialCompleted { get; private set; }
    private void Awake()
    {
        shared = this;
        tutorialCompleted = Preferences.TutorialCompleted;
    }
    private IEnumerator Start()
    {
        if (!tutorialCompleted)
        {
            gameTutorial.SetActive(true);
            yield return new WaitForSeconds(2f);
            controlTutorial.SetActive(true);
            yield return new WaitWhile(() => controlTutorial.activeSelf);
            tutorialCompleted = true;
            Preferences.TutorialCompleted = tutorialCompleted;
        }
        
    }
}
