using UnityEngine;

public class ActorBall : MonoBehaviour
{
    // links
    public CoreBody body;
    public Collider2D mainCollider;
    public JointDynamicAnchor joint;
    public TriggerVolume trigger;

    // state
    public State state;

    // one-time write state
    private int initLayer;

    public void Awake()
    {
        initLayer = gameObject.layer;
    }

    public void Update()
    {
        if (state == State.PICKUP)
        {
            gameObject.layer = CoreConstants.LAYER_PROP;
        }
        else if (!CoreUtilities.LayerExistsInMask(CoreConstants.LAYER_PLAYER, trigger.triggeredLayers))
        {
            gameObject.layer = initLayer;
        }
    }

    public void Grab(Transform parent)
    {
        joint.anchor = parent;
        joint.enabled = true;

        gameObject.layer = CoreConstants.LAYER_PROP;
        state = State.PICKUP;
    }

    public void Drop()
    {
        joint.enabled = false;
        joint.anchor = null;
        state = State.NONE;
    }

    public void Throw(Vector2 v)
    {
        Drop();

        body.velocity = v;
    }

    public enum State
    {
        NONE,
        PICKUP,
    }
}
