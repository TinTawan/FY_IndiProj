using UnityEngine;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject pausedSection, audioSection, resumeButtonGO;



    private void Start()
    {
        pausedSection.SetActive(true);
        audioSection.SetActive(false);

    }

    public void Resume()
    {
        GameMenuManager.instance.isPaused = false;
    }

    public void Settings()
    {
        pausedSection.SetActive(false);
        audioSection.SetActive(true);

        //EventSystem.current = 
    }

    public void Back()
    {
        pausedSection.SetActive(true);
        audioSection.SetActive(false);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(resumeButtonGO);

        pausedSection.SetActive(true);
        audioSection.SetActive(false);
    }
}
