using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour
{

    [Header("Prefabs and Cached Objects")]
    [SerializeField] private Text leftTeamScoreText;
    [SerializeField] private Text rightTeamScoreText;
    [SerializeField] private Text timerText;

    [Header("Game Settings")]
    [SerializeField] private int goalsTillVictory = 30;
    [SerializeField] private int matchDurationSeconds = 300;

    [Header("Current Values")]
    private static int leftTeamScore = 0;
    private static int rightTeamScore = 0;
    private float remainingTime;

    private Coroutine timerCoroutine;

    void Start()
    {
        remainingTime = matchDurationSeconds;
        UpdateTimerText();
        StartTimer();
    }

    void Update()
    {
        leftTeamScoreText.text = leftTeamScore.ToString();
        rightTeamScoreText.text = rightTeamScore.ToString();

        if (leftTeamScore >= goalsTillVictory || rightTeamScore >= goalsTillVictory || remainingTime <= 0f)
        {
            EndMatch();
        }

        // Change the color of the timer text to red if remaining time is less than 1 minute
        if (remainingTime < 60f)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
    }

    public static void GoalLeft()
    {
        leftTeamScore++;
        MatchController.Restart();
    }

    public static void GoalRight()
    {
        rightTeamScore++;
        MatchController.Restart();
    }

    public static void ResetScoreData()
    {
        leftTeamScore = 0;
        rightTeamScore = 0;
    }

    public static bool GetIsRedWinning()
    {
        return rightTeamScore > leftTeamScore;
    }

    public static bool GetIsGreenWinning()
    {
        return leftTeamScore > rightTeamScore;
    }

    private void StartTimer()
    {
        timerCoroutine = StartCoroutine(Timer());
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    private IEnumerator Timer()
    {
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }
    }

    private void EndMatch()
    {
        StopCoroutine(timerCoroutine);
        GameController.ReturnToMenu();
    }
}
