using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text moneyText;
    public AudioSource gameplayMusic;

    public float gameDuration = 60f;
    public int targetMoney = 10;

    private float remainingTime;
    private int currentMoney = 0;
    private bool gameEnded = false;

    void Start()
    {
        remainingTime = gameDuration;
        UpdateMoneyText();
        UpdateTimerText();

        if (gameplayMusic != null)
        {
            gameplayMusic.pitch = 1f;
        }
    }

    void Update()
    {
        if (gameEnded)
        {
            return;
        }

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndGame();
        }

        UpdateTimerText();
        UpdateMusicSpeed();
    }

    void UpdateTimerText()
    {
        int seconds = Mathf.CeilToInt(remainingTime);
        timerText.text = "TIME: " + seconds;
    }

    void UpdateMoneyText()
    {
        moneyText.text = "MONEY: $" + currentMoney;
    }
    void UpdateMusicSpeed()
    {
        if (gameplayMusic == null)
        {
            return;
        }

        if (remainingTime <= 15f && remainingTime > 0f)
        {
            float progress = 1f - (remainingTime / 15f);

            gameplayMusic.pitch = Mathf.Lerp(1f, 1.5f, progress);
        }
        else
        {
            gameplayMusic.pitch = 1f;
        }
    }

    public void AddMoney(int amount)
    {
        if (gameEnded)
        {
            return;
        }

        currentMoney += amount;
        UpdateMoneyText();

        Debug.Log("Money increased! Current money: $" + currentMoney);
    }

    void EndGame()
    {
        gameEnded = true;

        if (currentMoney >= targetMoney)
        {
            SceneManager.LoadScene("WinScene");
        }
        else
        {
            SceneManager.LoadScene("LoseScene");
        }
    }
}