using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;

using TMPro;
using System.Linq;

public class VisualPresenter : MonoBehaviour
{
    [SerializeField] private GameObject componentA, componentB, componentC;
    [SerializeField] private GameObject visualA, visualB, visualC;
    private IVisualState currentState;

    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject[] imagesNames;
    [SerializeField] private TextMeshProUGUI input;

    [SerializeField] private TextMeshProUGUI[] inputplayers;
    [SerializeField] private TextMeshProUGUI[] inputplaces;
    [SerializeField] private TMP_InputField[] inputprizes;
    [SerializeField] private GameObject[] outputPrizes;
    [SerializeField] private TextMeshProUGUI[] outputText;
    [SerializeField] private Color[] winColors;

    public void Clear()
    {
        endConfetti.SetActive(false);
        foreach (var item in outputPrizes)
        {
            item.SetActive(false);
        }

        foreach(var item in placeConfetti)
        {
            item.SetActive(false);
        }

        foreach(var item in outputText)
        {
            item.text = "";
        }

        inputprizes[0].text = "40000";
        inputprizes[1].text = "30000";
        inputprizes[2].text = "20000";
        inputprizes[3].text = "10000";

        mainConfetti.SetActive(false);
    }
    public void HighPlayer()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive((i + 1).ToString() == input.text);
            imagesNames[i].SetActive((i + 1).ToString() == input.text);
        }
    }
    public void ShowTheLeaderBoard()
    {
        for(int i = 0; i < 4; i++)
        {
            if(inputplaces[i].text == "место")
            {
                for(int j = 0; j < 4; j++)
                    outputPrizes[j].SetActive(false);
                return;
            }
        }

        int[] places = new int[4];

        int[] placePrizes = new int[4];

        for(int i = 0; i < 4; i++)
        {
            if (int.TryParse(inputplaces[i].text, out int result))
            {
                places[i] = result;
            }
        }

        for(int i = 0; i < 4; i++)
        {

            if (int.TryParse(inputprizes[i].text, out int result))
            {
                placePrizes[i] = result;
            }
        }

        int[] prizes = DistributePrizes(places, placePrizes);
        StartTheAnimation(prizes).Forget();
    }

    [SerializeField] private GameObject mainConfetti, endConfetti;
    [SerializeField] private GameObject[] placeConfetti;
    [SerializeField] private AnimatedNumberVisualizer[] output;

    public async UniTask StartTheAnimation(int[] prizes) 
    {
        mainConfetti.SetActive(true);
        await UniTask.Delay(5000);

        for(int i = 0; i < 4; i++)
        {
            outputPrizes[i].SetActive(true);
            output[i].JustDisplay(prizes[i], ShowThePlace, i).Forget();
        }
    }

    private void ShowThePlace(int place)
    {
        outputText[place].text = inputplaces[place].text;
        outputText[place].color = winColors[int.Parse(inputplaces[place].text) - 1];
        placeConfetti[place].SetActive(true);

        if(inputplaces[place].text == "1")
        {
            endConfetti.SetActive(true);
        }
    }

    public void ShowWallpaper()
    {
        SetState(new WallpaperState(componentA, visualA));
    }

    public void ShowMainGame()
    {
        SetState(new MainGameState(componentB, visualB));
    }

    public void ShowChoosingWinner()
    {
        SetState(new ChooseWinnerState(componentC, visualC));
    }

    private void SetState(IVisualState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState();
    }

    public static int[] DistributePrizes(int[] playerPlaces, int[] placePrizes)
    {   
        var groups = playerPlaces.Select((place, index) => new { Place = place, Index = index })
                                .GroupBy(x => x.Place)
                                .OrderBy(g => g.Key);
        
        int[] prizes = new int[4];
        
        foreach (var group in groups)
        {
            int place = group.Key - 1;
            int count = group.Count();
            int totalPrize = 0;
            for (int i = 0; i < count; i++)
            {
                if (place + i < placePrizes.Length)
                {
                    totalPrize += placePrizes[place + i];
                }
            }
            
            int prizePerPlayer = totalPrize / count;
            
            foreach (var player in group)
            {
                prizes[player.Index] = prizePerPlayer;
            }
        }
        
        return prizes;
    }
}