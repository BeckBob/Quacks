using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Oculus.Interaction;
using Unity.VisualScripting;

public class PotionQuality : MonoBehaviour 
{
    [SerializeField] private Renderer potionOne;
    private XRUIInputModule InputModule => EventSystem.current.currentInputModule as XRUIInputModule;

    [SerializeField] XRRayInteractor rightInteractor;
    [SerializeField] XRRayInteractor leftInteractor;

    public UnityEvent<GameObject> OnEnterEvent;

    public ChipPoints _chipPoints;
    GrabIngredient _grabIngredient;

    [SerializeField] AudioSource _bubble;
    [SerializeField] AudioSource _explosion;

    [SerializeField] TextMeshProUGUI cherryBombsText;
    [SerializeField] TextMeshProUGUI cherryBombsText2;

    private int _cherryBombs = 0;
    
    public int cherryBombLimit = 7;

    public bool nextIngredientTime = true;
    [SerializeField] ParticleSystem Smoke;

    private Color originalTopColor;
    private Color originalVoronoiColor;
    private Color originalFoamColor;
    public float fadeDuration = 6f; 
   
    public Color objectColor = Color.green;
    public Color fadeColor = Color.black;

    string hexColor = "#539546";
    Color topColor;


    private Color currentTopColor; 
    private Color currentVoronoiColor;
    private Color currentFoamColor;





    private void Start()
    {
        _grabIngredient = FindObjectOfType<GrabIngredient>();

        originalTopColor = potionOne.material.GetColor("_topColor");
        originalVoronoiColor = potionOne.material.GetColor("_voronoiColor");
        originalFoamColor = potionOne.material.GetColor("_foamColour");
    }

    public void SetSmokeColor(string Colour)
    {
        var main = Smoke.main;
        if (Colour == "black")
        {
            main.startColor = Color.white;
            Smoke.Play();
        }
        if (Colour == "green")
        {
            main.startColor = new Color(0.325601f, 0.5849056f, 0.2731399f, 1f);
            Smoke.Play();
        }
        if (Colour == "red")
        {
            main.startColor = Color.red;
            Smoke.Play();
            FunctionTimer.Create(() => SetSmokeColor("green"), 5f);
        }
        if (Colour == "purple")
        {
            Color customPurple = new Color(0.5f, 0f, 0.5f);
            main.startColor = customPurple;
            Smoke.Play();
            FunctionTimer.Create(() => SetSmokeColor("green"), 5f);
        }


    }

    public bool IsPotExploded()
    {
        if (_cherryBombs > cherryBombLimit)
        {
            return true;
        }
        else { return false; }
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
                StartCoroutine(FadeToColor(potionOne, fadeColor, Color.red, Color.black));
                _chipPoints.PotExplosionEndRound();
                _explosion.Play();
                SetSmokeColor("black");
                HapticFeedbackExplosion(1,1);
            }
            else
            {
                StartCoroutine(FadeToColor(potionOne, Color.red, Color.yellow, Color.red));
                FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
                _bubble.Play();
                SetSmokeColor("red");
                HapticFeedbackExplosion(1, 3);
            }
        }
        else if (other.gameObject.CompareTag("cherryBombTwo"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 2;

            if (_cherryBombs > cherryBombLimit)
            {
                StartCoroutine(FadeToColor(potionOne, fadeColor, Color.red, Color.red));
                _chipPoints.PotExplosionEndRound();
                _explosion.Play();
                SetSmokeColor("black");
                HapticFeedbackExplosion(1, 1);
            }
            else
            {
                StartCoroutine(FadeToColor(potionOne, Color.red, Color.yellow, Color.red));
                FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
                _bubble.Play();
                SetSmokeColor("red");
                HapticFeedbackExplosion(1, 3);
            }
        }
        else if (other.gameObject.CompareTag("cherryBombThree"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 3;

            if (_cherryBombs > cherryBombLimit)
            {
                StartCoroutine(FadeToColor(potionOne, fadeColor, Color.red, Color.red));
                _chipPoints.PotExplosionEndRound();
                _explosion.Play();
                SetSmokeColor("black");
                HapticFeedbackExplosion(1, 1);
            }
            else
            {
                StartCoroutine(FadeToColor(potionOne, Color.red, Color.yellow, Color.red));
                FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
                _bubble.Play();
                SetSmokeColor("red");
                HapticFeedbackExplosion(1, 3);
            }
        }
        

     
        else if (other.gameObject.tag.Contains("ghost"))
        {
            Color customPurple = new Color(0.5f, 0f, 0.5f);  // Equivalent to RGB(128, 0, 128)
            Color customPink = new Color(1f, 0.75f, 0.8f);   // Equivalent to RGB(255, 192, 203)
            StartCoroutine(FadeToColor(potionOne, customPurple, customPink, customPurple));
            FunctionTimer.Create(() => StartCoroutine(ReturnColor()), 5f);
            SetSmokeColor("purple");
        }
       
       
        _grabIngredient.updateCherryBombs();
        nextIngredientTime = true;
        SetCherryBombText();
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

    public void ResetPotionColour()
    {
        potionOne.material.SetColor("_topColor", originalTopColor);
        potionOne.material.SetColor("_voronoiColor", originalVoronoiColor);
    }

    private void HapticFeedbackExplosion(float intensity, float duration)
    {
        
        StartCoroutine(ContinuousHapticFeedback(intensity, duration));
    }

    private IEnumerator ContinuousHapticFeedback(float intensity, float totalDuration)
    {
       
        float pulseDuration = 0.1f; 
        float elapsedTime = 0f;

        
        while (elapsedTime < totalDuration)
        {
            
            if (leftInteractor?.xrController != null)
            {
                leftInteractor.xrController.SendHapticImpulse(intensity, pulseDuration);
            }
            if (rightInteractor?.xrController != null)
            {
                rightInteractor.xrController.SendHapticImpulse(intensity, pulseDuration);
            }

            
            yield return new WaitForSeconds(pulseDuration);

            // Increment the elapsed time
            elapsedTime += pulseDuration;
        }
    }

}
    


   









