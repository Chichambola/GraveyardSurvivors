using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public const string Speed = nameof(Speed);
    
    [SerializeField] private Animator _controller;
    [SerializeField] private float _dampTime = 0.1f;

    public void PlayRunAnimation(float speed)
    {
        speed = Mathf.Max(speed, 0);
        
        _controller.SetFloat(Speed, speed, _dampTime, Time.deltaTime);
    }
}
