using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    public static MenuManager singleton { get; private set; }
    [SerializeField]
    private GameObject SettingsPanel;
    private GameObject skinViewHolder;
    [SerializeField]
    private Animator canvasHandler, skinView;
    [SerializeField]
    private TextMeshProUGUI maxScore, maxTime;
    [SerializeField]
    private GameObject loadingText;
    [SerializeField]
    private Image VolumeSwitcher;
    [SerializeField]
    private Sprite SoundOn, SoundOff;
    private void Awake()
    {
        singleton = this;
        LocalizeManager.Init();
        #if UNITY_IOS
        Application.targetFrameRate = 60;
#endif
    }
    private void Start()
    {
        skinViewHolder = SkinViewController.SharedGameObject;
        var volume = Preferences.Volume;
        AudioListener.volume =  volume;
        VolumeSwitcher.sprite = volume == 1f ? SoundOn : SoundOff;
        UpdateRecords();
    }
    private void UpdateRecords()
    {
        maxScore.text = LocalizeManager.GetLocalizedString(LocalizeManager.MaxScore,false) + Preferences.ScoreRecord.ToString();
        var time = Preferences.TimeRecord;
        string Seconds()
        {
            var seconds = time % 60;
            return seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
        }
        maxTime.text = LocalizeManager.GetLocalizedString(LocalizeManager.MaxTime, false) + (time / 60).ToString() + ":" + Seconds();
    }
    public void ChangeVolume()
    {
        var volume = AudioListener.volume == 0f ? 1f : 0f;
        AudioListener.volume = volume;
        VolumeSwitcher.sprite = volume == 1f ? SoundOn : SoundOff;
        Preferences.Volume = volume;
    }
    public void Play()
    {
        loadingText.SetActive(true);
        SceneManager.LoadScene(1);
    }
    public void OpenShop()
    {
        if (!SettingsPanel.activeSelf)
        {
            canvasHandler.Play("OpenShop");
            if (skinViewHolder.activeSelf)
                skinView.Play("OpenSkinView");
        }
    }
    public void ChangeStatsLocalization()
    {
        UpdateRecords();
    }
    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }
}
