using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Extensions;

public class BlinkManager : MonoBehaviour
{
    [SerializeField] private RectTransform leftBlink, rightBlink, rotateBlink;
    [SerializeField] private Image leftImageBlink, rightImageBlink, allBlink;

    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private Vector3 defRotation;

    [SerializeField] private Color defColor, targetColor;
    

    void Start()
    {
        leftBlink.rotation = Quaternion.Euler(defRotation);
        rightBlink.rotation = Quaternion.Euler(defRotation);
    }

    public void MakeBlink()
    {
        leftBlink.rotation = Quaternion.Euler(defRotation);
        rightBlink.rotation = Quaternion.Euler(defRotation);
        leftBlink.localScale = Vector3.zero;
        rightBlink.localScale = Vector3.zero;
        rotateBlink.localScale = Vector3.zero;

        leftImageBlink.color = targetColor;
        rightImageBlink.color = targetColor;
        allBlink.color = targetColor;

        LMotion.Create(rotateBlink.localScale, new Vector3(1, 1, 1), animationDuration)
            .WithEase(easeCurve)
            .BindToLocalScale(rotateBlink)
            .AddTo(gameObject);



        LMotion.Create(leftBlink.rotation, Quaternion.Euler(new Vector3(0, 0, -90)), animationDuration)
            .WithEase(easeCurve)
            .BindToRotation(leftBlink)
            .AddTo(gameObject);

        LMotion.Create(leftBlink.localScale, new Vector3(1, 1, 1), animationDuration)
            .WithEase(easeCurve)
            .BindToLocalScale(leftBlink)
            .AddTo(gameObject);

        LMotion.Create(leftImageBlink.color, defColor, animationDuration)
            .WithEase(Ease.OutQuad)
            .BindToColor(leftImageBlink)
            .AddTo(gameObject);



        LMotion.Create(rightBlink.rotation, Quaternion.Euler(new Vector3(0, 180, -90)), animationDuration)
            .WithEase(easeCurve)
            .WithOnComplete(NextsStep)
            .BindToRotation(rightBlink)
            .AddTo(gameObject);
        
        LMotion.Create(rightBlink.localScale, new Vector3(1, 1, 1), animationDuration)
            .WithEase(easeCurve)
            .BindToLocalScale(rightBlink)
            .AddTo(gameObject);

        LMotion.Create(rightImageBlink.color, defColor, animationDuration)
            .WithEase(Ease.OutQuad)
            .BindToColor(rightImageBlink)
            .AddTo(gameObject);
    }

    private void NextsStep()
    {
        LMotion.Create(allBlink.color, defColor, animationDuration)
            .WithEase(Ease.OutQuad)
            .WithOnComplete(End)
            .BindToColor(allBlink)
            .AddTo(gameObject);
    }

    private void End()
    {
        rotateBlink.localScale = Vector3.zero;
        leftImageBlink.color = targetColor;
        rightImageBlink.color = targetColor;
        allBlink.color = targetColor;
    }
}
