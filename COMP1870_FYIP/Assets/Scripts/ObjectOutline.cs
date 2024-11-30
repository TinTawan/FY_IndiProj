using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutline : MonoBehaviour
{
    bool isOutlined = false, canBeTriggered = false;

    [SerializeField] private float outlineTime = 3f;
    [SerializeField] private Color outlineColour;
    Material outlineMat;

    private void Start()
    {
        outlineMat = GetComponent<MeshRenderer>().materials[1];
        outlineMat.SetColor("_outlineColour", outlineColour);
        
    }

    private void Update()
    {
        if (canBeTriggered && !isOutlined)
        {
            canBeTriggered = false;

            StartCoroutine(FadeOutline(outlineMat));
        }
    }


    IEnumerator FadeOutline(Material mat)
    {
        isOutlined = true;
        mat.SetFloat("_outlineDepth", 0.02f);

        yield return new WaitForSeconds(outlineTime);


        for (float i = 0.02f; i >= -0.001f; i -= 0.001f)
        {
            mat.SetFloat("_outlineDepth", i);
            yield return null;

        }

        isOutlined = false;

    }


    public bool GetIsOutlined()
    {
        return isOutlined;
    }

    public void SetCanBeTriggered(bool inBool)
    {
        canBeTriggered = inBool;
    }
}
