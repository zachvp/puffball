using UnityEngine;
using System;

public class TriggerVolume : MonoBehaviour
{
    public bool isTriggered;
    public LayerMask mask;
    public Collider2D[] overlappingObjects = new Collider2D[1];

    [NonSerialized]
    new public Collider2D collider;

    public void Awake()
    {
        collider = GetComponent<Collider2D>();

        Debug.AssertFormat(collider != null, $"Script:{nameof(TriggerVolume)} requires collider attached to the same game object");
        Debug.AssertFormat(overlappingObjects.Length > 0, "non-zero length required for trigger");
        Debug.AssertFormat(collider.isTrigger, "attached collider required to be trigger");
    }

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
