using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    private int _minDelay = 2;
    private int _maxDelay = 5;
    private Timer _timer;
    
    public DieState(CharacterBase stateHandler, Animator animator) : base(stateHandler, animator) { }

    public override void DoEnter()
    {
        int delay = Random.Range(_minDelay, _maxDelay);
        
        _timer = new IntervalTimer(delay);
        _timer.Stopped += DoExit;
        _timer.Start();
        
        Animator.CrossFade(s_Death, CrossFadeDuration);
    }

    public override void DoExit()
    {
        _timer.Stopped -= DoExit;
        _timer?.Stop();
        StateHandler.Release();
    }
}
