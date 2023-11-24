using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Roller : MonoBehaviour
{
    [SerializeField]
    private CandyParams bomb,
        rocket;

    [SerializeField]
    private float roomModifier = 1;

    [SerializeField]
    private float roomNumber;

    [SerializeField]
    private float roomChanse = 0.25f;

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

        playerBalance = PlayerPrefs.GetFloat("PlayerBalance", StaticParams.StartMoney);
        bet = 0.1f;
        betText.text = bet.ToString();
        balanceText.text = playerBalance.ToString();
        balanceText.text = balanceText.text.Replace(".", ",");
        ls.Hide();
        if (PlayerPrefs.GetInt("Music", 1) == 1)
        {
            AudioSource musicPlayer = gameObject.AddComponent<AudioSource>();
            musicPlayer.clip = slotMusic;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
    }

    public void Roll()
    {
        if (!rolling)
        {
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioSource.PlayClipAtPoint(spinBtn, Vector2.zero);
                AudioSource.PlayClipAtPoint(spin, Vector2.zero);
            }

            playerBalance -= bet;
            playerBalance = Mathf.Round(playerBalance * 100.0f) / 100.0f;
            balanceText.text = playerBalance.ToString();
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
            if (Random.value <= roomChanse)
            {
                CandyParams candyParam = candyParams[0];
                float rollChanse = Random.value;
                foreach (var param in candyParams)
                {
                    if (rollChanse <= param.chanse)
                    {
                        candyParam = param;
                        if (param.chanse <= 0.1f)
                        {
                            float extraChanse = Random.value;
                            if (extraChanse < 0.3f && PlayerPrefs.GetInt("Bomb") > 0)
                            {
                                candyParam = bomb;
                                PlayerPrefs.SetInt("Bomb", 0);
                                PlayerPrefs.Save();
                            }
                            if (extraChanse < 0.2f && PlayerPrefs.GetInt("Rocket") > 0)
                            {
                                candyParam = rocket;
                                PlayerPrefs.SetInt("Rocket", 0);
                                PlayerPrefs.Save();
                            }
                        }
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

    [SerializeField]
    private AudioClip winSound,
        spin,
        spinBtn,
        upBtn,
        downBtn,
        slotMusic;

    private void CheckConnects()
    {
        float prize = 0;
        float totalPrize = 0;

        foreach (var elementIndex in matchedLines)
        {
            prize =
                (1 - columns[0].candyColumn[elementIndex].candyParam.chanse) * (bet * roomModifier);
            prize = Mathf.Round(prize * 100.0f) / 100.0f;
            foreach (var column in columns)
            {
                column.candyColumn[elementIndex].SetConnected(prize);
            }
            prize *= StaticParams.MaxHorizontal;
            prize = Mathf.Round(prize * 100.0f) / 100.0f;
            if (columns[0].candyColumn[elementIndex].candyParam.candyId == 150)
            {
                prize = 750;
            }
            if (columns[0].candyColumn[elementIndex].candyParam.candyId == 200)
            {
                prize = 1500;
            }
            totalPrize += prize;
        }
        if (totalPrize > 0)
        {
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
            {
                AudioSource.PlayClipAtPoint(winSound, Vector2.zero);
            }
            if (PlayerPrefs.GetInt("Vibro", 1) == 1)
            {
                Handheld.Vibrate();
            }
        }
        playerBalance += totalPrize;
        playerBalance = Mathf.Round(playerBalance * 100.0f) / 100.0f;
        balanceText.text = playerBalance.ToString();
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
        betText.text = bet.ToString();
        betText.text = betText.text.Replace(".", ",");
        HideInfo();
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            AudioSource.PlayClipAtPoint(upBtn, Vector2.zero);
        }
    }

    public void DownBet()
    {
        bet -= 0.1f;
        if (bet < 0.1f)
        {
            bet = 0.1f;
        }
        bet = Mathf.Round(bet * 100.0f) / 100.0f;
        betText.text = bet.ToString();
        betText.text = betText.text.Replace(".", ",");
        HideInfo();
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            AudioSource.PlayClipAtPoint(downBtn, Vector2.zero);
        }
    }

    public void ShowInfo()
    {
        if (!showInfo)
        {
            showInfo = true;
            infoWindow.DOAnchorPosX(0, 0.3f).Play();
            foreach (var candyInfo in candiesInfo)
            {
                candyInfo.SetPrize(bet * roomModifier);
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
                StaticParams.PrevScene = roomNumber;
                UnityEngine.SceneManagement.SceneManager.LoadScene("GuestRoom");
            });
        changeScene.Restart();
    }
}
