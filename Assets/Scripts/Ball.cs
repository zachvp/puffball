using UnityEngine;

public class Ball : MonoBehaviour
{
    public CoreBody body;
    public Collider2D mainCollider;
    public GameObject pickup;
    public GameObject held;
    public GameObject released;
    public MovementFollowTransform heldMovement;

    public Vector2 assistThrow = new Vector2(50, 50);

    // Activate the held object, deactivate the pickup object.
    public void Grab(Transform holdAnchor)
    {
        heldMovement.root = holdAnchor;
        body.StopVertical();
        body.FreezeRotation();
        
        held.SetActive(true);
        pickup.SetActive(false);
    }

    // Activate the temp release object, deactivate the held object.
    public void ActivateRelease()
    {
        held.SetActive(false);
        released.transform.position = held.transform.position;
        released.SetActive(true);

        body.ResetVertical();
        body.UnfreezeRotation();
    }

    // Deactivate the released object, activate the pickup object.
    public void ActivatePickup()
    {
        released.SetActive(false);
        pickup.SetActive(true);

        body.ResetVertical();
        body.UnfreezeRotation();

        pickup.transform.position = released.transform.position;
    }

    // Activate the pickup object, deactivate the temp released object.
    public void ThrowReset()
    {
        pickup.transform.position = released.transform.position;
        pickup.SetActive(true);
        released.gameObject.SetActive(false);
    }

    public void Shoot(Vector2 baseVelocity, Vector2 inputDirection, bool inputBonus, bool jumpBonus)
    {
        ActivateRelease();

        var modVelocity = inputDirection * baseVelocity.magnitude * 1.5f + assistThrow;

        // todo: adjust closeness to "magic shot" depending on context
        //      + boost if close to jump peak (+15)
        //      + boost if at jump peak (+30)
        //      + boost if right stick gesture matches shot (+30)
        //      + boost if right stick gesture matches follow thru (+20)
        //      + boost if have dribble stack of TBD (+20)
        var shotMagic = inputBonus ? 50f : 0f;
        shotMagic += jumpBonus ? 50f : 0f;

        if (shotMagic > 0)
        {
            var magicVel = Vector2.zero;
            var toTarget = SceneRefs.instance.targetGoal.transform.position - released.transform.position;
            var a = new Vector2(-body.body.drag, body.originalGravity * Physics2D.gravity.y - body.body.drag);

            magicVel.x = 2 * (toTarget.x / 2.55f);
            magicVel.y = Mathf.Sqrt(Mathf.Pow(baseVelocity.y, 2) - (2 * a.y * toTarget.y));

            if (float.IsNaN(magicVel.y))
            {
                magicVel.y = 0;
                magicVel.x = toTarget.x;
                Debug.LogWarning("magicVelocity.y is NaN");
            }

            //var yFudge = 1 / (toTarget.y / target to floor dist)
            var baseFudge = 1 + body.body.drag;
            var yFudge = 0.8f / Mathf.Max(0.25f, toTarget.y / SceneRefs.instance.targetGoal.distToFloor);

            magicVel.x *= baseFudge;
            magicVel.y *= Mathf.Max(baseFudge, yFudge);
            shotMagic = Mathf.Min(100, shotMagic);
            //modVelocity = Vector2.Lerp(modVelocity, magicVel, shotMagic / 100);
            modVelocity = magicVel;
            //modVelocity = magicVel;
            //Debug.Log($"precise jump shot! | {magicVel}; fudge denom: {toTarget.y / measureFloorToTarget.transform.localScale.y}");
            //Debug.Log($"shot magic: {shotMagic / 100}");
        }

        body.Trigger(modVelocity);
    }

    // Deactivate the held object, activate the temp released object.
    public void ThrowAttack(Vector2 baseVelocity, Vector2 inputDirection)
    {
        ActivateRelease();

        //Debug.Log($"up: {dotUp}");
        //Debug.Log($"right: {dotRight}");

        var modVelocity = inputDirection * baseVelocity.magnitude * 1.5f + assistThrow;

        if (inputDirection.x < 0)
        {
            modVelocity.x -= 2 * assistThrow.x;
        }

        body.Trigger(modVelocity);
    }

    public void Dribble(Vector2 baseVelocity)
    {
        ActivateRelease();

        var modVelocity = new Vector2(baseVelocity.x, -180);

        Debug.Log("dribble");

        body.Trigger(modVelocity);
    }
}
