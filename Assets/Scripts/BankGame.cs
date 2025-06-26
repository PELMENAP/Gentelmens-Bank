using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

using LitMotion;
using LitMotion.Extensions;

public class BankGame : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private List<Sprite> defimages, openimages;
    [SerializeField] private List<TextMeshProUGUI> players;
    [SerializeField] private List<TMP_InputField> inputsplayer1;
    [SerializeField] private List<AnimatedNumberVisualizer> output;
    [SerializeField] private AnimatedNumberVisualizer mainVisualizer;

    [SerializeField] private List<UIAnimation> vaultAnimators;
    [SerializeField] private UIAnimation objectVaultAnimator;

    [SerializeField] private List<GameObject> curplayerhighlihght;

    public int curpair = -1, curplayer;
    [SerializeField] private TextMeshProUGUI player1name, player2name, winmes, costmes;

    [SerializeField] private GameObject wait, chooseWhoGet, winobj, playerVideo;
    [SerializeField] private RectTransform winTrans;
    [SerializeField] private Material highlightmaterial;

    private bool winnerOpened;

    private void Start() 
    {
        mainVisualizer.gameObject.SetActive(false);
        StartThePair();
        InvokeRepeating("SynctTheText", 2.0f, 0.3f);

        for(int i = 0; i<4; i++)
        {
            output[i].gameObject.SetActive(false);
        }
    }

    public void StartThePair()
    {
        // objectVaultAnimator.PlayForwardAnimation();
        // Clear();
        winnerOpened = false;
        
        playerVideo.SetActive(true);
        winobj.SetActive(false);

        curpair++;
        if(curpair > 3) curpair = 0;

        for(int i = 0; i < 4; i++)
            curplayerhighlihght[i].SetActive(i == curpair);

        curplayer = 0;
        howcuropen = 0;

        // foreach (var item in vaultAnimators)
        // {
        //     item.isOpened = false;
        // }

        player1name.text = players[2 * curpair].text;
        player2name.text = players[2 * curpair + 1].text;
    }
    public void SynctTheText()
    {
        player1name.text = players[2 * curpair].text;
        player2name.text = players[2 * curpair + 1].text;
    }
    public void Clear()
    {
        foreach (var item in vaultAnimators)
        {
            item.isOpened = false;
        }
        objectVaultAnimator.PlayForwardAnimation();
        for(int i = 0; i < images.Count; i++)
        {
            images[i].material = highlightmaterial;
            images[i].sprite = defimages[i];
        }
        mainVisualizer.Clear();
    }


    public void AnimateVault(int number)
    {
        curvault = number;

        if(vaultAnimators[curvault].isOpened) return;

        vaultAnimators[curvault].PlayForwardAnimation();
        vaultAnimators[curvault].gameObject.transform.SetAsLastSibling();

        wait.SetActive(true);
    }

    private void OnAwait()
    {
        LMotion.Create(0f, 0f, 5f)
            .WithOnComplete(OnSecondPlayerGet)
            .RunWithoutBinding();
    }

    private void OnSecondPlayerGet()
    {
        LMotion.Create(winTrans.anchoredPosition, new Vector2(0, -600), 1f)
            .WithEase(Ease.OutQuad)
            .BindToAnchoredPosition(winTrans)
            .AddTo(gameObject);
    }

    int curprize = 0, curvault = 0, howcuropen = 0;

    [SerializeField] private Image[] targetImages;
    [SerializeField] private Color defColor, targetColor;

    public void OpenVault(int num)
    {
        images[curvault].material = null;
        LMotion.Create(0f, 0f, 2f)
            .WithOnComplete(AwaitOpen)
            .RunWithoutBinding();

        if (int.TryParse(inputsplayer1[num].text, out int result))
        {
            curprize = result;
        }
        else
        {
            Debug.LogError("Ошибка преобразования строки в число!");
        }

        for(int i = 0; i < targetImages.Length; i++)
        {
            LMotion.Create(targetImages[i].color, targetColor, 1f)
                .WithEase(Ease.OutQuad)
                .BindToColor(targetImages[i])
                .AddTo(gameObject);
        }   

        wait.SetActive(false);
        chooseWhoGet.SetActive(true);

        mainVisualizer.gameObject.SetActive(true);
        mainVisualizer.AnimateWin(curprize);
        costmes.text = "Выигрыш: " + curprize.ToString();
    }

    private void AwaitOpen()
    {
        images[curvault].sprite = openimages[curvault];
    }

    public void GiveThePrizeToPlayer()
    {   
        if(vaultAnimators[curvault].isAnimating()) return;

        output[curvault].gameObject.SetActive(true);
        output[curvault].DisplayNumberImmediately(curprize);
        mainVisualizer.gameObject.SetActive(false);
        chooseWhoGet.SetActive(false);
        vaultAnimators[curvault].PlayBackwardAnimation();

        for(int i = 0; i < targetImages.Length; i++)
        {
            LMotion.Create(targetImages[i].color, defColor, 1f)
                .WithEase(Ease.OutQuad)
                .BindToColor(targetImages[i])
                .AddTo(gameObject);
        }  
    }

    int lastOpenVault = -1;

    public void PlayerWin(int player)
    {
        playerVideo.SetActive(false);

        if(winnerOpened) 
        {
            winmes.text = "Игрок " + players[2 * curpair + player].text + " получает:";

            LMotion.Create(winTrans.anchoredPosition, new Vector2(0, -330), 1f)
                .WithEase(Ease.OutQuad)
                .WithOnComplete(OnAwait)
                .BindToAnchoredPosition(winTrans)
                .AddTo(gameObject);
            
            return;
        }

        winnerOpened = true;
        // if(vaultAnimators[curvault].isAnimating()) return;
        
        // gotonext.SetActive(true);
        // int win = 0;
        
        winobj.SetActive(true);

        winmes.text = "Игрок " + players[2 * curpair + player].text + " победил!";

        // for(int i = 0; i < 4; i++)
        // {
        //     images[i].material = null;
        //     images[i].sprite = openimages[i];

        //     output[i].text = player == 0 ? inputsplayer1[i].text : inputsplayer2[i].text;

        //     if(!isopened[player, i])
        //     {
        //         lastOpenVault = i;
                
        //         if (int.TryParse(player == 0 ? inputsplayer1[i].text : inputsplayer2[i].text, out int result))
        //         {
        //             win = result;
        //         }

        //         vaultAnimators[i].PlayForwardAnimation();
        //         vaultAnimators[i].gameObject.transform.SetAsLastSibling();
        //     }
        // }

        costmes.text = "";
    }
}