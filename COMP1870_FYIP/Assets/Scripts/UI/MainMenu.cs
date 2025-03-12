using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Sections")]
    [SerializeField] GameObject mainSection;
    [SerializeField] GameObject settingsSection, audioSection, controlsSection, creditsSection;

    [Header("UI Elements")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider, sfxSlider;

    Coroutine playRoutine;


    private const string MASTER_VOL_PARAM = "Master";
    private const string MUSIC_VOL_PARAM = "Music";
    private const string SFX_VOL_PARAM = "SFX";


    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(delegate { MasterSliderChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderChanged(); });
        sfxSlider.onValueChanged.AddListener(delegate { SFXSliderChanged(); });

        Time.timeScale = 1f;
    }

    private void MasterSliderChanged()
    {
        AudioMixerManager.instance.SetMasterVol(masterSlider.value);
    }
    private void MusicSliderChanged()
    {
        AudioMixerManager.instance.SetMusicVol(musicSlider.value);
    }
    private void SFXSliderChanged()
    {
        AudioMixerManager.instance.SetSFXVol(sfxSlider.value);
    }



    void Start()
    {
        AudioMixerManager.instance.LoadVolSettings();

        mainSection.SetActive(true);
        settingsSection.SetActive(false);
        audioSection.SetActive(false);
        controlsSection.SetActive(false);
        creditsSection.SetActive(false);

        masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOL_PARAM);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOL_PARAM);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOL_PARAM);
    }


    public void Play()
    {
        playRoutine = StartCoroutine(PlayGame());
    }

    IEnumerator PlayGame()
    {

        yield return new WaitForSeconds(0.3f);
        //fade screen?

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);


    }

    public void Quit()
    {
        StartCoroutine(QuitGame());

    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(0.3f);
        //fade screen?
        Application.Quit();

    }

    private void OnDisable()
    {
        playRoutine = null;
    }
}
