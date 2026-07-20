using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RestartersHandler
{
    private static List<IRestarter> _restarters = new();
    
    public static void Register(IRestarter restarter) => _restarters.Add(restarter);
    public static void Deregister(IRestarter restarter) => _restarters.Remove(restarter);

    public static void Execute()
    {
        if (_restarters.Count == 0)
            return;
        
        foreach (var restarter in _restarters)
        {
            restarter.Restart();
        }
    }
}
