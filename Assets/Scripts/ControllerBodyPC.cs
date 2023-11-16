using UnityEngine;

public class ControllerBodyPC : MonoBehaviour
{
    // fixed links
    public PCMetadata meta;
    public Rigidbody2D body;
    public TriggerVolume ground;
    new public Collider2D collider;

    public float jumpStrength = 10;
    public float walkSpeed = 8;

    public void Awake()
    {
        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;
            //meta.controllerBody = this;
        };
    }

    public void Update()
    {
        var inputData = meta.commandEmitter.data;

        body.velocity = CoreUtilities.SetX(body.velocity, walkSpeed * inputData.move);
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.Action.JUMP:
                if (args.jump && ground.isTriggered)
                {
                    body.velocity = CoreUtilities.SetY(body.velocity, jumpStrength);
                }
                break;
        }
    }
}
