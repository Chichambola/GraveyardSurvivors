using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableHandler : MonoBehaviour
{
    [SerializeField] protected InteractableSpawner InteractableSpawner;
    
    protected IPlayer Player;

    public void Init(IPlayer player)
    {
        if(player == null)
            throw new System.ArgumentNullException("player");
        
        Player = player;
    }

    public void Spawn(Vector3 position) => InteractableSpawner.Spawn(position);
}
