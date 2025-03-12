using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    EcholocationPulse echoPulse;

    [Header("Edge Gradient info")]
    [SerializeField] RawImage edgeGradientRI;
    [SerializeField] Color highFreqCol;
    [SerializeField] Color lowFreqCol;
    [SerializeField] Color hurtCol = Color.red;
    //min val for the radius that is furthest off the screen, max for closest towards middle
    [SerializeField] float edgeRadMin = 0.75f, edgeRadMax = 0.5f;

    Material edgeGradientMat;

    [SerializeField] float colLerpSpeed = 2f;
    Color currentEdgeCol;

    PlayerMovement player;

    private void Start()
    {
        echoPulse = FindObjectOfType<EcholocationPulse>();

        edgeGradientMat = edgeGradientRI.material;
        edgeGradientMat.SetColor("_edgeColour", highFreqCol);

        player = HapticManager.instance.GetPlayer();

    }

    private void Update()
    {
        LerpEdgeCol();
        EdgeCooldownLerp();
    }

    //lerp between the colours for the currently selected frequency type
    void LerpEdgeCol()
    {
        Color targetCol;

        if (player.hurt)
        {
            targetCol = hurtCol;
            if (player.hurt)
            {
                Invoke(nameof(PlayerHurt), Random.Range(3, 6));
            }
        }
        else
        {
            if (echoPulse.GetCurrentPulse() == 0)
            {
                targetCol = lowFreqCol;
            }
            else
            {
                targetCol = highFreqCol;

            }
        }
        

        currentEdgeCol = Color.Lerp(currentEdgeCol, targetCol, colLerpSpeed * Time.deltaTime);

        edgeGradientMat.SetColor("_edgeColour", currentEdgeCol);


    }

    //lerp the radius of the edge colour to act as a cooldown bar
    void EdgeCooldownLerp()
    {
        float hRad = Mathf.Lerp(edgeRadMax, edgeRadMin, echoPulse.GetHTimer());
        float lRad = Mathf.Lerp(edgeRadMax, edgeRadMin, echoPulse.GetLTimer());

        if (echoPulse.GetCurrentPulse() == 0)
        {
            edgeGradientMat.SetFloat("_radius", lRad);

        }
        else
        {
            edgeGradientMat.SetFloat("_radius", hRad);

        }

    }

    //slowly increase and decrease the smoothness in a beating heart pattern to make the edge feel more alive
    void EdgeSoftness()
    {

    }

    void PlayerHurt()
    {
        player.hurt = false;
    }


    private void OnDisable()
    {
        //reset material attributes
        edgeGradientMat.SetColor("_edgeColour", highFreqCol);
        edgeGradientMat.SetFloat("_radius", edgeRadMax);

    }

}
