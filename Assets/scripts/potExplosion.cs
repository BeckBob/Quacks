using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class PotExplosion : MonoBehaviour
{
    [SerializeField] private Renderer potionOne;
    public PotionQuality quality;
    string hexColor = "#209A00";
    Color topColor;

    public float fadeDuration = 2.0f; // Duration for the fade effect

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a cherry bomb and if the cherryBombs count in quality is less than or equal to 7
        if (other.gameObject.tag.Contains("cherryBomb") && quality.GetCherryBombs() <= quality.cherryBombLimit)
        {
            // Start fading to red and magenta colors
            StartCoroutine(FadeToColor(potionOne, Color.red, Color.magenta));

            // Create a timer to return to the original color after 3 seconds
            FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 3f);
        }
    }

    private IEnumerator FadeToColor(Renderer renderer, Color targetTopColor, Color targetVoronoiColor)
    {
        float elapsedTime = 0.0f;
        Color startTopColor = renderer.material.GetColor("_topColor");
        Color startVoronoiColor = renderer.material.GetColor("_voronoiColor");

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            // Interpolate between the start and target colors
            renderer.material.SetColor("_topColor", Color.Lerp(startTopColor, targetTopColor, t));
            renderer.material.SetColor("_voronoiColor", Color.Lerp(startVoronoiColor, targetVoronoiColor, t));

            yield return null; // Wait for the next frame
        }

        // Ensure final colors are set
        renderer.material.SetColor("_topColor", targetTopColor);
        renderer.material.SetColor("_voronoiColor", targetVoronoiColor);
    }

    private IEnumerator ReturnColor()
    {
        float elapsedTime = 0.0f;
        Color startTopColor = potionOne.material.GetColor("_topColor");
        Color startVoronoiColor = potionOne.material.GetColor("_voronoiColor");
        Color originalVoronoiColor = new Color(0.966518641f, 2f, 0.254716992f, 0.396078408f);

        // Parse the top color from the hex string
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
