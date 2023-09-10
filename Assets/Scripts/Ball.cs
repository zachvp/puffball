using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    // links
    public CoreBody body;
    public Collider2D mainCollider;

    public JointDynamicAnchor joint;
    public TriggerVolume trigger;

    public Vector2 assistThrow = new Vector2(4, 4);

    private int initLayer;

    public void Awake()
    {
        initLayer = gameObject.layer;
    }

    public void Grab(Transform parent)
    {
        //transform.parent = parent;
        //transform.localPosition = Vector3.zero;
        //body.StopVertical();

        joint.anchor = parent;
        joint.enabled = true;

        gameObject.layer = CoreConstants.LAYER_PROP;
    }

    public void Drop()
    {
        //transform.parent = null;
        //body.ResetVertical();

        joint.enabled = false;
        joint.anchor = null;

        StartCoroutine(CheckOverlap());
    }

    public void Throw(Vector2 v)
    {
        Drop();

        body.velocity = new Vector2(assistThrow.x * v.x, assistThrow.y * v.y);
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
