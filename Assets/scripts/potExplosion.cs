using UnityEngine;

public class PotExplosion : MonoBehaviour
{
    [SerializeField] private Renderer potionOne;
    public PotionQuality quality;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a cherry bomb and if the cherryBombs count in quality is less than or equal to 7
        if (other.gameObject.tag.Contains("cherryBomb") && quality.GetCherryBombs() <= quality.cherryBombLimit)
        {
            potionOne.material.color = Color.red;

            // Create a timer to change the color back to green after 3 seconds
            FunctionTimer.Create(() => potionOne.material.color = Color.green, 3f);
        }
    }
}
