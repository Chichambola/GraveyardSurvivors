using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private Ease _ease;
    [SerializeField] private bool _isUnscaledTime = true;
    
    private TweenSettings<float> _settings;
    private Tween _tween;

    private void Awake()
    {
        _settings = new TweenSettings<float>();
    }

    private void OnEnable()
    {
        _settings.settings.ease = _ease;
        _settings.settings.useUnscaledTime = _isUnscaledTime;
    }

    private void OnDestroy()
    {
        if (_tween.isAlive)
        {
            _tween.Stop();
        }
    }
    
    public async UniTask UpdateSlider(float value)
    {
        if(_tween.isAlive)
            _tween.Stop();
        
        _settings.startValue = _slider.value;
        _settings.settings.duration = _duration;
        _settings.endValue = value;
        
        _tween = Tween.UISliderValue(_slider, _settings).OnComplete(() => _tween.Stop());

        await _tween;
    }
    
    public void Set(float currentValue, float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = currentValue;
    }
}