using UnityEngine;
using UnityEngine.Events;

public class PotionQuality : MonoBehaviour
{
    [SerializeField] private Renderer potionOne;
    public UnityEvent<GameObject> OnEnterEvent;

    private int _cherryBombs;
    private grabIngredient _grabIngredient;
    public int cherryBombLimit = 7;


    private void Awake()
    {
        _grabIngredient = FindObjectOfType<grabIngredient>();


    }

    public int GetCherryBombs()
    {
        return _cherryBombs;
    }

    public int getCherryBombLimit()
    {
        return cherryBombLimit;
    }

    public void AddToCherryBombLimit()
    {
        cherryBombLimit++;
    }

    private void OnTriggerEnter(Collider other)
    {
        _grabIngredient.ingredientInPotion();
        if (other.gameObject.CompareTag("cherryBombOne"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 1;

            if (_cherryBombs > cherryBombLimit)
            {
                potionOne.material.color = Color.black;
            }
        }
        else if (other.gameObject.CompareTag("cherryBombTwo"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 2;

            if (_cherryBombs > cherryBombLimit)
            {
                potionOne.material.color = Color.black;
            }
        }
        else if (other.gameObject.CompareTag("cherryBombThree"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 3;

            if (_cherryBombs > cherryBombLimit)
            {
                potionOne.material.color = Color.black;
            }
        }
    }
}






