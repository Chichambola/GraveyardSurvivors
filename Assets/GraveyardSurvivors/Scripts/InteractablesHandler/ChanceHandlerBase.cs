using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceHandlerBase : MonoBehaviour
{
    [SerializeField] protected Player Player;
    [SerializeField] protected RarityEvaluator RarityEvaluator;
    [SerializeField] protected ItemsHandler ItemsHandler;
}
