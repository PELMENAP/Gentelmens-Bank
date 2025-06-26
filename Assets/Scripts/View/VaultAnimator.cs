using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class UIAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform targetObject;
    
    [Header("Animation Settings")]
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private Vector3 targetScale = Vector3.one;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.OutQuad;

    private Vector2 _initialPosition;
    private Vector3 _initialScale;
    private bool _isAnimating;
    public bool isOpened = false;

    public bool isAnimating() => _isAnimating;
    private void Awake()
    {
        _initialPosition = targetObject.anchoredPosition;
        _initialScale = targetObject.localScale;
    }

    public void PlayForwardAnimation()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        LMotion.Create(targetObject.anchoredPosition, targetPosition, animationDuration)
            .WithEase(easeType)
            .BindToAnchoredPosition(targetObject)
            .AddTo(gameObject);
            
        LMotion.Create(targetObject.localScale, targetScale, animationDuration)
            .WithEase(easeType)
            .WithOnComplete(() => _isAnimating = false)
            .BindToLocalScale(targetObject)
            .AddTo(gameObject);
    }

    public void PlayBackwardAnimation()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        isOpened = true;

        LMotion.Create(targetObject.anchoredPosition, _initialPosition, animationDuration)
            .WithEase(easeType)
            .BindToAnchoredPosition(targetObject)
            .AddTo(gameObject);
            
        LMotion.Create(targetObject.localScale, _initialScale, animationDuration)
            .WithEase(easeType)
            .WithOnComplete(() => _isAnimating = false)
            .BindToLocalScale(targetObject)
            .AddTo(gameObject);
    }
}