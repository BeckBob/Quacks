using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PotionQuality : MonoBehaviour 
{
    [SerializeField] private Renderer potionOne;
  
    public UnityEvent<GameObject> OnEnterEvent;

    public ChipPoints _chipPoints;
    GrabIngredient _grabIngredient;
 
  

    [SerializeField] TextMeshProUGUI cherryBombsText;
    [SerializeField] TextMeshProUGUI cherryBombsText2;

    private int _cherryBombs = 0;
  
    public int cherryBombLimit = 7;

    public bool nextIngredientTime = true;

    private Color originalTopColor;
    private Color originalVoronoiColor;
    public float fadeDuration = 2.0f; 
   
    public Color objectColor = Color.green;
    public Color fadeColor = Color.black;

    private void Start()
    {
        
        originalTopColor = potionOne.material.GetColor("_topColor");
        originalVoronoiColor = potionOne.material.GetColor("_voronoiColor");
    }


    public void ResetCherryBombs()
    {
        _cherryBombs = 0;
    }

    public void SetCherryBombText()
    {
        string cherrybombsinPot = _cherryBombs.ToString();
        string cherrybombLimitforText = cherryBombLimit.ToString();
        cherryBombsText.text = $"{cherrybombsinPot}/{cherrybombLimitforText}";
        cherryBombsText2.text = $"{cherrybombsinPot}/{cherrybombLimitforText}";
    }

    public void ResetCherryBombLimit()
    {
        cherryBombLimit = 7;
    }

    public int GetCherryBombs()
    {
        return _cherryBombs;
    }

    public int GetCherryBombLimit()
    {
        return cherryBombLimit;
    }

    public void AddToCherryBombLimit()
    {
        cherryBombLimit++;
    }

    public void RemoveFromCherryBombLimit()
    {
        cherryBombLimit--;
    }

    public void RemoveFromCherryBombs()
    {
        _cherryBombs--;
    }

    public void FalseNextIngredientMethod()
    {
        if (nextIngredientTime)
        {
            nextIngredientTime = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cherryBombOne"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 1;

            if (_cherryBombs > cherryBombLimit)
            {
                StartCoroutine(FadeToColor(potionOne.material, fadeColor, Color.grey));
                _chipPoints.PotExplosionEndRound();
            }
        }
        else if (other.gameObject.CompareTag("cherryBombTwo"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 2;

            if (_cherryBombs > cherryBombLimit)
            {
                StartCoroutine(FadeToColor(potionOne.material, fadeColor, Color.grey));
                _chipPoints.PotExplosionEndRound();
            }
        }
        else if (other.gameObject.CompareTag("cherryBombThree"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 3;

            if (_cherryBombs > cherryBombLimit)
            {
                StartCoroutine(FadeToColor(potionOne.material, fadeColor, Color.grey));
                _chipPoints.PotExplosionEndRound();
            }
        }
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _grabIngredient.updateCherryBombs();
        nextIngredientTime = true;
        SetCherryBombText();
    }

    private IEnumerator FadeToColor(Material material, Color targetTopColor, Color targetVoronoiColor)
    {
        float elapsedTime = 0.0f;
        Color startTopColor = material.GetColor("_topColor");
        Color startVoronoiColor = material.GetColor("_voronoiColor");

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            // Interpolate between the start and target colors
            material.SetColor("_topColor", Color.Lerp(startTopColor, targetTopColor, t));
            material.SetColor("_voronoiColor", Color.Lerp(startVoronoiColor, targetVoronoiColor, t));

            yield return null; // Wait for the next frame
        }

        // Ensure final colors are set (useful if elapsedTime slightly undershoots)
        material.SetColor("_topColor", targetTopColor);
        material.SetColor("_voronoiColor", targetVoronoiColor);
    }

    public void ResetPotionColour()
    {
        potionOne.material.SetColor("_topColor", originalTopColor);
        potionOne.material.SetColor("_voronoiColor", originalVoronoiColor);
    }
}
    


   









