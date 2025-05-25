using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int objectsPlaced = 0;
    public bool win = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void IncrementObjects()
    {
        objectsPlaced++;

        if(objectsPlaced >= 5)
        {
            win = true;
        }
    }

    private void Update()
    {
        if(win)
        {
            //win
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(WinCountDown());
        }
    }

    IEnumerator WinCountDown()
    {
        win = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
