using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [Header("Prefabs and Cached Objects")]
    [SerializeField] private Text leftTeamScoreText;
    [SerializeField] private Text rightTeamScoreText;
    [Header("Prefabs and Cached Objects")]
    private int goalsTillVictory = 15;

    [Header("Current Values")]
    private static int leftTeamScore = 0;
    private static int rightTeamScore = 0;

    private Coroutine timerCoroutine;

    void Start()
    {
        // the timer coroutine.
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    void Update()
    {
        leftTeamScoreText.text = leftTeamScore.ToString();
        rightTeamScoreText.text = rightTeamScore.ToString();

        if (leftTeamScore >= goalsTillVictory || rightTeamScore >= goalsTillVictory)
        {
            foreach (Player player in PlayerHolder.GetPlayers())
            {
                player.GetCursor().ShowCursor();
                player.ResetPlayerData();

            }

            ResetScoreData();
            PlayerSetup.ResetSetupData();

            SceneManager.LoadScene("Setup");
        }
    }

    public static void GoalLeft()
    {
        leftTeamScore++;
        Restart();
    }

    public static void GoalRight()
    {
        rightTeamScore++;
        Restart();
    }

    public static void Restart()
    {
        // Get all players to get back to their spawn points.
        foreach (Object player in FindObjectsOfType(typeof(CharacterControls)))
            ((CharacterControls)player).Respawn();

        // Reset the ball.
        ((Ball)FindObjectOfType(typeof(Ball))).ResetBall();

        // Remove electric shield if it's present.
        GameObject.Find("BallWall")?.gameObject.SetActive(true);

    }

    public static void ResetScoreData()
    {
        leftTeamScore = 0;
        rightTeamScore = 0;
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(300); // Wait for 5 minutes.

        SceneManager.LoadScene("Graveyard"); // Load the "Graveyard" scene.
    }
}