using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;

    private Coroutine stopHaptics;

    private void Awake()
    {
        /*if(instance == null)
        {
            instance = this;
        }*/

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void HapticFeedback(float lowFrequency, float highFrequency, float duration)
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);

            stopHaptics = StartCoroutine(StopHaptics(duration, gamepad));

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
}
