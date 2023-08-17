using UnityEngine;

public class TriggerVolume : MonoBehaviour
{
    public bool isTriggered;
    public LayerMask mask;
    new public Collider2D collider;
    public Collider2D[] overlappingObjects;

#if DEBUG
    public void Awake()
    {
        Debug.AssertFormat(overlappingObjects.Length > 0, "non-zero length required for trigger");
        Debug.AssertFormat(collider.isTrigger, "attached collider required to be trigger");
    }
#endif

    public void OnTriggerEnter2D(Collider2D other)
    {
        ClearState();
        UpdateState();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        ClearState();
        UpdateState();
    }

    public void RefreshState()
    {
        ClearState();
        UpdateState();
    }

    private void UpdateState()
    {
        var filter = new ContactFilter2D();

        filter.useLayerMask = true;
        filter.layerMask = mask;

        isTriggered = collider.OverlapCollider(filter, overlappingObjects) > 0;
    }

    private void ClearState()
    {
        for (var i = 0; i < overlappingObjects.Length; i++)
        {
            overlappingObjects[i] = null;
        }
    }
}
