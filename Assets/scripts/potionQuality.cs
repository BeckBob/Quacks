using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

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
        nextIngredientTime = true;
       

        if (other.gameObject.CompareTag("cherryBombOne"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 1;

            if (_cherryBombs > cherryBombLimit)
            {
           
                potionOne.material.color = Color.black;
                _chipPoints.PotExplosionEndRound();
            }
        }
        else if (other.gameObject.CompareTag("cherryBombTwo"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 2;

            if (_cherryBombs > cherryBombLimit)
            {
                potionOne.material.color = Color.black;
                _chipPoints.PotExplosionEndRound();

            }
        }
        else if (other.gameObject.CompareTag("cherryBombThree"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 3;

            if (_cherryBombs > cherryBombLimit)
            {
                potionOne.material.color = Color.black;
                _chipPoints.PotExplosionEndRound();
            }
        }
        _grabIngredient = FindObjectOfType<GrabIngredient>();
        _grabIngredient.updateCherryBombs();
        SetCherryBombText();
    }

    public void ResetPotionColour()
    {
        potionOne.material.color = Color.green;
    }
}






