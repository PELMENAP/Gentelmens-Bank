using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class FloatingAnimationAlt : MonoBehaviour
{
    [SerializeField] private float animTime = 0.5f;
    [SerializeField] private float scaleFactor = 0.8f;

    private void Start()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        LMotion.Create(originalScale, targetScale, animTime)
            .WithEase(Ease.InOutSine)
            .WithLoops(-1, LoopType.Yoyo)
            .BindToLocalScale(transform)
            .AddTo(this);
    }
}