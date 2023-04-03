using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UISetup))]
public class PlayerSetup : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	private Image[] teamColorBlocks;
	private Image leaveTeamButton;
	private Image readyButton;
	private Image previousMapButton;
	private Image nextMapButton;
	private Image previousGameModeButton;
	private Image nextGameModeButton;
	private Image lessTimeButton;
	private Image moreTimeButton;
	private Image lessGoalsButton;
	private Image moreGoalsButton;
	private Image periodicalDamageSwitch;
	
	[Header("Current Settings")]
	private static Player[] playerByTeamBlockIndex = new Player[6];
	private static int amountOfPlayersInLeftTeam;
	private static int amountOfPlayersInRightTeam;
	private static int selectedMapIndex = 0;
	private static int gameModeIndex = 0;
	private static int goalsNeeded = 15;
	private static int timerLength = 180;
	private static bool periodicalDamageEnabled = true;
	private bool gameStarted;

	// Get references from UI Setup.
	void Start() {
		UISetup setup = GetComponent<UISetup>();
		
		teamColorBlocks = setup.GetTeamColorBlocks();
		leaveTeamButton = setup.GetLeaveTeamButton();
		readyButton = setup.GetReadyButton();
		previousMapButton = setup.GetPreviousMapButton();
		nextMapButton = setup.GetNextMapButton();
		previousGameModeButton = setup.GetPreviousGameModeButton();
		nextGameModeButton = setup.GetNextGameModeButton();
		lessTimeButton = setup.GetLessTimeButton();
		moreTimeButton = setup.GetMoreTimeButton();
		lessGoalsButton = setup.GetLessGoalsButton();
		moreGoalsButton = setup.GetMoreGoalsButton();
		periodicalDamageSwitch = setup.GetPeriodicalDamageSwitch();
	}
	
    void Update() {
		FreeSpaces();
		CheckInput();
		
		// Get active players for current frame.
		Player[] players = PlayerHolder.GetPlayers();
		
		// Count the amount of players who are ready.
		int readyPlayers = 0;
		foreach (Player player in players)
			if (player.GetReady())
				readyPlayers++;

		// If all players are ready and there are more than one in the game, start.
		if (readyPlayers == players.Length && players.Length > 0 && !gameStarted)
			StartMatch();
    }
	
	void FreeSpaces() {
		for(int i = 0; i < teamColorBlocks.Length; i++)
			if(playerByTeamBlockIndex[i] != null && playerByTeamBlockIndex[i].GetDisconnected())
				DeoccupyBlocks(playerByTeamBlockIndex[i]);
	}
	
	void CheckInput() {
		// Get active players for current frame.
		Player[] players = PlayerHolder.GetPlayers();
		
		foreach(Player player in players) {
			if(player.GetCursor().IsCursorPressed()) {
					
				for(int i = 0; i < teamColorBlocks.Length; i++) {
					// Assign new team if a player clicks on an empty square.
					if(player.GetCursor().CursorOverUI(teamColorBlocks[i].gameObject))
						if(playerByTeamBlockIndex[i] == null)
							OccupyNewBlock(player, i);
				}
							
				// Leave any team when the player clicks on that button.
				if(player.GetCursor().CursorOverUI(leaveTeamButton.gameObject))
					DeoccupyBlocks(player);
						
				// Ready up if a player is currently in a team.
				if(player.GetCursor().CursorOverUI(readyButton.gameObject))
					if(player.GetTeam() != Player.Team.Unselected)
						player.SetReady(true);
						
				// Change map to the previous one if the player presses on previous map button.
				if(player.GetCursor().CursorOverUI(previousMapButton.gameObject)) {
					selectedMapIndex--;
					if(selectedMapIndex == -1)
						selectedMapIndex = 2;
					MatchSettings.SetMap((MatchSettings.GameMap)selectedMapIndex);
				}
				
				// Change map to the next one if the player presses on next map button.
				if(player.GetCursor().CursorOverUI(nextMapButton.gameObject)) {
					selectedMapIndex++;
					if(selectedMapIndex == 3)
						selectedMapIndex = 0;
					MatchSettings.SetMap((MatchSettings.GameMap)selectedMapIndex);
				}
				
				// Change game mode to the previous one if the player presses on previous game mode button.
				if(player.GetCursor().CursorOverUI(previousGameModeButton.gameObject)) {
					gameModeIndex--;
					if(gameModeIndex == -1)
						gameModeIndex = 2;
					MatchSettings.SetGameMode((MatchSettings.GameMode)gameModeIndex);
				}
				
				// Change game mode to the next one if the player presses on next game mode button.
				if(player.GetCursor().CursorOverUI(nextGameModeButton.gameObject)) {
					gameModeIndex++;
					if(gameModeIndex == 3)
						gameModeIndex = 0;
					MatchSettings.SetGameMode((MatchSettings.GameMode)gameModeIndex);
				}
				
				// Change timer to 10 secs less if the player presses on less time button.
				if(player.GetCursor().CursorOverUI(lessTimeButton.gameObject)) {
					timerLength -= 10;
					if(timerLength == 0)
						timerLength = 10;
					MatchSettings.SetMatchTime(timerLength);
				}
				
				// Change timer to 10 secs more if the player presses on more time button.
				if(player.GetCursor().CursorOverUI(moreTimeButton.gameObject)) {
					timerLength += 10;
					if(timerLength == 610)
						timerLength = 600;
					MatchSettings.SetMatchTime(timerLength);
				}
				
				// Change needed amount of goals to 1 less if the player presses on less goals button.
				if(player.GetCursor().CursorOverUI(lessGoalsButton.gameObject)) {
					goalsNeeded -= 1;
					if(goalsNeeded == 0)
						goalsNeeded = 1;
					MatchSettings.SetGoalsTillVictory(goalsNeeded);
				}
				
				// Change needed amount of goals to 1 more if the player presses on more goals button.
				if(player.GetCursor().CursorOverUI(moreGoalsButton.gameObject)) {
					goalsNeeded += 1;
					if(goalsNeeded == 100)
						goalsNeeded = 99;
					MatchSettings.SetGoalsTillVictory(goalsNeeded);
				}
				
				// Change needed amount of goals to 1 more if the player presses on more goals button.
				if(player.GetCursor().CursorOverUI(periodicalDamageSwitch.gameObject)) {
					periodicalDamageEnabled = !periodicalDamageEnabled;
					MatchSettings.SetPeriodicalDamageValue(periodicalDamageEnabled);
				}
			}
		}
	}
	
	void OccupyNewBlock(Player newOwner, int selectedBlockIndex) {
		// Deoccupy any previously occupied blocks.
		DeoccupyBlocks(newOwner);
		
		// Occupy the new one.
		playerByTeamBlockIndex[selectedBlockIndex] = newOwner;
		
		if(selectedBlockIndex <= 2) {
			amountOfPlayersInLeftTeam++;
			newOwner.SetTeam(Player.Team.Green);
		} else {
			amountOfPlayersInRightTeam++;
			newOwner.SetTeam(Player.Team.Red);
		}
	}
	
	void DeoccupyBlocks(Player relatedPlayer) {
		for(int i = 0; i < playerByTeamBlockIndex.Length; i++)
			if(playerByTeamBlockIndex[i] == relatedPlayer) {
				playerByTeamBlockIndex[i] = null;
				
				if(i <= 2)
					amountOfPlayersInLeftTeam--;
				else
					amountOfPlayersInRightTeam--;
				
				relatedPlayer.SetTeam(Player.Team.Unselected);
				relatedPlayer.SetReady(false);
				break;
			}
	}
	
	void StartMatch() {
		gameStarted = true;
		StartCoroutine(GameController.LoadMatch());
	}
	
	public static int GetAmountOfPlayersInLeftTeam() {
		return amountOfPlayersInLeftTeam;
	}
	
	public static int GetAmountOfPlayersInRightTeam() {
		return amountOfPlayersInRightTeam;
	}
	
	public static Player[] GetPlayerByTeamBlockIndex() {
		return playerByTeamBlockIndex;
	}
	
	public static int GetSelectedMapIndex() {
		return selectedMapIndex;
	}
	
	public static int GetGameModeIndex() {
		return gameModeIndex;
	}
	
	public static int GetGoalsNeeded() {
		return goalsNeeded;
	}
	
	public static int GetTimerLength() {
		return timerLength;
	}
	
	public static bool GetPeriodicalDamageEnabled() {
		return periodicalDamageEnabled;
	}
	
	public static void ResetSetupData() {
		playerByTeamBlockIndex = new Player[6];
		amountOfPlayersInLeftTeam = 0;
		amountOfPlayersInRightTeam = 0;
	}
}
