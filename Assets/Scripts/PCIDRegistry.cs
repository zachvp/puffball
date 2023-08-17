using System;
using System.Collections.Generic;

public class PCIDRegistry : Singleton<PCIDRegistry>
{
    public Dictionary<Type, int> assignments;

    public PCIDRegistry()
    {
        assignments = new Dictionary<Type, int>();
    }

    public void Register(object associate, Action<int> callback)
    {
        var key = associate.GetType();

        if (assignments.ContainsKey(key))
        {
            assignments[key] += 1;
        }
        else
        {
            assignments.Add(key, 0);
        }

        callback(assignments[key]);
    }
}
