using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Controls the general flow of the match. Handles restarting, scoring, winning conditions, etc.
public static class MatchController {
    public enum MatchState {Play, Goal, ExtraTime, Victory};
	
	// Info
	private static MatchState state = MatchState.Play;
    private static int leftTeamScore = 0;
    private static int rightTeamScore = 0;
	private static Coroutine countdownCoroutine;
	
	public static void Start() {
		if(MatchSettings.GetGameMode() == MatchSettings.GameMode.TimeAttack)
			countdownCoroutine = CoroutinePlayer.StartCoroutine(Countdown());
	}
	
	private static IEnumerator Countdown() {
		yield return new WaitForSeconds(MatchSettings.GetMatchTime());
		
		// If score is even when timer is up, declare extra time. If not, declare a victory for either team.
		if(leftTeamScore == rightTeamScore)
			state = MatchState.ExtraTime;
		else
			CoroutinePlayer.StartCoroutine(Victory(rightTeamScore > leftTeamScore ? Player.Team.Red : Player.Team.Green));
	}
	
    public static void GoalLeft() {
        rightTeamScore++;
		
		CoroutinePlayer.StartCoroutine(GoalFreeze());
    }	
	
    public static void GoalRight() {
        leftTeamScore++;
		
		CoroutinePlayer.StartCoroutine(GoalFreeze());
    }
	
	private static IEnumerator GoalFreeze() {
		bool goalDuringExtraTime = state == MatchState.ExtraTime;
		state = MatchState.Goal;
		
		// Play whistle sound.
		SoundManager.PlaySound("Goal");
		
		// Stop ball wherever it is.
        ((Ball)Object.FindObjectOfType(typeof(Ball))).SetVelocity(Vector2.zero);
		
		yield return new WaitForSeconds(2f);
		
		// If we're in Classic mode, declare victory whenever either team reaches needed amount of goals.
		if(MatchSettings.GetGameMode() == MatchSettings.GameMode.Classic) {
			if(leftTeamScore >= MatchSettings.GetGoalsTillVictory())
				CoroutinePlayer.StartCoroutine(Victory(Player.Team.Green));
			else if(rightTeamScore >= MatchSettings.GetGoalsTillVictory())
				CoroutinePlayer.StartCoroutine(Victory(Player.Team.Red));
			else 
				Restart();
		}
		// If we're in Time Attack mode, declary victory only if it's extra time.
		else if(MatchSettings.GetGameMode() == MatchSettings.GameMode.TimeAttack) {
			if(!goalDuringExtraTime)
				Restart();
			else if(goalDuringExtraTime) 
				CoroutinePlayer.StartCoroutine(Victory(rightTeamScore > leftTeamScore ? Player.Team.Red : Player.Team.Green));
		}
	}

    private static void Restart() {
        // Get all players to get back to their spawn points.
        foreach (Object player in Object.FindObjectsOfType(typeof(CharacterControls)))
            ((CharacterControls)player).Respawn();

        // Remove electric shield if it's present.
        GameObject.Find("BallWall")?.gameObject.SetActive(false);
		
        // Reset the ball.
        ((Ball)Object.FindObjectOfType(typeof(Ball))).ResetBall();
		
		state = MatchState.Play;
    }
	
	private static IEnumerator Victory(Player.Team team) {
		
		//SoundManager.StopMusic();
		SoundManager.PlayMusic("Victory", 0.5f, false);
		
		state = MatchState.Victory;
		
		yield return new WaitForSeconds(SoundManager.GetClipLength("Victory"));
		
		GameController.ReturnToMenu();
	}
	
    public static void ResetScoreData() {
		state = MatchState.Play;
		CoroutinePlayer.StopCoroutine(countdownCoroutine);
		
        leftTeamScore = 0;
        rightTeamScore = 0;
    }
	
	
	// Methods for getting match data.
	
	public static MatchState GetMatchState() {
		return state;
	}
	
	public static int GetLeftTeamScore() {
		return leftTeamScore;
	}
	
	public static int GetRightTeamScore() {
		return rightTeamScore;
	}
}
