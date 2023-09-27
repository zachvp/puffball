using UnityEngine;
using System;
using System.Collections.Generic;

// todo: refactor to separate lightweight and heavy classes
public class TriggerVolume : MonoBehaviour
{
    // config
    public LayerMask mask;
    public bool ignoreEventsInHierarchy = true;
    public HashSet<Collider2D> ignoredColliders;

    // state
    public BufferQueue<State> buffer;

    public bool isTriggered;
    public Collider2D[] triggeredObjects = new Collider2D[1];
    public Trait triggeredTraits;
    public LayerMask triggeredLayers;

    // Events
    public Action<ContainerTrait> onTraitFound;
    public Action onUpdateState;

    // dynamic links
    [NonSerialized]
    new public Collider2D collider;

    // debug
#if DEBUG
    public DebugDraw debugDraw;
#endif

    public void Awake()
    {
        buffer = new BufferQueue<State>(this, 0.5f);

        collider = GetComponent<Collider2D>();

        // based on config, populate the ignored colliders
        if (ignoreEventsInHierarchy)
        {
            var root = CoreUtilities.FindRoot(gameObject);
            var colliders = root.GetComponentsInChildren<Collider2D>();

            foreach (var c in colliders)
            {
                Physics2D.IgnoreCollision(collider, c);
            }

            // store the hierarchy's colliders
            ignoredColliders = new HashSet<Collider2D>(colliders);
        }

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

    private void UpdateState()
    {
        var filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = mask
        };

        var ignoreCount = 0;
        var overlapCount = collider.OverlapCollider(filter, triggeredObjects);
        isTriggered = overlapCount > 0;

        if (debugDraw)
        {
            debugDraw.enabled = isTriggered;
        }

        if (isTriggered)
        {
            // search for trait in each object hierarchy
            foreach (var o in triggeredObjects)
            {
                // update triggered traits
                var trait = o.GetComponentInParent<ContainerTrait>();
                if (trait)
                {
                    triggeredTraits |= trait.value;

                    Emitter.Send(onTraitFound, trait);
                }

                // Update triggered layers
                triggeredLayers |= 1 << o.gameObject.layer;

                // increment ignore count for each triggered collider that should be ignored
                if (ignoreEventsInHierarchy && ignoredColliders.Contains(o))
                {
                    ignoreCount++;
                }
            }

            // override triggered result if all found colliders should be ignored
            if (ignoreEventsInHierarchy && ignoreCount == overlapCount)
            {
                isTriggered = false;
                ClearState();
            }

            // store state in buffer
            var state = new State()
            {
                isTriggered = isTriggered,
                triggeredTraits = triggeredTraits,
                triggeredLayers = triggeredLayers
            };
            triggeredObjects.CopyTo(state.triggeredObjects, 0);
            buffer.Add(state);
        }

        Emitter.Send(onUpdateState);
    }

    private void ClearState()
    {
        triggeredTraits = Trait.NONE;
        triggeredLayers = 0;
        isTriggered = false;

        for (var i = 0; i < triggeredObjects.Length; i++)
        {
            triggeredObjects[i] = null;
        }
    }

    [Serializable]
    public class State
    {
        public bool isTriggered;
        public Collider2D[] triggeredObjects = new Collider2D[1];
        public Trait triggeredTraits;
        public LayerMask triggeredLayers;
    }
}
