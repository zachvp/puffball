using UnityEngine;
using System;

public class TriggerVolume : MonoBehaviour
{
    public bool isTriggered;
    public LayerMask mask;
    public Collider2D[] triggeredObjects = new Collider2D[1];
    public Action<ContainerTrait> onTraitFound;
    public Trait triggeredTraits;
    public LayerMask triggeredLayers;

    [NonSerialized]
    new public Collider2D collider;

    public void Awake()
    {
        collider = GetComponent<Collider2D>();

        Debug.AssertFormat(collider != null, $"Script:{nameof(TriggerVolume)} requires collider attached to the same game object");
        Debug.AssertFormat(triggeredObjects.Length > 0, "non-zero length required for trigger");
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

        isTriggered = collider.OverlapCollider(filter, triggeredObjects) > 0;

        if (isTriggered)
        {
            // search for trait in each object hierarchy
            foreach (var o in triggeredObjects)
            {
                var trait = o.GetComponentInParent<ContainerTrait>();

                if (trait)
                {
                    triggeredTraits |= trait.value;

                    Emitter.Send(onTraitFound, trait);
                }

                // Update triggered layers
                triggeredLayers |= 1 << o.gameObject.layer;
            }
        }
    }

    private void ClearState()
    {
        triggeredTraits = Trait.NONE;
        triggeredLayers = 0;

        for (var i = 0; i < triggeredObjects.Length; i++)
        {
            triggeredObjects[i] = null;
        }
    }
}
