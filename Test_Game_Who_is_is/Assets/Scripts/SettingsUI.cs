using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    
    [SerializeField] private Button lowButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button highButton;

   
    [SerializeField] private Slider musicVolumeSlider;

    private void Start()
    {
        lowButton.onClick.AddListener(() => SetQuality(0));
        mediumButton.onClick.AddListener(() => SetQuality(2));
        highButton.onClick.AddListener(() => SetQuality(5));

        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

       
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SetMusicVolume(musicVolumeSlider.value);
    }

    private void SetQuality(int level)
    {
        QualitySettings.SetQualityLevel(level, true);
        PlayerPrefs.SetInt("QualityLevel", level);
    }

    private void SetMusicVolume(float volume)
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(volume);

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}