using UnityEngine;

public class AreaCheck : MonoBehaviour
{
    public bool inArea { get; private set; }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            inArea = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            inArea = false;
        }
    }

}
