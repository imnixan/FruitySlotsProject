using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GuestRoom : MonoBehaviour
{
    [SerializeField]
    private RectTransform shopWindow,
        shopButton,
        pinkLoli,
        greenLoli,
        blueLoli,
        shopText;

    [SerializeField]
    private TextMeshProUGUI balanceText;

    private AudioSource mus;

    [SerializeField]
    private LoadingScreen pinkScreen,
        greenScreen,
        blueScreen;

    private void Start()
    {
        mus = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("Music", 1) == 1)
        {
            mus.Play();
        }
        pinkScreen.gameObject.SetActive(false);
        greenScreen.gameObject.SetActive(false);
        blueScreen.gameObject.SetActive(false);
        ShowStartAnim();
    }

    private void ShowStartAnim()
    {
        ShowFromMenu();
        switch (StaticParams.PrevScene)
        {
            case 1:
                pinkScreen.gameObject.SetActive(true);
                pinkScreen.Hide();
                break;
            case 2:
                greenScreen.gameObject.SetActive(true);
                greenScreen.Hide();
                break;
            case 3:
                blueScreen.gameObject.SetActive(true);
                blueScreen.Hide();
                break;
        }
    }

    public void GoMenu()
    {
        Sequence showSettings = DOTween.Sequence();
        showSettings
            .PrependCallback(() =>
            {
                shopButton.GetComponent<Animator>().enabled = false;
            })
            .Append(shopButton.DOAnchorPosY(1000, 0.5f))
            .Join(shopText.DOAnchorPosY(1000, 0.5f))
            .Append(pinkLoli.DOAnchorPosY(-1000, 0.5f))
            .AppendCallback(() =>
            {
                if (PlayerPrefs.GetInt("GreenRoom") == 1)
                {
                    greenLoli.DOAnchorPosX(-1000, 0.5f).Play();
                }
                if (PlayerPrefs.GetInt("BlueRoom") == 1)
                {
                    blueLoli.DOAnchorPosX(1000, 0.5f).Play();
                }
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene("Menu");
            });
        showSettings.Restart();
    }

    private void ShowFromMenu()
    {
        Sequence fromMenu = DOTween.Sequence();
        fromMenu
            .Append(shopButton.DOAnchorPosY(-200, 0.5f))
            .Join(shopText.DOAnchorPosY(-350, 0.5f))
            .AppendCallback(() =>
            {
                shopButton.GetComponent<Animator>().enabled = true;
            })
            .Append(pinkLoli.DOAnchorPosY(155, 0.5f))
            .AppendCallback(() =>
            {
                if (PlayerPrefs.GetInt("GreenRoom") == 1)
                {
                    fromMenu.Join(greenLoli.DOAnchorPosX(-70, 0.5f));
                }
                if (PlayerPrefs.GetInt("BlueRoom") == 1)
                {
                    fromMenu.Join(blueLoli.DOAnchorPosX(70, 0.5f));
                }
            });
        fromMenu.Restart();
    }

    public void ShowMenu()
    {
        shopWindow.DOAnchorPosX(2000, 1).Play();
        ShowFromMenu();
    }

    public void UpdateBalance()
    {
        float balance = PlayerPrefs.GetFloat("PlayerBalance", StaticParams.StartMoney);
        balance = Mathf.Round(balance * 100.0f) / 100.0f;
        balanceText.text = balance.ToString();
        balanceText.text = balanceText.text.Replace(".", ",");
    }

    public void ShowShop()
    {
        UpdateBalance();
        Sequence showSettings = DOTween.Sequence();
        showSettings
            .PrependCallback(() =>
            {
                shopButton.GetComponent<Animator>().enabled = false;
            })
            .Append(shopButton.DOAnchorPosY(1000, 0.5f))
            .Join(shopText.DOAnchorPosY(1000, 0.5f))
            .Append(pinkLoli.DOAnchorPosY(-1000, 0.5f))
            .AppendCallback(() =>
            {
                if (PlayerPrefs.GetInt("GreenRoom") == 1)
                {
                    showSettings.Join(greenLoli.DOAnchorPosX(-1000, 0.5f));
                }
                if (PlayerPrefs.GetInt("BlueRoom") == 1)
                {
                    showSettings.Join(blueLoli.DOAnchorPosX(1000, 0.5f));
                }
            })
            .Append(shopWindow.DOAnchorPosX(0, 0.5f));
        showSettings.Restart();
    }

    public void StartGreenRoom()
    {
        Sequence loadRoom = DOTween.Sequence();
        loadRoom
            .PrependCallback(() =>
            {
                greenScreen.gameObject.SetActive(true);
                greenScreen.Show();
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene("GreenRoom");
            });
        loadRoom.Restart();
    }

    public void StartBlueRoom()
    {
        Sequence loadRoom = DOTween.Sequence();
        loadRoom
            .PrependCallback(() =>
            {
                blueScreen.gameObject.SetActive(true);
                blueScreen.Show();
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene("BlueRoom");
            });
        loadRoom.Restart();
    }

    public void StartPinkRoom()
    {
        Sequence loadRoom = DOTween.Sequence();
        loadRoom
            .PrependCallback(() =>
            {
                pinkScreen.gameObject.SetActive(true);
                pinkScreen.Show();
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                SceneManager.LoadScene("PinkRoom");
            });
        loadRoom.Restart();
    }
}
