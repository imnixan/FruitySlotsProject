using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CandyInfo : MonoBehaviour
{
    [SerializeField]
    private CandyParams param;
    private Image image;
    private TextMeshProUGUI prize;
    private float basePrize,
        finalPrize;

    private void Start()
    {
        image = GetComponent<Image>();
        prize = GetComponentInChildren<TextMeshProUGUI>();
        image.sprite = param.candySprite;
        basePrize = 1 - param.chanse;
    }

    public void SetPrize(float bet)
    {
        finalPrize = basePrize * bet;
        finalPrize = Mathf.Round(finalPrize * 100.0f) / 100.0f;
        prize.text = finalPrize.ToString() + "$";
    }
}
