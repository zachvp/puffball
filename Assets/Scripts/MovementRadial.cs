using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MovementRadial : MonoBehaviour, IControlPC
{
    public PCMetadata metadata;
    public CoreBody body;  // todo: remove
    public Transform root;
    public Transform target;
    public float range;

    public void Awake()
    {
        //metadata.onInitialized += () =>
        //{
        //    //metadata.commandEmitter.onPCCommand += HandleCommand;
            
        //};

        //Signals.instance.onPCCommand += HandleCommand;
        
    }


    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.MOVE_HAND:
                RadialPosition(args.vVec2);
                break;
        }
    }

    public Vector3 RadialPosition(Vector2 input)
    {
        var inputVector3 = new Vector3(input.x, input.y, 0);
        var newPos = root.position + (inputVector3 * range);

        if (body)
        {
            body.position = newPos;
            //body.MoveKinematic(newPos);
        }
        else
        {
            //transform.position = newPos;
            target.position = newPos;
        }

        return newPos;
    }

    public void Reset()
    {
        target.position = root.position;
    }
}
