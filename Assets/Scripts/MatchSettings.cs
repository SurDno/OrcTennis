using UnityEngine;

public static class MatchSettings {
	public enum GameMode {Classic, TimeAttack};
	public enum GameMap {Forest, Graveyard, Desert};
	
	// Settings
	private static GameMap map = GameMap.Forest;
	private static GameMode mode = GameMode.Classic;
	private static float timeInSeconds = 180;
	private static int goalsTillVictory = 15;
	private static bool periodicalDamage = true;
	
	// Methods for setting settings.
	
	public static void SetMap(GameMap newMap) {
		map = newMap;
	}

	public static void SetGameMode(GameMode newMode) {
		mode = newMode;
	}

	public static void SetMatchTime(float newTime) {
		timeInSeconds = newTime;
	}

	public static void SetGoalsTillVictory(int newGoals) {
		goalsTillVictory = newGoals;
	}
	

	public static void SetPeriodicalDamageValue(bool newValue) {
		periodicalDamage = newValue;
	}
	
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
	
	public static bool GetPeriodicalDamageEnabled() {
		return periodicalDamage;
	}
}
