using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    private float _delay = 1f;
    private Timer _timer;
    
    public DieState(CharacterBase stateHandler, Animator animator) : base(stateHandler, animator) { }

    public override void DoEnter()
    {
        _timer = new IntervalTimer(_delay);
        _timer.TimerStopped += DoExit;
        _timer.Start();
        
        Animator.CrossFade(s_Death, CrossFadeDuration);
    }

    public override void DoExit()
    {
        _timer.TimerStopped -= DoExit;
        _timer?.Stop();
        StateHandler.Release();
    }
}
