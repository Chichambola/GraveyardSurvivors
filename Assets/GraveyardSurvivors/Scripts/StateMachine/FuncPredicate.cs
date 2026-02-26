using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncPredicate : IPredicate
{
    private readonly Func<bool> _func;

    public FuncPredicate(Func<bool> func)
    {
        _func = func;
    }
    
    public bool Evaluate()
    {
        return _func.Invoke();
    }
}
