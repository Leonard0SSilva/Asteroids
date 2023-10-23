using UnityEngine;

public class DisableOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask myLayerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
        // Check if the LayerMask of 'other.includeLayers' contains the layer of this GameObject
        if ((other.includeLayers & myLayerMask) != 0)
        {
            gameObject.SetActive(false);
        }
    }
}
