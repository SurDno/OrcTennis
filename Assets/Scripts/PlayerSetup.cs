using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UISetup))]
public class PlayerSetup : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	private Image[] teamColorBlocks;
	private Image leaveTeamButton;
	private Image readyButton;
	
	[Header("Current Settings")]
	private static Player[] playerByTeamBlockIndex = new Player[6];
	private static int amountOfPlayersInLeftTeam;
	private static int amountOfPlayersInRightTeam;

	// Get references from UI Setup.
	void Start() {
		UISetup setup = GetComponent<UISetup>();
		
		teamColorBlocks = setup.GetTeamColorBlocks();
		leaveTeamButton = setup.GetLeaveTeamButton();
		readyButton = setup.GetReadyButton();
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
		if (readyPlayers == players.Length && players.Length > 1)
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
		
		foreach(Player player in players)
			for(int i = 0; i < teamColorBlocks.Length; i++) {
				if(player.GetCursor().IsCursorPressed()) {
					// Assign new team if a player clicks on an empty square.
					if(player.GetCursor().CursorOverUI(teamColorBlocks[i].gameObject))
						if(playerByTeamBlockIndex[i] == null)
							OccupyNewBlock(player, i);
						
					// Leave any team when the player clicks on that button.
					if(player.GetCursor().CursorOverUI(leaveTeamButton.gameObject))
						DeoccupyBlocks(player);
						
					// Ready up if a player is currently in a team.
					if(player.GetCursor().CursorOverUI(readyButton.gameObject))
						if(player.GetTeam() != Player.Team.Unselected)
							player.SetReady(true);
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
		GameController.SetGameState(GameController.GameState.Match);
		SceneManager.LoadScene("Game");
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
}
