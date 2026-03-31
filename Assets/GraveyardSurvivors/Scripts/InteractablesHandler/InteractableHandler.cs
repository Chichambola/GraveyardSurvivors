using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHandler : MonoBehaviour, IInteractableHandler
{
    [SerializeField] protected InteractableSpawner InteractableSpawner;
    
    protected IPlayerStats Player; 
    
    public void Init(IPlayerStats player)
    {
        if(player == null)
            throw new System.ArgumentNullException("player");
        
        Player = player;
    }
}
