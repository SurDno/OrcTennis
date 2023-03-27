using UnityEngine;
using UnityEngine.SceneManagement;

// Hold core information about the game independent of all gameobjects and scenes.
public static class GameController {
    public enum GameState {Setup, Match};
	public enum GameMode {Classic, TimeAttack};
	public enum GameMap {Forest, Graveyard};
	
	// Info
	private static GameState currentState = GameState.Setup;
	
	// Settings
	private static GameMap map = GameMap.Graveyard;
	private static GameMode mode = GameMode.Classic;
	private static float timeInSeconds = 300;
	private static int goalsTillVictory;
	
	public static void LoadMatch() {
		// Load specific level environment first to apply lighting correctly.
		currentState = GameState.Match;
		SceneManager.LoadScene(map.ToString(),  LoadSceneMode.Single);
		
		// Load core scene for gameplay additively.
		SceneManager.LoadScene("GameCore",  LoadSceneMode.Additive);
		
		// Start music.
		SoundManager.PlayMusic("Match", 0.3f);
	}
	
	// Resets all game info and goes back to game menu.
	public static void ReturnToMenu() {
		foreach (Player player in PlayerHolder.GetPlayers()) {
            player.GetCursor().ShowCursor();
            player.ResetPlayerData();
		}

        ScoreManager.ResetScoreData();
        PlayerSetup.ResetSetupData();

		currentState = GameState.Setup;
        SceneManager.LoadScene("Setup");
		
		// Stop music.
		SoundManager.StopMusic();
	}
	
	private static GameState GetGameState() {
		return currentState;
	}
}
