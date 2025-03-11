using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject mainSection, settingsSection, audioSection, controlsSection, creditsSection;

    void Start()
    {
        mainSection.SetActive(true);
        settingsSection.SetActive(false);
        audioSection.SetActive(false);
        controlsSection.SetActive(false);
        creditsSection.SetActive(false);

    }


    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void Quit()
    {
        Application.Quit();
    }

}
