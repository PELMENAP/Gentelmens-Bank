using UnityEngine;
using UnityEngine.UI;

using LitMotion;
using LitMotion.Extensions;

public class VisualSlotUI : MonoBehaviour 
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite defSprite;
    private float animTime = 0.2f;
    private bool isProcessAnimation = false;
    public void ChangeTheViewSprite(Sprite newSprite = null) 
    {
        if(isProcessAnimation) return;
        
        isProcessAnimation = true;
        var sequence = LSequence.Create();

        sequence.Append(LMotion.Create(1f, 0f, animTime)
            .WithOnComplete(() =>
            {
                if (icon != null)
                    icon.sprite = newSprite == null ? defSprite : newSprite;
            })
            .BindToLocalScaleX(transform));

        sequence.Append(LMotion.Create(0f, 1f, animTime)
            .WithOnComplete(() => isProcessAnimation = false)
            .BindToLocalScaleX(transform));

        sequence.Run();
    }
}