using UnityEngine;
using UnityEngine.SceneManagement;


public class WinMenu : MonoBehaviour
{
    [SerializeField] GameObject mainSection, creditsSection;

    PlayerInput pInput;

    private void Awake()
    {
        pInput = new PlayerInput();
        pInput.UI.Enable();

        Cursor.lockState = CursorLockMode.None;
    }

    void Start()
    {
        Time.timeScale = 1f;

        mainSection.SetActive(true);
        creditsSection.SetActive(false);

        AudioManager.instance.PlaySound(AudioManager.soundType.win, transform.position, 0);

    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
