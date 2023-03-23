using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text timerText;
    public Text greenTeamScoreText;
    public Text redTeamScoreText;

    public int greenTeamScore;
    public int redTeamScore;

    private float startTime;
    private float timeRemaining;
    private bool isTimerRunning;

    void Start()
    {
        startTime = Time.time;
        timeRemaining = 300f; // 5 minutes in seconds
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeRemaining = startTime + 300f - Time.time;

            if (timeRemaining <= 0f)
            {
                isTimerRunning = false;
                timeRemaining = 0f;
                // Game over
            }

            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void IncreaseGreenTeamScore(int amount)
    {
        greenTeamScore += amount;
        greenTeamScoreText.text = "Green Team: " + greenTeamScore;
    }

    public void IncreaseRedTeamScore(int amount)
    {
        redTeamScore += amount;
        redTeamScoreText.text = "Red Team: " + redTeamScore;
    }
}