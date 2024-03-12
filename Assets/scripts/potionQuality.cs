using UnityEngine;
using UnityEngine.Events;

public class PotionQuality : MonoBehaviour
{
    [SerializeField] private Renderer potionOne;
    public UnityEvent<GameObject> OnEnterEvent;

    private int _cherryBombs;

    public int GetCherryBombs()
    {
        return _cherryBombs;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cherryBombOne"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 1;

            if (_cherryBombs > 7)
            {
                potionOne.material.color = Color.black;
            }
        }
        else if (other.gameObject.CompareTag("cherryBombTwo"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 2;

            if (_cherryBombs > 7)
            {
                potionOne.material.color = Color.black;
            }
        }
        else if (other.gameObject.CompareTag("cherryBombThree"))
        {
            OnEnterEvent.Invoke(other.gameObject);
            _cherryBombs += 3;

            if (_cherryBombs > 7)
            {
                potionOne.material.color = Color.black;
            }
        }
    }
}






