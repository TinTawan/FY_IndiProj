using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int objectsPlaced = 0;

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
    }

    private void Update()
    {
        if(objectsPlaced >= 5)
        {
            //win
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
