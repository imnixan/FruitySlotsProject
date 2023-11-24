using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Clock : MonoBehaviour
{
    [SerializeField]
    private Button clockBtn;
    private ParticleSystem shine;

    [SerializeField]
    private AudioClip coinGet;

    [SerializeField]
    private TextMeshProUGUI clockText;

    public double currentTime;
    private double timeDelay = 3600;

    public double savedTime;

    private void Start()
    {
        shine = GetComponentInChildren<ParticleSystem>();
        currentTime = DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds;
        savedTime = PlayerPrefs.GetFloat("SavedTime");
        clockBtn.interactable = false;
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("Clock"))
        {
            currentTime = DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds;
            if (currentTime >= savedTime && !clockBtn.interactable)
            {
                clockBtn.interactable = true;
                shine.Play();
                clockText.text = "Get 750 x" + PlayerPrefs.GetInt("Clock");
            }
            if (!clockBtn.interactable)
            {
                float timeRemain = (float)(savedTime - currentTime);
                int minutes = Mathf.FloorToInt(timeRemain / 60);
                int seconds = Mathf.FloorToInt(timeRemain % 60);
                clockText.text = string.Format("{0:d2}:{1:d2}", minutes, seconds);
            }
        }
    }

    public void GetClockMoney()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 1)
        {
            AudioSource.PlayClipAtPoint(coinGet, Vector2.zero);
        }
        float balance = PlayerPrefs.GetFloat("PlayerBalance");
        float newBalance = balance + (750 * PlayerPrefs.GetInt("Clock"));
        PlayerPrefs.SetFloat("PlayerBalance", newBalance);
        PlayerPrefs.Save();
        currentTime = DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds;
        savedTime = currentTime + timeDelay;
        PlayerPrefs.SetFloat("SavedTime", (float)(savedTime));
        PlayerPrefs.Save();
        clockBtn.interactable = false;
        shine.Stop();
        FindAnyObjectByType<MenuController>().UpdateBalance(true);
    }
}
