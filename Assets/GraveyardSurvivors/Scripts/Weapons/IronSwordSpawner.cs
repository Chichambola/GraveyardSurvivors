using System;
using System.Collections.Generic;

public class IronSwordSpawner : Spawner<Blade>
{
    public event Action ReleasedAllObjects;
    
    private const int Capacity = 60;
    private float _duration;
    private HashSet<Blade> _blades;

    protected override void Awake()
    {
        base.Awake();
        
        _blades = new HashSet<Blade>(Capacity);
    }

    public void Spawn(float duration)
    {
        _duration = duration;
        
        GetObject();
    }
    
    public void Upgrade(float gain)
    {
        foreach (var blade in _blades)
        {
            blade.IncreaseSize(gain);
        }
    }


    protected override void ActionOnGet(Blade blade)
    {
        blade.CanBeReleased += Release;
        
        base.ActionOnGet(blade);
        
        _blades.Add(blade);
        ActiveObjects.Add(blade);
        
        blade.SetVisibility(_duration);

        blade.StartSpin();
    }

    protected override void ActionOnRelease(Blade blade)
    {
        ActiveObjects.Add(blade);

        if (ActiveObjects.Count == 0)
            ReleasedAllObjects?.Invoke();
        
        base.ActionOnRelease(blade);
        
        blade.ResetCharacteristics();
        
        blade.CanBeReleased -= Release;
    }

    protected override void ActionOnDestroy(Blade blade)
    {
        blade.CanBeReleased -= Release;
        
        _blades.Remove(blade);
        
        base.ActionOnDestroy(blade);
    }
}