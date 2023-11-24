using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine;

public class InfoWindow : MonoBehaviour
{
    private RectTransform rt;

    [SerializeField]
    private TextMeshProUGUI summ;

    [SerializeField]
    private Image icon;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        rt.localScale = Vector2.zero;
    }

    public void CloseInfo()
    {
        rt.DOScale(0, 0.5f).Play();
    }

    public void ShowInfo(string text, Sprite iconsprite)
    {
        summ.text = text;
        icon.sprite = iconsprite;
        rt.DOScale(1, 0.5f).Play();
    }
}
