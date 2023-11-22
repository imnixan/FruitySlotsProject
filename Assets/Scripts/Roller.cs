using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Roller : MonoBehaviour
{
    [SerializeField]
    private CandyParams[] candyParams;

    [SerializeField]
    private TextMeshProUGUI betText,
        balanceText;

    [SerializeField]
    private RectTransform infoWindow,
        pauseWindow;

    [SerializeField]
    private CandyInfo[] candiesInfo;

    [SerializeField]
    private CandiesColumn[] columns;
    private int finishedLines;
    private bool rolling;
    private List<int> matchedLines = new List<int>();

    [SerializeField]
    private LoadingScreen ls;

    private float bet;
    private float playerBalance;

    private bool showInfo;
    private bool showPause;

    private void Start()
    {
        columns = GetComponentsInChildren<CandiesColumn>();
        foreach (var column in columns)
        {
            column.InitStartCandies(candyParams);
        }

        playerBalance = PlayerPrefs.GetFloat("PlayerBalance", 500);
        bet = 0.1f;
        betText.text = bet.ToString() + "$";
        balanceText.text = playerBalance.ToString() + "$";
        balanceText.text = balanceText.text.Replace(".", ",");
        ls.Hide();
    }

    public void Roll()
    {
        if (!rolling)
        {
            playerBalance -= bet;
            playerBalance = Mathf.Round(playerBalance * 100.0f) / 100.0f;
            balanceText.text = playerBalance.ToString() + "$";
            balanceText.text = balanceText.text.Replace(".", ",");
            rolling = true;
            finishedLines = 0;
            foreach (var line in columns)
            {
                line.AddRandomElements(candyParams);
            }
            RandomiseLines();
            foreach (var line in columns)
            {
                line.RollLine();
            }
        }
        HideInfo();
    }

    private void RandomiseLines()
    {
        matchedLines.Clear();
        for (int lineIndex = 0; lineIndex < StaticParams.MaxVertical; lineIndex++)
        {
            if (Random.value <= StaticParams.LineChanse)
            {
                CandyParams candyParam = candyParams[0];
                float rollChanse = Random.value;
                foreach (var param in candyParams)
                {
                    if (rollChanse <= param.chanse)
                    {
                        candyParam = param;
                    }
                }
                foreach (var column in columns)
                {
                    column.SetElement(lineIndex, candyParam);
                }
                matchedLines.Add(lineIndex);
            }
        }
    }

    public void LineFinished()
    {
        finishedLines++;
        if (finishedLines == columns.Length - 1)
        {
            rolling = false;
            CheckConnects();
        }
    }

    private void CheckConnects()
    {
        float prize = 0;
        float totalPrize = 0;
        foreach (var elementIndex in matchedLines)
        {
            prize = (1 - columns[0].candyColumn[elementIndex].candyParam.chanse) * bet;
            prize = Mathf.Round(prize * 100.0f) / 100.0f;
            foreach (var column in columns)
            {
                column.candyColumn[elementIndex].SetConnected(prize);
            }
            prize *= StaticParams.MaxHorizontal;
            prize = Mathf.Round(prize * 100.0f) / 100.0f;
            totalPrize += prize;
        }
        playerBalance += totalPrize;
        playerBalance = Mathf.Round(playerBalance * 100.0f) / 100.0f;
        balanceText.text = playerBalance.ToString() + "$";
        balanceText.text = balanceText.text.Replace(".", ",");
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("PlayerBalance", playerBalance);
        PlayerPrefs.Save();
    }

    public void UpBet()
    {
        bet += 0.1f;
        if (bet > 10)
        {
            bet = 10f;
        }
        if (bet > playerBalance)
        {
            bet = playerBalance;
        }
        bet = Mathf.Round(bet * 100.0f) / 100.0f;
        betText.text = bet.ToString() + "$";
        betText.text = betText.text.Replace(".", ",");
        HideInfo();
    }

    public void DownBet()
    {
        bet -= 0.1f;
        if (bet < 0.1f)
        {
            bet = 0.1f;
        }
        bet = Mathf.Round(bet * 100.0f) / 100.0f;
        betText.text = bet.ToString() + "$";
        betText.text = betText.text.Replace(".", ",");
        HideInfo();
    }

    public void ShowInfo()
    {
        if (!showInfo)
        {
            showInfo = true;
            infoWindow.DOAnchorPosX(0, 0.3f).Play();
            foreach (var candyInfo in candiesInfo)
            {
                candyInfo.SetPrize(bet);
            }
        }
        else
        {
            HideInfo();
        }
    }

    public void ShowPause()
    {
        if (!showPause)
        {
            showPause = true;
            pauseWindow.DOAnchorPosX(0, 0.3f).Play();
        }
    }

    private void HideInfo()
    {
        if (showInfo)
        {
            showInfo = false;
            infoWindow.DOAnchorPosX(2000, 0.3f).Play();
        }
        HidePause();
    }

    public void HidePause()
    {
        if (showPause)
        {
            showPause = false;
            pauseWindow.DOAnchorPosX(-2000, 0.3f).Play();
        }
    }

    public void Menu()
    {
        Sequence changeScene = DOTween.Sequence();
        changeScene
            .PrependCallback(() =>
            {
                ls.Show();
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                StaticParams.FromGame = true;
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu");
            });
        changeScene.Restart();
    }
}
