using UnityEngine;
using UnityEngine.SceneManagement;


public class WinMenu : MonoBehaviour
{
    [SerializeField] GameObject mainSection, creditsSection;

    void Start()
    {
        mainSection.SetActive(true);
        creditsSection.SetActive(false);
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
