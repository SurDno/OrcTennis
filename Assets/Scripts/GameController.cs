using UnityEngine;

// Hold core information about the game independent of all gameobjects and scenes.

public static class GameController {
    public enum GameState {Setup, Match};
	
	private static GameState currentState = GameState.Setup;
	
	public static void SetGameState(GameState newState) {
		currentState = newState;
	}
	
	private static GameState GetGameState() {
		return currentState;
	}
}
