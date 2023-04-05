using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: This is a clear example of god object anti-pattern. It HAS to be separated into at least two scripts. Though I am not yet sure how...
// Controls the general flow of the match. Handles restarting, scoring, winning conditions, etc.
public static class MatchController {
    public enum MatchState {Play, Goal, ExtraTime, Victory};
	
	// Info
	private static MatchState state = MatchState.Play;
    private static int leftTeamScore = 0;
    private static int rightTeamScore = 0;
	private static int totalPlayers;
	private static int livingPlayers;
	private static Coroutine countdownCoroutine;
	private static Coroutine restartCoroutine;
	
	public static void Start() {
		if(MatchSettings.GetGameMode() == MatchSettings.GameMode.TimeAttack)
			countdownCoroutine = CoroutinePlayer.StartCoroutine(Countdown());
		totalPlayers = PlayerHolder.GetPlayers().Length;
		livingPlayers = totalPlayers;
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
		
		// Activate goal cam.
		//CameraControl cam = Camera.main.GetComponent<CameraControl>();
		//Vector3 goalPos = ((Ball)Object.FindObjectOfType(typeof(Ball))).gameObject.transform.position;
		//goalPos.y = 8f;
		//float goalAngle = 70f;
		//cam.OverwritePosition(goalPos, goalAngle, 2);
		
		// Stop ball wherever it is.
        ((Ball)Object.FindObjectOfType(typeof(Ball))).SetVelocity(Vector2.zero);
		
		yield return new WaitForSeconds(3f);
		
		// If victory due to time was declared while we were waiting, don't restart.
		if(state == MatchState.Victory)
			yield break;
		
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
		// If we're in Sandbox mode, never declare victory.
		else if(MatchSettings.GetGameMode() == MatchSettings.GameMode.Sandbox) {
			Restart();
		}
	}
	
	public static void RegisterDeath(CharacterHealth deadPlayer) {
		// In Sandbox mode, dont track deaths and just respawn players.
		if(MatchSettings.GetGameMode() == MatchSettings.GameMode.Sandbox) {
			CoroutinePlayer.StartCoroutine(ResurrectAfterDelay(deadPlayer));
			return;
		}
		
		// In other modes, prevents softlock if all players are dead.
		livingPlayers--;
		
		// If there are no alive players, check ball horizontal velocity. If it's too low, restart. Else we can wait for the collision.
		if(livingPlayers == 0) {
			if(state == MatchState.Goal)
				return;
			
			float ballHorizontalVelocity = ((Ball)Object.FindObjectOfType(typeof(Ball))).GetVelocity().x;
			if(Mathf.Abs(ballHorizontalVelocity) < 1f)
				restartCoroutine = CoroutinePlayer.StartCoroutine(RestartAfterDelay());
		}
	}
	
	public static IEnumerator ResurrectAfterDelay(CharacterHealth deadPlayer) {
		yield return new WaitForSeconds(3.0f);
		
		deadPlayer.GetComponent<CharacterControls>().Respawn();
	}
	
	public static void UnregisterDeath() {
		livingPlayers++;
	}
	
	static IEnumerator RestartAfterDelay() {
		yield return new WaitForSeconds(2.0f);
		
		Restart();
	}

    private static void Restart() {
		// Deactivate goal cam.
		//CameraControl cam = Camera.main.GetComponent<CameraControl>();
		//cam.ContinueFollowing();
		CoroutinePlayer.StopCoroutine(restartCoroutine);
		
        // Get all players to get back to their spawn points, resurrect and heal them.
        foreach (Object player in Object.FindObjectsOfType(typeof(CharacterControls)))
            ((CharacterControls)player).Respawn();
		livingPlayers = totalPlayers;

        // Remove electric shield if it's present.
        GameObject.Find("BallWall")?.gameObject.SetActive(false);
		
        // Reset the ball.
        ((Ball)Object.FindObjectOfType(typeof(Ball))).ResetBall();
		
		if(state == MatchState.Goal)
			state = MatchState.Play;
    }
	
	private static IEnumerator Victory(Player.Team winningTeam) {
		CoroutinePlayer.StopCoroutine(restartCoroutine);
		Coroutine fireworks = CoroutinePlayer.StartCoroutine(CreateFireworks());
		state = MatchState.Victory;
		
		// Stop ball wherever it is.
        ((Ball)Object.FindObjectOfType(typeof(Ball))).SetVelocity(Vector2.zero);
		
		// Activate victory cam.
		CameraControl cam = Camera.main.GetComponent<CameraControl>();
		Vector3 victoryPos = new Vector3(0, 6, -14.5f);
		float victoryAngle = 20f;
		cam.OverwritePosition(victoryPos, victoryAngle, 1);
		
		// Hide all healthbars and abilities.
        foreach (CharacterUI player in (CharacterUI[])Object.FindObjectsOfType(typeof(CharacterUI))) {
			player.DisablePlayerSpecificUI();
		}
		
		// Play victory or defeat animations for each player depending on if they're on winning team.
		// Create confetti on top of winning players.
        foreach (CharacterAnimation player in (CharacterAnimation[])Object.FindObjectsOfType(typeof(CharacterAnimation))) {
			Player.Team characterTeam = player.GetComponent<CharacterOwner>().GetOwner().GetTeam();
			if(characterTeam == winningTeam) {
				player.StartVictoryAnimations();
				
				// Create confetti.
				GameObject prefab = Resources.Load<GameObject>("Prefabs/Magic/VictoryConfetti");
				Vector3 effectPosition = player.gameObject.transform.position;
				effectPosition.y += 12f;
				Object.Instantiate(prefab, effectPosition, Quaternion.identity);
			} else
				player.StartDefeatAnimations();
		}
		
		// Start victory theme and wait for it to end.
		SoundManager.PlayMusic("Victory", 0.5f, false);
		yield return new WaitForSeconds(SoundManager.GetClipLength("Victory"));
		
		CoroutinePlayer.StopCoroutine(fireworks);
		CoroutinePlayer.StartCoroutine(GameController.ReturnToMenu());
	}
	
	private static IEnumerator CreateFireworks() {
		// Preload the effect.
		GameObject prefab = Resources.Load<GameObject>("Prefabs/Magic/VictoryFireworks");
		
		while(true) {
			Vector3 effectPosition = new Vector3(Random.Range(-50f, 50f), -5f, Random.Range(-10, 50));
			Object.Instantiate(prefab, effectPosition, Quaternion.identity);
			yield return new WaitForSeconds(0.1f);
		}
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
