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

    

    private void Start()
    {
        
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
                StartCoroutine(FadeToColor(potionOne.material, fadeColor, Color.red, Color.black));
                _chipPoints.PotExplosionEndRound();
                _explosion.Play();
                SetSmokeColor("black");
                HapticFeedbackExplosion(1,3);
            }
            else
            {
                _bubble.Play();
                SetSmokeColor("red");
                HapticFeedbackExplosion(1, 5);
            }
        }
        else if (other.gameObject.CompareTag("cherryBombTwo"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 2;

            if (_cherryBombs > cherryBombLimit)
            {
                StartCoroutine(FadeToColor(potionOne.material, fadeColor, Color.red, Color.red));
                _chipPoints.PotExplosionEndRound();
                _explosion.Play();
                SetSmokeColor("black");
                HapticFeedbackExplosion(1, 3);
            }
            else
            {
                
                _bubble.Play();
                SetSmokeColor("red");
                HapticFeedbackExplosion(1, 5);
            }
        }
        else if (other.gameObject.CompareTag("cherryBombThree"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 3;

            if (_cherryBombs > cherryBombLimit)
            {
                StartCoroutine(FadeToColor(potionOne.material, fadeColor, Color.red, Color.red));
                _chipPoints.PotExplosionEndRound();
                _explosion.Play();
                SetSmokeColor("black");
                HapticFeedbackExplosion(1, 3);
            }
            else
            {
                _bubble.Play();
                SetSmokeColor("red");
                HapticFeedbackExplosion(1, 5);
            }
        }
        else if (other.gameObject.tag.Contains("ghost"))
        {
            SetSmokeColor("purple");
        }
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _grabIngredient.updateCherryBombs();
        nextIngredientTime = true;
        SetCherryBombText();
    }

    private IEnumerator FadeToColor(Material material, Color targetTopColor, Color targetVoronoiColor, Color targetFoamColour)
    {
        float elapsedTime = 0.0f;
        Color startTopColor = material.GetColor("_topColor");
        Color startVoronoiColor = material.GetColor("_voronoiColor");
        Color startFoamColor = material.GetColor("_foamColour");

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            
            material.SetColor("_topColor", Color.Lerp(startTopColor, targetTopColor, t));
            material.SetColor("_voronoiColor", Color.Lerp(startVoronoiColor, targetVoronoiColor, t));
            material.SetColor("_foamColour", Color.Lerp(startFoamColor, targetFoamColour, t));

            yield return null; 
        }

        
        material.SetColor("_topColor", targetTopColor);
        material.SetColor("_voronoiColor", targetVoronoiColor);
        material.SetColor("_foamColour", startFoamColor);
    }

    public void ResetPotionColour()
    {
        potionOne.material.SetColor("_topColor", originalTopColor);
        potionOne.material.SetColor("_voronoiColor", originalVoronoiColor);
    }

    private void HapticFeedbackExplosion(float intensity, int time)
    {
        leftInteractor.xrController.SendHapticImpulse(intensity, time);
        rightInteractor.xrController.SendHapticImpulse(intensity, time);
    }

}
    


   









