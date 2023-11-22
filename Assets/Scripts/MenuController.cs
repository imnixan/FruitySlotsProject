using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private RectTransform appname,
        hud,
        settingsWindow;

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

    [SerializeField]
    private LoadingScreen ls;

    private bool showsettings;

    private void Start()
    {
        soundImage.sprite = statusSprite[PlayerPrefs.GetInt("Sound", 1)];
        vibroImage.sprite = statusSprite[PlayerPrefs.GetInt("Vibro", 1)];
        musicImage.sprite = statusSprite[PlayerPrefs.GetInt("Music", 1)];
        appname.DOScale(Vector2.zero, 0.5f).From().Play();
        hud.DOAnchorPosY(-2000, 1.5f).From().Play();

        if (StaticParams.FromGame)
        {
            StaticParams.FromGame = false;
            ls.Hide();
        }
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
            .PrependCallback(() =>
            {
                ls.Show();
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Slot");
            });
        changeScene.Restart();
    }
}
