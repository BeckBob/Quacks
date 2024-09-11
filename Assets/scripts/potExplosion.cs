using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PotExplosion : MonoBehaviour
{
    [SerializeField] private Renderer potionOne;
    public PotionQuality quality;
    string hexColor = "#539546";
    Color topColor;

    public float fadeDuration = 0.5f; 

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag.Contains("cherryBomb") && quality.GetCherryBombs() <= quality.cherryBombLimit)
        {
          
            StartCoroutine(FadeToColor(potionOne, Color.red, Color.magenta, Color.red));

            
            FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
        }
    }

    private IEnumerator FadeToColor(Renderer renderer, Color targetTopColor, Color targetVoronoiColor, Color targetFoamColour)
    {
        float elapsedTime = 0.0f;
        Color startTopColor = renderer.material.GetColor("_topColor");
        Color startVoronoiColor = renderer.material.GetColor("_voronoiColor");
        Color startFoamColour = renderer.material.GetColor("_foamColour");

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            
            renderer.material.SetColor("_topColor", Color.Lerp(startTopColor, targetTopColor, t));
            renderer.material.SetColor("_voronoiColor", Color.Lerp(startVoronoiColor, targetVoronoiColor, t));
            renderer.material.SetColor("_foamColour", Color.Lerp(startFoamColour, targetFoamColour, t));

            yield return null; 
        }

       
        renderer.material.SetColor("_topColor", targetTopColor);
        renderer.material.SetColor("_voronoiColor", targetVoronoiColor);
        renderer.material.SetColor("_foamColour", startFoamColour);
    }

    private IEnumerator ReturnColor()
    {
        float elapsedTime = 0.0f;
        Color startTopColor = potionOne.material.GetColor("_topColor");
        Color startVoronoiColor = potionOne.material.GetColor("_voronoiColor");
        Color originalVoronoiColor = new Color(0.325601f, 0.5849056f, 0.2731399f, 1f);

        
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out topColor))
        {
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);

                // Interpolate back to the original colors
                potionOne.material.SetColor("_topColor", Color.Lerp(startTopColor, topColor, t));
                potionOne.material.SetColor("_voronoiColor", Color.Lerp(startVoronoiColor, originalVoronoiColor, t));

                yield return null; // Wait for the next frame
            }

            // Ensure final colors are set
            potionOne.material.SetColor("_topColor", topColor);
            potionOne.material.SetColor("_voronoiColor", originalVoronoiColor);
        }
    }
}
