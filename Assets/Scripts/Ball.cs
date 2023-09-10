using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    // links
    public CoreBody body;
    public Collider2D mainCollider;

    public JointDynamicAnchor joint;
    public TriggerVolume trigger;

    public Vector2 assistThrow = new Vector2(50, 50);

    private int initLayer;

    public void Awake()
    {
        initLayer = gameObject.layer;
    }

    public void GrabNew(Transform parent)
    {
        joint.anchor = parent;
        joint.enabled = true;
        gameObject.layer = CoreConstants.LAYER_PROP;
    }

    public void Drop()
    {
        joint.enabled = false;
        joint.anchor = null;

        StartCoroutine(CheckOverlap());
    }

    public IEnumerator CheckOverlap()
    {
        // while the ball is overlapping some player...
        while (CoreUtilities.LayerExistsInMask(CoreConstants.LAYER_PLAYER, trigger.triggeredLayers))
        {
            yield return null;
        }
        gameObject.layer = initLayer;
    }
}
