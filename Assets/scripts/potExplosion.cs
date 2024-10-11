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

    private Color currentTopColor; // Store current color at start of fade
    private Color currentVoronoiColor;
    private Color currentFoamColor;

    public float fadeDuration = 0.5f;

    private void Start()
    {
        // Store the original colors
        originalTopColor = potionOne.material.GetColor("_topColor");
        originalVoronoiColor = potionOne.material.GetColor("_voronoiColor");
        originalFoamColor = potionOne.material.GetColor("_foamColour");

        // Initialize current colors to original colors
        currentTopColor = originalTopColor;
        currentVoronoiColor = originalVoronoiColor;
        currentFoamColor = originalFoamColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        Color customPurple = new Color(0.5f, 0f, 0.5f);  // Equivalent to RGB(128, 0, 128)
        Color customPink = new Color(1f, 0.75f, 0.8f);   // Equivalent to RGB(255, 192, 203)

        if (other.gameObject.tag.Contains("cherryBomb") && quality.GetCherryBombs() <= quality.cherryBombLimit)
        {
            StartCoroutine(FadeToColor(potionOne, Color.red, Color.yellow, Color.red));
            FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
        }
        else if (other.gameObject.tag.Contains("ghost"))
        {
            StartCoroutine(FadeToColor(potionOne, customPurple, customPink, customPurple));
            FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
        }
    }

    private IEnumerator FadeToColor(Renderer renderer, Color targetTopColor, Color targetVoronoiColor, Color targetFoamColour)
    {
        float elapsedTime = 0.0f;

        // Store the current colors before starting the fade
        currentTopColor = renderer.material.GetColor("_topColor");
        currentVoronoiColor = renderer.material.GetColor("_voronoiColor");
        currentFoamColor = renderer.material.GetColor("_foamColour");

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            // Lerp to the target colors
            renderer.material.SetColor("_topColor", Color.Lerp(currentTopColor, targetTopColor, t));
            renderer.material.SetColor("_voronoiColor", Color.Lerp(currentVoronoiColor, targetVoronoiColor, t));
            renderer.material.SetColor("_foamColour", Color.Lerp(currentFoamColor, targetFoamColour, t));

            yield return null;
        }

        // Ensure final colors are set and update current colors
        renderer.material.SetColor("_topColor", targetTopColor);
        renderer.material.SetColor("_voronoiColor", targetVoronoiColor);
        renderer.material.SetColor("_foamColour", targetFoamColour);

        // Update current colors to the final colors after the fade
        currentTopColor = targetTopColor;
        currentVoronoiColor = targetVoronoiColor;
        currentFoamColor = targetFoamColour;
    }

    private IEnumerator ReturnColor()
    {
        float elapsedTime = 0.0f;

        // Parse the top color from hex
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out topColor))
        {
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / fadeDuration);

                // Lerp back to the original colors from the current colors
                potionOne.material.SetColor("_topColor", Color.Lerp(currentTopColor, originalTopColor, t));
                potionOne.material.SetColor("_voronoiColor", Color.Lerp(currentVoronoiColor, originalVoronoiColor, t));
                potionOne.material.SetColor("_foamColour", Color.Lerp(currentFoamColor, originalFoamColor, t));

                yield return null;
            }

            // Ensure the final original colors are restored
            potionOne.material.SetColor("_topColor", originalTopColor);
            potionOne.material.SetColor("_voronoiColor", originalVoronoiColor);
            potionOne.material.SetColor("_foamColour", originalFoamColor);

            // Reset current colors to original colors
            currentTopColor = originalTopColor;
            currentVoronoiColor = originalVoronoiColor;
            currentFoamColor = originalFoamColor;
        }
    }
}