using UnityEngine;

public class CanvasShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f; // Длительность тряски
    [SerializeField] private float shakeMagnitude = 10f; // Сила тряски

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private float currentShakeDuration = 0f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        originalPosition = rectTransform.localPosition;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            rectTransform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude * (currentShakeDuration / shakeDuration);
            
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            currentShakeDuration = 0f;
            rectTransform.localPosition = originalPosition;
        }
    }

    public void Shake()
    {
        originalPosition = rectTransform.localPosition;
        currentShakeDuration = shakeDuration;
    }
}