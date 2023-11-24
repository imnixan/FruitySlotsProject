using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class BuyLine : MonoBehaviour
{
    [SerializeField]
    private InfoWindow infoWindow;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private string itemInfo;

    private Image itemImage;

    [SerializeField]
    private float price;

    [SerializeField]
    private bool canStack;

    [SerializeField]
    private AudioClip buySound;

    [SerializeField]
    private Button buyBtn,
        info;
    private TextMeshProUGUI priceText;

    [SerializeField]
    private Sprite cart,
        accept;
    private ParticleSystem flash;

    private GuestRoom gr;

    private void Start()
    {
        itemImage = GetComponent<Image>();
        flash = GetComponentInChildren<ParticleSystem>();
        gr = FindAnyObjectByType<GuestRoom>();
        priceText = GetComponentInChildren<TextMeshProUGUI>();
        buyBtn.onClick.AddListener(Buy);
        info.onClick.AddListener(ShowInfo);
        if (canStack)
        {
            price += PlayerPrefs.GetInt(itemName) * price;
        }

        UpdateLine();
    }

    private void ShowInfo()
    {
        infoWindow.ShowInfo(itemInfo, itemImage.sprite);
    }

    private void Buy()
    {
        infoWindow.CloseInfo();
        float playerBalance = PlayerPrefs.GetFloat("PlayerBalance", StaticParams.StartMoney);
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            AudioSource.PlayClipAtPoint(buySound, Vector2.zero);
        }
        playerBalance -= price;
        PlayerPrefs.SetFloat("PlayerBalance", playerBalance);
        PlayerPrefs.Save();
        gr.UpdateBalance();
        PlayerPrefs.SetInt(itemName, PlayerPrefs.GetInt(itemName) + 1);
        PlayerPrefs.Save();
        if (canStack)
        {
            price += PlayerPrefs.GetInt(itemName) * price;
        }
        UpdateLine();
    }

    private void UpdateLine()
    {
        float playerBalance = PlayerPrefs.GetFloat("PlayerBalance", StaticParams.StartMoney);
        priceText.text = price.ToString();
        if ((canStack || PlayerPrefs.GetInt(itemName) == 0) && playerBalance >= price)
        {
            priceText.color = Color.green;
            buyBtn.interactable = true;
        }
        else
        {
            priceText.color = Color.gray;
            buyBtn.interactable = false;
            if (!canStack && PlayerPrefs.GetInt(itemName) > 0)
            {
                flash.Play();
                buyBtn.image.sprite = accept;
                buyBtn.image.color = Color.white;
                buyBtn.image.SetNativeSize();
                buyBtn.image.rectTransform.sizeDelta *= 2;
            }
        }
    }

    private void Update()
    {
        float playerBalance = PlayerPrefs.GetFloat("PlayerBalance", StaticParams.StartMoney);
        bool canbuy = ((canStack || PlayerPrefs.GetInt(itemName) == 0) && playerBalance >= price);
        priceText.color = canbuy ? Color.green : Color.gray;
        buyBtn.interactable = canbuy;
    }
}
