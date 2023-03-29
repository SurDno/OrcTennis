using UnityEngine;
using UnityEngine.SceneManagement;

// Hold core methods for switching between scenes and accesing information about current game state.
public static class GameController {
    public enum GameState {Setup, Match};
	
	// Info
	private static GameState currentState = GameState.Setup;
	
	public static void LoadMatch() {
		// Load specific level environment first to apply lighting correctly.
		currentState = GameState.Match;
		SceneManager.LoadScene(MatchSettings.GetMap().ToString(),  LoadSceneMode.Single);
		
		// Load core scene for gameplay additively.
		SceneManager.LoadScene("GameCore",  LoadSceneMode.Additive);
		MatchController.Start();
		
		// Start music.
		SoundManager.PlayMusic("Match", 0.3f, true);
	}
	
	// Resets all game info and goes back to game menu.
	public static void ReturnToMenu() {
		foreach (Player player in PlayerHolder.GetPlayers()) {
            player.GetCursor().ShowCursor();
            player.ResetPlayerData();
		}

        MatchController.ResetScoreData();
        PlayerSetup.ResetSetupData();

		currentState = GameState.Setup;
        SceneManager.LoadScene("Setup");
		Debug.Log("Test");
		// Stop music.
		SoundManager.StopMusic();
	}
	
	public static GameState GetGameState() {
		return currentState;
	}
}
