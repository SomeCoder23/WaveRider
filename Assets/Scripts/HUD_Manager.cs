using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Manager : MonoBehaviour
{
    #region Singleton

    public static HUD_Manager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.LogWarning("MORE THAN ONE HUD MANAGER!!");
        else instance = this;
    }

    #endregion

    int coins = 0;
    int time = 0;
    int gems = 0;
    int highScore = 0;
    public Text coinsTxt, timerTxt, gemsTxt, highScoreTxt, pointsTxt;
    public GameObject msgTxt;
    public Slider shieldTimer;
    bool playing = false;

    private void Start()
    {
        shieldTimer.gameObject.SetActive(false);
        timerTxt.text = "00 : 00";
        InitializeHighScore();
    }

    void InitializeHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
            highScoreTxt.text = "High Score " + highScore.ToString();
        }
    }

    void SaveHighScore(int score)
    {
        highScore = score;
        PlayerPrefs.SetInt("HighScore", highScore);
        highScoreTxt.text = "High Score " + highScore.ToString();
    }

    public void StartTimer()
    {
        MapManager.instance.InitializeMap();
        playing = true;
        StartCoroutine(Timer());
    }

    public void AddCoins()
    {
        coins++;
        coinsTxt.text = coins.ToString();
    }

    public void AddGem()
    {
        gems++;
        gemsTxt.text = gems.ToString();
    }

    public void PauseTimer(bool pause)
    {
        if (pause)
            StopAllCoroutines();
        else StartTimer();
    }


    IEnumerator Timer()
    {
        while (playing)
        {
            yield return new WaitForSeconds(1);
            time++;
            timerTxt.text = FormatTime();
        }
    }

    string FormatTime()
    {
        int minutes = time / 60;
        int seconds = time - (minutes * 60);
        string result = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        result += " : ";
        result += seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();
        return result;
    }

    public void Restart()
    {
        SoundManager.instance.PlayMusic();
        coins = 0;
        gems = 0;
        time = 0;
        coinsTxt.text = "0";
        gemsTxt.text = "0";
        timerTxt.text = "00 : 00";
        msgTxt.SetActive(false);

    }

    public void Replay()
    {
        Restart();
        Boat.instance.StartBoat();
        MapManager.instance.ResetMap();
        StartTimer();

    }

    public void ResetHighScore()
    {
        SaveHighScore(0);
    }

    public void ActivateShieldTime(float time)
    {
        shieldTimer.gameObject.SetActive(true);
        shieldTimer.maxValue = time;
        shieldTimer.value = time;
    }

    public void DeactivateShieldTimer()
    {
        shieldTimer.gameObject.SetActive(false);
    }

    //IEnumerator ShieldCountdown()
    //{
    //    while (shieldTimer.value > 0)
    //    {
    //        yield return new WaitForSeconds(0.1f);
    //        shieldTimer.value -= 0.1f;               
    //    }
    //    shieldTimer.gameObject.SetActive(false);
    //}

    public void UpdateShieldCountdown(float value)
    {
        shieldTimer.value -= value;

        //if (shieldTimer.value <= 0)
        //    shieldTimer.gameObject.SetActive(false);
    }

    public void LoseGame()
    {
        int score = (gems * 5) + coins + time;
        if (score > highScore)
        {
            SaveHighScore(score);
            msgTxt.SetActive(true);
        }

        playing = false;
        pointsTxt.text = score.ToString();
        Boat.instance.StopBoat();
        UIManager ui = GetComponent<UIManager>();
        SoundManager.instance.Lose();
        if (ui != null)
            ui.DisplayLoseWindow();
    }



}
