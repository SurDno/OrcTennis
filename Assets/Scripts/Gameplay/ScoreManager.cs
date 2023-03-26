using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class ScoreManager : MonoBehaviour {
    [Header("Prefabs and Cached Objects")]
    [SerializeField] private Text leftTeamScoreText;
    [SerializeField] private Text rightTeamScoreText;
    [Header("Prefabs and Cached Objects")]
    private int goalsTillVictory = 15;

    [Header("Current Values")]
    private static int leftTeamScore = 0;
    private static int rightTeamScore = 0;

    private Coroutine timerCoroutine;

    void Update() {
        leftTeamScoreText.text = leftTeamScore.ToString();
        rightTeamScoreText.text = rightTeamScore.ToString();

        if (leftTeamScore >= goalsTillVictory || rightTeamScore >= goalsTillVictory) {
			// Victory cutscene code should go here.
			
			GameController.ReturnToMenu();
        }
    }

    public static void GoalLeft() {
        leftTeamScore++;
        MatchController.Restart();
    }

    public static void GoalRight() {
        rightTeamScore++;
        MatchController.Restart();
    }

    public static void ResetScoreData() {
        leftTeamScore = 0;
        rightTeamScore = 0;
    }
	
	public static bool GetIsRedWinning() {
		return rightTeamScore > leftTeamScore;
	}
	
	public static bool GetIsGreenWinning() {
		return leftTeamScore > rightTeamScore;
	}
}