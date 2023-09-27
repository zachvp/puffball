using UnityEngine;

public class ControllerBodyPC : MonoBehaviour
{
    public PCMetadata meta;
    public Rigidbody2D body;
    public TriggerVolume ground;

    public float jumpStrength = 10;
    public float walkSpeed = 8;

    public void Awake()
    {
        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;
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
