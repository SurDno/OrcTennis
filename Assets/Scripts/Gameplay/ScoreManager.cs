using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    [Header("Prefabs and Cached Objects")]
    [SerializeField] private Text leftTeamScoreText;
    [SerializeField] private Text rightTeamScoreText;
    [SerializeField] private Image leftTeamWinning;
    [SerializeField] private Image rightTeamWinning;
    [SerializeField] private Text timerText;

    [Header("Current Values")]
    private float remainingTime;


    void Start() {
		// See if we need to work with timer depending on the game mode.
		if(MatchSettings.GetGameMode() == MatchSettings.GameMode.TimeAttack)
			remainingTime = MatchSettings.GetMatchTime();
		else
			timerText.gameObject.SetActive(false);
		
		// See if we need to display score depending on the game mode.
		if(MatchSettings.GetGameMode() == MatchSettings.GameMode.Sandbox)
			leftTeamScoreText.gameObject.transform.parent.gameObject.SetActive(false);
    }

    void Update() {
        leftTeamScoreText.text = MatchController.GetLeftTeamScore().ToString();
        rightTeamScoreText.text = MatchController.GetRightTeamScore().ToString();
		
		leftTeamWinning.enabled = MatchController.GetLeftTeamScore() > MatchController.GetRightTeamScore();
		rightTeamWinning.enabled = MatchController.GetRightTeamScore() > MatchController.GetLeftTeamScore();

        // Change the color of the timer text to red if it's extra time.
        timerText.color = (remainingTime < 0f) ? Color.red : Color.white;
		
		// Don't do anything else if we are not in Time Attack mode.
		if(MatchSettings.GetGameMode() != MatchSettings.GameMode.TimeAttack)
			return;
		
		// Else, update timer text.
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.Max(0, Mathf.FloorToInt(remainingTime / 60f));
        int seconds = Mathf.Max(0, Mathf.FloorToInt(remainingTime % 60f));
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
