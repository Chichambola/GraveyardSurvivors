using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHandler : MonoBehaviour, IInteractableHandler
{
    protected IPlayer Player; 
    
    public void Init(IPlayer player)
    {
        if(player == null)
            throw new System.ArgumentNullException("player");
        
        Player = player;
    }
}
