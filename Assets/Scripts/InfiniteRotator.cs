using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class InfiniteRotation : MonoBehaviour
{
    [SerializeField] private float _duration = 2f;
    [SerializeField] private Ease _easeType = Ease.Linear;
    [SerializeField] private Vector3 _rotationAxis = Vector3.forward;

    private void Start()
    {
        LMotion.Create(0f, 360f, _duration)
            .WithEase(_easeType)
            .WithLoops(-1, LoopType.Restart)
            .Bind(x => 
            {
                transform.rotation = Quaternion.Euler(
                    _rotationAxis * x
                );
            });
    }
}