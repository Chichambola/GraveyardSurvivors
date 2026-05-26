using DG.Tweening;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;
    private float _initialSpeed;

    public float Speed => _speed;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _initialSpeed = _speed;
    }
    
    public void MoveDyDirection(Vector3 direction, float speedMultiplier = 0f)
    {
        float currentSpeed = _speed.AddPercentToNumber(speedMultiplier);
        
        transform.position += direction * (currentSpeed * Time.deltaTime);
    }

    public void MoveToPosition(Vector3 targetPosition, float speedMultiplier = 0f)
    {
        float currentSpeed = _speed.AddPercentToNumber(speedMultiplier);
    
        Vector3 adjustedTarget = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        
        transform.position = Vector3.MoveTowards(transform.position, adjustedTarget, currentSpeed * Time.deltaTime);
    }
    
    public void SetSpeed(float speed)
    {
        if (speed < 0)
        {
            speed = 0;
        }
        
        _speed = speed;
    }

    public void ResetSpeed() => _speed = _initialSpeed;
}
