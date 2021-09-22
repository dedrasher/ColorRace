using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Game : MonoBehaviour
{
    public static Game singleton { get; private set; }
    private BallController ballController;
    private TutorialManager tutorialManager;
    [SerializeField]
    private Animator pausePanel;
    [SerializeField]
    private QuestionDialog questionDialog;
    [SerializeField]
    private GameObject LosePanel, PausePanel, PauseButton, RecordMark;
    [SerializeField]
    private TextMeshProUGUI timeText, scoreGameText, scoreGameOverText, moneyEarnedText;
    private int destroyedCubesCount = 0;
    private float time = 0f;
    public enum GameState
    {
        Running, Paused, Losed
    }
    public GameState gameState { get; private set; } = GameState.Running;
    private void Awake()
    {
        singleton = this;
    }
    private IEnumerator Start()
    {
        ballController = BallController.singleton;
        tutorialManager = TutorialManager.shared;
        yield return new WaitUntil(() => tutorialManager.TutorialCompleted);
        PauseButton.SetActive(true);
    }
    private void Update()
    {
        if (gameState == GameState.Running && tutorialManager.TutorialCompleted)
            time += Time.deltaTime;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus && gameState == GameState.Running)
            Pause();
    }
    public void IncreaseDestroyedCubesCount(bool show)
    {
        destroyedCubesCount++;
        if (show)
            scoreGameText.text = destroyedCubesCount.ToString();
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        if (ballController.IsTimeTextActive)
            ballController.SetTimeTextActive(false);
        PauseButton.SetActive(false);
        PausePanel.SetActive(true);
        pausePanel.Play("PausePanel");
        gameState = GameState.Paused;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        if (!ballController.IsTimeTextActive && !ballController.IsInEffect)
            ballController.SetTimeTextActive(true);
        PauseButton.SetActive(true);
        PausePanel.SetActive(false);
        gameState = GameState.Running;
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
#if !UNITY_EDITOR
        Handheld.Vibrate();
#endif
        PauseButton.SetActive(false);
        ballController.SetTimeTextActive(false);
        LosePanel.SetActive(true);
        var time = Mathf.RoundToInt(this.time);
        string Seconds()
        {
            var seconds = time % 60;
            return seconds < 10 ? $"0{seconds}" : seconds.ToString();
        }
        timeText.text = $"{LocalizeManager.GetLocalizedString(LocalizeManager.Time, false)}{time / 60}:{ Seconds()}";
        scoreGameOverText.text = LocalizeManager.GetLocalizedString(LocalizeManager.Score, false) + destroyedCubesCount.ToString();
        moneyEarnedText.text = $"+{destroyedCubesCount}";
        var oldTimeRecord = Preferences.TimeRecord;
        var oldScoreRecord = Preferences.ScoreRecord;
        var IsNewRecordGot = oldTimeRecord < time || oldScoreRecord < destroyedCubesCount;
        Preferences.SetMoney(destroyedCubesCount, true);
        if (IsNewRecordGot)
        {
            RecordMark.SetActive(true);
            if (oldTimeRecord < time)
                Preferences.TimeRecord = time;
            if (oldScoreRecord < destroyedCubesCount)
                Preferences.ScoreRecord = destroyedCubesCount;
        }
        gameState = GameState.Losed;
    }
    public void OpenQuit()
    {
        PausePanel.SetActive(false);
        questionDialog.Show(LocalizeManager.GetLocalizedString(LocalizeManager.QuitQuestion, false), delegate
        {
            Quit();
        }, delegate
        {
            questionDialog.Hide(); PausePanel.SetActive(true);
        });
    }
    public void OpenRestart()
    {
        PausePanel.SetActive(false);
        questionDialog.Show(LocalizeManager.GetLocalizedString(LocalizeManager.RestartQuestion, false), delegate
        {
            Restart();
        }, delegate
        {
            questionDialog.Hide(); PausePanel.SetActive(true);
        });
    }
    private void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
