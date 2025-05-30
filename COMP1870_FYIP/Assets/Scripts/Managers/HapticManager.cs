using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;
    
    private PlayerMovement player;


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

    }

    public void HapticFeedback(float lowFrequency, float highFrequency, float duration)
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);

            StartCoroutine(StopHaptics(duration, gamepad));

        }
    }

    private IEnumerator StopHaptics(float duration, Gamepad gamepad)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }


        gamepad.ResetHaptics();
    }

    public void StopHaptics()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.ResetHaptics();
        }
    }

    private void OnDisable()
    {
        if(Gamepad.current != null)
        {
            Gamepad.current.ResetHaptics();
        }
    }
}
