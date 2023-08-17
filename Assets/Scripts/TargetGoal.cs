using System;
using UnityEngine;

public class TargetGoal : MonoBehaviour
{
    public Transform root;
    public float distToFloor;

    public void Start()
    {
        distToFloor = ComputeDistToFloor();
    }

    public float ComputeDistToFloor()
    {
        var distanceGoalToFloor = 0f;
        var filter = new ContactFilter2D();
        var results = new RaycastHit2D[4];
        var cast = Physics2D.Raycast(root.position, Vector2.down, filter, results);

        for (var i = 0; i < cast; i++)
        {
            distanceGoalToFloor = Mathf.Max(distanceGoalToFloor, results[i].distance);
        }

        return distanceGoalToFloor;
    }
}
