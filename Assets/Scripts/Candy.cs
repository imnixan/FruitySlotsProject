using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Candy : MonoBehaviour
{
    public RectTransform rt;
    public CandyParams candyParam;
    private Image image;
    private ParticleSystem[] ps;
    private Animator animator;
    private TextMeshProUGUI prizeText;

    public void Init(CandyParams candyParam)
    {
        this.candyParam = candyParam;
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.sprite = candyParam.candySprite;
        ps = GetComponentsInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
        prizeText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Blur()
    {
        image.sprite = candyParam.blurSprite;
    }

    public void UnBlur()
    {
        image.sprite = candyParam.candySprite;
    }

    public void SetConnected(float prize)
    {
        foreach (var particle in ps)
        {
            particle.Play();
        }
        prizeText.text = prize.ToString().Replace(".", ",");
        animator.SetTrigger("ShowWin");
    }
}
