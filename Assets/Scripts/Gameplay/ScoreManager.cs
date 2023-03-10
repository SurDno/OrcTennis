using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Text leftTeamScoreText;
	[SerializeField]private Text rightTeamScoreText;
    
	[Header("Current Values")]
	private static int leftTeamScore = 0;
	private static int rightTeamScore = 0;
	
    void Update() {
		leftTeamScoreText.text = leftTeamScore.ToString();
		rightTeamScoreText.text = rightTeamScore.ToString();
		
		if(leftTeamScore > 2 || rightTeamScore > 2) {
			foreach(Player player in PlayerHolder.GetPlayers()) {
				player.GetCursor().ShowCursor();
				player.ResetPlayerData();
				
			}
				
			ResetScoreData();
			PlayerSetup.ResetSetupData();
			
			SceneManager.LoadScene("Setup");
		}
	}
	
	public static void GoalLeft() {
		leftTeamScore++;
	}
	
	public static void GoalRight() {
		rightTeamScore++;
	}
	
	public static void ResetScoreData() {
		leftTeamScore = 0;
		rightTeamScore = 0;
	}
}
