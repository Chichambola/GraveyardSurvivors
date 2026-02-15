using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChanceHandlerBase : InteractableHandler
{
    [SerializeField] protected RarityEvaluator RarityEvaluator;
    [SerializeField] protected ItemsHandler ItemsHandler;
}
