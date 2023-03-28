using UnityEngine;

public static class MatchSettings {
	public enum GameMode {Classic, TimeAttack};
	public enum GameMap {Forest, Graveyard};
	
	// Settings
	private static GameMap map = GameMap.Graveyard;
	private static GameMode mode = GameMode.TimeAttack;
	private static float timeInSeconds = 180;
	private static int goalsTillVictory = 10;
	
	// Methods for getting settings.
	
	public static GameMap GetMap() {
		return map;
	}
	
	public static GameMode GetGameMode() {
		return mode;
	}
	
	public static float GetMatchTime() {
		return timeInSeconds;
	}
	
	public static int GetGoalsTillVictory() {
		return goalsTillVictory;
	}
}
