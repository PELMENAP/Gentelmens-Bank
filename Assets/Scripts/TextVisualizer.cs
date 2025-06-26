using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;

using System;

public class AnimatedNumberVisualizer : MonoBehaviour
{
    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private Image[] digitRenderers;
    [SerializeField] private float animationDuration = 5f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve easeType = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve easeTypeForWin = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private RectTransform targetObject, winObject, leftLight, rightLight, textView;
    [SerializeField] private Image winImage;
    [SerializeField] private Sprite[] winRenderers;

    [SerializeField] private Vector2 winDefPosition, winTargetPosition, leftDefPosition, leftTargetPosition, rightDefPosition, rightTargetPosition;
    [SerializeField] private GameObject confetti, flares;

    [SerializeField] private CanvasShake canvasShake;
    [SerializeField] private BlinkManager blinkManager;

    private Vector3 targetScale = new Vector3(100, 100, 1);

    private int _currentDisplayedNumber;
    private CancellationTokenSource _animationCts;

    public void Clear()
    {
        targetObject.localScale = targetScale;
        winObject.localScale = targetScale / 10;
        winObject.anchoredPosition = winDefPosition;
        leftLight.anchoredPosition = leftDefPosition;
        rightLight.anchoredPosition = rightDefPosition;
        confetti.SetActive(false);
        flares.SetActive(false);
    }

    public async UniTask AnimateWin(int output)
    {
        DisplayNumberImmediately(0);
        winImage.sprite = winRenderers[FindClosestIndex(output)];
        blinkManager.MakeBlink();
        AppearTheAnimator();
        await AnimateNumberAsync(output);
    }

    public int[] numbers = { 10000, 15000, 25000, 35000};

    public int FindClosestIndex(int input)
    {

        int closestIndex = 0;
        int smallestDifference = Mathf.Abs(input - numbers[0]);

        for (int i = 1; i < numbers.Length; i++)
        {
            int currentDifference = Mathf.Abs(input - numbers[i]);
            if (currentDifference < smallestDifference)
            {
                smallestDifference = currentDifference;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public void AppearTheAnimator()
    {
        textView.localScale = Vector3.zero;
        LMotion.Create(textView.localScale, Vector3.one, 1f)
            .WithEase(easeTypeForWin)
            .BindToLocalScale(textView)
            .AddTo(gameObject);
    }

    public async UniTask AnimateNumberAsync(int targetNumber)
    {
        targetObject.localScale = targetScale;
        winObject.localScale = targetScale / 10;
        winObject.anchoredPosition = winDefPosition;
        leftLight.anchoredPosition = leftDefPosition;
        rightLight.anchoredPosition = rightDefPosition;
        confetti.SetActive(false);

        await UniTask.Delay(3500);

        LMotion.Create(winObject.localScale, Vector3.one, 1f)
            .WithEase(easeTypeForWin)
            .BindToLocalScale(winObject)
            .AddTo(gameObject);
        
        LMotion.Create(winObject.anchoredPosition, winTargetPosition, 1f)
            .WithEase(easeTypeForWin)
            .BindToAnchoredPosition(winObject)
            .AddTo(gameObject);

        await UniTask.Delay(1000);
        
        canvasShake.Shake();

        await UniTask.Delay(500);

        LMotion.Create(targetObject.localScale, Vector3.one, animationDuration)
            .WithEase(easeType)
            .BindToLocalScale(targetObject)
            .AddTo(gameObject);

        LMotion.Create(leftLight.anchoredPosition, leftTargetPosition, animationDuration / 2)
            .WithEase(easeCurve)
            .BindToAnchoredPosition(leftLight)
            .AddTo(gameObject);

        LMotion.Create(rightLight.anchoredPosition, rightTargetPosition, animationDuration / 2)
            .WithEase(easeCurve)
            .BindToAnchoredPosition(rightLight)
            .AddTo(gameObject);

        await UniTask.Delay(500);

        flares.SetActive(true);
        _animationCts?.Cancel();
        _animationCts = new CancellationTokenSource();

        if (targetNumber > GetMaxDisplayableNumber())
        {
            Debug.LogWarning("Number is too big for the display!");
            targetNumber = GetMaxDisplayableNumber();
        }

        int startNumber = _currentDisplayedNumber;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = easeCurve.Evaluate(elapsed / animationDuration);
            int currentNumber = (int)Mathf.Lerp(startNumber, targetNumber, progress);
            
            UpdateNumberDisplay(currentNumber);
            await UniTask.Yield(_animationCts.Token);
        }

        _currentDisplayedNumber = targetNumber;
        UpdateNumberDisplay(targetNumber);

        flares.SetActive(false);
        confetti.SetActive(true);
    }

    public async UniTask JustDisplay(int targetNumber, Action<int> action, int place)
    {
        _animationCts?.Cancel();
        _animationCts = new CancellationTokenSource();

        int startNumber = _currentDisplayedNumber;
        float elapsed = 0f;

        float time = targetNumber / 10000f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float progress = easeCurve.Evaluate(elapsed / time);
            int currentNumber = (int)Mathf.Lerp(startNumber, targetNumber, progress);
            
            UpdateNumberDisplay(currentNumber);
            await UniTask.Yield(_animationCts.Token);
        }

        _currentDisplayedNumber = targetNumber;
        UpdateNumberDisplay(targetNumber);

        action?.Invoke(place);
    }


    public void DisplayNumberImmediately(int number)
    {
        if (number > GetMaxDisplayableNumber())
        {
            Debug.LogWarning("Number is too big for the display!");
            number = GetMaxDisplayableNumber();
        }
        _currentDisplayedNumber = number;
        UpdateNumberDisplay(number);
    }

    private void UpdateNumberDisplay(int number)
    {
        string numberStr = number.ToString();

        for (int i = 0; i < digitRenderers.Length; i++)
        {
            digitRenderers[i].gameObject.SetActive(false);
        }

        int rendererIndex = digitRenderers.Length - 1;
        for (int i = numberStr.Length - 1; i >= 0; i--)
        {
            if (rendererIndex < 0) break;

            int digit = int.Parse(numberStr[i].ToString());
            digitRenderers[rendererIndex].gameObject.SetActive(true);
            digitRenderers[rendererIndex].sprite = numberSprites[digit];
            rendererIndex--;
        }
    }
    

    private int GetMaxDisplayableNumber()
    {
        return (int)Mathf.Pow(10, digitRenderers.Length) - 1;
    }
}