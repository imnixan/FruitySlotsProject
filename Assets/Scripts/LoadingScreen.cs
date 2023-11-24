using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Image screen,
        candy,
        text;

    private Color transparent = new Color(0, 0, 0, 0);

    public void Hide()
    {
        screen.DOColor(transparent, 0.5f).Play();
        candy.DOColor(transparent, 0.5f).Play();
        text.DOColor(transparent, 0.5f).Play();
    }

    public void Show()
    {
        screen.DOColor(Color.white, 0.5f).Play();
        candy.DOColor(Color.white, 0.5f).Play();
        text.DOColor(Color.white, 0.5f).Play();
    }
}
