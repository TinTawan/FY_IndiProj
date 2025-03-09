using UnityEngine;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pausedSection, audioSection, controlsSection, resumeButtonGO;



    private void Start()
    {
        pausedSection.SetActive(true);

        audioSection.SetActive(false);
        controlsSection.SetActive(false);

    }

    public void Resume()
    {
        GameMenuManager.instance.isPaused = false;
    }

    public void Audio()
    {
        audioSection.SetActive(true);

        pausedSection.SetActive(false);
        controlsSection.SetActive(false);

    }
    public void Controls()
    {
        controlsSection.SetActive(true);

        audioSection.SetActive(false);
        pausedSection.SetActive(false);

    }

    public void Back()
    {
        pausedSection.SetActive(true);

        audioSection.SetActive(false);
        controlsSection.SetActive(false);

    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(resumeButtonGO);

        pausedSection.SetActive(true);

        audioSection.SetActive(false);
        controlsSection.SetActive(false);

    }
}
