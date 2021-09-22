using UnityEngine;
using System.Collections;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject gameTutorial, controlTutorial;
    public static TutorialManager shared;
    private TutorialData tutorailData;
    public bool TutorialCompleted { get; private set; }
    private void Awake()
    {
        shared = this;
        tutorailData = TutorialData.Shared;
        TutorialCompleted = tutorailData.GameTutorialCompleted;
    }
    private IEnumerator Start()
    {
        if (!TutorialCompleted)
        {
            gameTutorial.SetActive(true);
            yield return new WaitForSeconds(2f);
            controlTutorial.SetActive(true);
            yield return new WaitWhile(() => controlTutorial.activeSelf);
            TutorialCompleted = true;
            tutorailData.GameTutorialCompleted = TutorialCompleted;
            tutorailData.Save();
        }
        
    }
}
