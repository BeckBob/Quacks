using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PotExplosion : MonoBehaviour
{
    [SerializeField] private Renderer potionOne;
    public PotionQuality quality;
    string hexColor = "#539546";
    Color topColor;

    private Color originalTopColor;
    private Color originalVoronoiColor;
    private Color originalFoamColor;

    public float fadeDuration = 0.5f;


    private void Start()
    {

        originalTopColor = potionOne.material.GetColor("_topColor");
        originalVoronoiColor = potionOne.material.GetColor("_voronoiColor");
        originalFoamColor = potionOne.material.GetColor("_foamColour");
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag.Contains("cherryBomb") && quality.GetCherryBombs() <= quality.cherryBombLimit)
        {
          
            StartCoroutine(FadeToColor(potionOne, Color.red, Color.yellow, Color.red));

            
            FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
        }
    }

    private IEnumerator FadeToColor(Renderer renderer, Color targetTopColor, Color targetVoronoiColor, Color targetFoamColour)
    {
        float elapsedTime = 0.0f;
    

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);


            renderer.material.SetColor("_topColor", Color.Lerp(originalTopColor, targetTopColor, t));
            renderer.material.SetColor("_voronoiColor", Color.Lerp(originalVoronoiColor, targetVoronoiColor, t));
            renderer.material.SetColor("_foamColour", Color.Lerp(originalFoamColor, targetFoamColour, t));

            yield return null; 
        }

       
        renderer.material.SetColor("_topColor", targetTopColor);
        renderer.material.SetColor("_voronoiColor", targetVoronoiColor);
        renderer.material.SetColor("_foamColour", originalFoamColor);
    }

    private IEnumerator ReturnColor()
    {
        float elapsedTime = 0.0f;

        
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out topColor))
        {
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);

                // Interpolate back to the original colors
                potionOne.material.SetColor("_topColor", Color.Lerp(Color.red, originalTopColor, t));
                potionOne.material.SetColor("_voronoiColor", Color.Lerp(Color.red, originalVoronoiColor, t));

                yield return null; // Wait for the next frame
            }

            // Ensure final colors are set
            potionOne.material.SetColor("_topColor", originalTopColor);
            potionOne.material.SetColor("_voronoiColor", originalVoronoiColor);
        }
    }
}
