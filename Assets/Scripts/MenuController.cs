using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private RectTransform appname,
        hud,
        settingsWindow;

    [SerializeField]
    private TextMeshProUGUI balanceText;

    private float balance;

    [SerializeField]
    private ParticleSystem[] soundPs,
        vibroPs,
        musicPs;

    [SerializeField]
    private Image soundImage,
        vibroImage,
        musicImage;

    [SerializeField]
    private Sprite[] statusSprite;

    private bool showsettings;
    private AudioSource musicPlayer;

    [SerializeField]
    private ParticleSystem ps;

    public void UpdateBalance(bool salut = false)
    {
        balance = PlayerPrefs.GetFloat("PlayerBalance", StaticParams.StartMoney);
        balance = Mathf.Round(balance * 100.0f) / 100.0f;

        Sequence updateBalance = DOTween.Sequence();
        updateBalance
            .Append(balanceText.rectTransform.DOScale(1.2f, 0.3f))
            .AppendCallback(() =>
            {
                balanceText.text = balance.ToString();
                balanceText.text = balanceText.text.Replace(".", ",");
                if (salut)
                {
                    ps.Play();
                }
            })
            .Append(balanceText.rectTransform.DOScale(1f, 0.3f));
        updateBalance.Restart();
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Application.targetFrameRate = 400;
        musicPlayer = GetComponent<AudioSource>();

        soundImage.sprite = statusSprite[PlayerPrefs.GetInt("Sound", 1)];
        vibroImage.sprite = statusSprite[PlayerPrefs.GetInt("Vibro", 1)];
        musicImage.sprite = statusSprite[PlayerPrefs.GetInt("Music", 1)];
        appname.DOScale(Vector2.zero, 0.5f).From().Play();
        hud.DOAnchorPosY(-2000, 1.5f).From().Play();
        if (PlayerPrefs.GetInt("Music", 1) == 1)
        {
            musicPlayer.Play();
        }
        UpdateBalance();
    }

    public void ChangeSound()
    {
        int currentSound = PlayerPrefs.GetInt("Sound", 1);
        currentSound = currentSound == 1 ? 0 : 1;
        soundPs[currentSound].Play();
        PlayerPrefs.SetInt("Sound", currentSound);
        PlayerPrefs.Save();
        soundImage.sprite = statusSprite[PlayerPrefs.GetInt("Sound", 1)];
    }

    public void ChangeMusic()
    {
        int currentMusic = PlayerPrefs.GetInt("Music", 1);
        currentMusic = currentMusic == 1 ? 0 : 1;
        if (currentMusic == 1)
        {
            musicPlayer.Play();
        }
        else
        {
            musicPlayer.Pause();
        }
        musicPs[currentMusic].Play();
        PlayerPrefs.SetInt("Music", currentMusic);
        PlayerPrefs.Save();
        musicImage.sprite = statusSprite[PlayerPrefs.GetInt("Music", 1)];
    }

    public void ChangeVibro()
    {
        int currentVibro = PlayerPrefs.GetInt("Vibro", 1);
        currentVibro = currentVibro == 1 ? 0 : 1;
        vibroPs[currentVibro].Play();
        PlayerPrefs.SetInt("Vibro", currentVibro);
        PlayerPrefs.Save();
        vibroImage.sprite = statusSprite[PlayerPrefs.GetInt("Vibro", 1)];
    }

    public void ShowSettings()
    {
        if (!showsettings)
        {
            showsettings = true;
            settingsWindow.DOAnchorPosX(0, 0.5f).Play();
        }
        else
        {
            HideSettings();
        }
    }

    public void HideSettings()
    {
        if (showsettings)
        {
            showsettings = false;
            settingsWindow.DOAnchorPosX(2000, 0.5f).Play();
        }
    }

    public void Play()
    {
        Sequence changeScene = DOTween.Sequence();
        changeScene
            .Append(hud.DOAnchorPosY(-2000, 0.5f))
            .Join(appname.DOAnchorPosY(3000, 0.5f))
            .AppendCallback(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GuestRoom");
            });
        changeScene.Restart();
    }
}
