using UnityEngine;

public class GameController : MonoBehaviour {
    public enum GameState {Setup, Match};
	
	public static GameState currentState = GameState.Setup;

	public static void SetGameState(GameState newState) {
		currentState = newState;
	}
}
