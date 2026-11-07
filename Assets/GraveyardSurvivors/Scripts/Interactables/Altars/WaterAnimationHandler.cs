using PrimeTween;
using UnityEngine;

public class WaterAnimationHandler : MonoBehaviour
{
    [SerializeField] private GameObject _water;
    [SerializeField] private float _scaleDuration = 1f;
    [SerializeField] private Ease _scaleEase;
    [SerializeField] private bool _isShrinking = true;
    
    private const float MaxScale = 1f;
    private float _initialScale;

    public void Init(int count)
    {
        _initialScale = MaxScale / count;
    }
    
    public void StartAnimation(int interactionCount)
    {
        var targetScale = CalculateTargetScale(interactionCount);

        Sequence.Create().Group(Tween.Scale(_water.transform, targetScale, _scaleDuration));
    }

    public void StartAnimation(int interactionCount, Tween tween)
    {
        var targetScale = CalculateTargetScale(interactionCount);
        
        Sequence.Create().Group(Tween.Scale(_water.transform, targetScale, _scaleDuration))
            .Group(tween);
    }
    
    private float CalculateTargetScale(int interactionCount)
    {
        float targetScale = _initialScale;

        if (interactionCount != 0)
        {
            if (_isShrinking)
            {
                targetScale = _initialScale / interactionCount;
            }
            else
            {
                targetScale = _initialScale * interactionCount;
            }
        }
        
        return targetScale;
    }
}