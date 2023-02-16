using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO: UI and technical implementation should be separated into two different scripts to make the code more readable.
public class PlayerSetup : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Text[] playerNumbers;
	[SerializeField]private Image[] playerColorBlocks;
	[SerializeField]private Text[] playerReadyStatuses;
	[SerializeField]private Image[] teamColorBlocks;
	[SerializeField]private Image leaveTeamButton;
	[SerializeField]private Image readyButton;
	[SerializeField]private Text tip;
	[SerializeField]private Text warningBalance;
	[SerializeField]private Text warningChoose;
	[SerializeField]private Text warningReady;
	private PlayerHolder holder;
	
	[Header("Current Settings")]
	private Player[] playerByTeamBlockIndex = new Player[6];
	private int amountOfPlayersInLeftTeam;
	private int amountOfPlayersInRightTeam;
		
	// Initializes cached GameObjects and Components.
	void Start() {
		holder = GameObject.Find("PlayerHolder").GetComponent<PlayerHolder>();
    }

    // Update is called once per frame
    void Update() {
		FreeSpaces();
		CheckInput();
		UpdateUI();
		
		// Get active players for current frame.
		Player[] players = holder.GetPlayers();
		
		// Count the amount of players who are ready.
		int readyPlayers = 0;
		foreach (Player player in players)
			if (player.GetReady())
				readyPlayers++;

		// If all players are ready and there are more than one in the game, start.
		if (readyPlayers == players.Length && players.Length > 1)
			SceneManager.LoadScene("Game");
    }
	
	void FreeSpaces() {
		for(int i = 0; i < teamColorBlocks.Length; i++)
			if(playerByTeamBlockIndex[i] != null && playerByTeamBlockIndex[i].GetDisconnected())
				DeoccupyBlocks(playerByTeamBlockIndex[i]);
	}
	
	void CheckInput() {
		// Get active players for current frame.
		Player[] players = holder.GetPlayers();
		
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
	
	void UpdateUI() {
		// Get active players for current frame.
		Player[] players = holder.GetPlayers();
		
		// Activate a tip if no gamepads are connected.
		tip.enabled = players.Length == 0;
		
		// Activate UI elements for players that are present.
		for(int i = 0; i < playerNumbers.Length; i++) {
			playerNumbers[i].enabled = i < players.Length;
			playerColorBlocks[i].enabled = i < players.Length;
			playerReadyStatuses[i].enabled = i < players.Length;
		}
		
		// Get individual states for whether the player pressed the ready button, what their color is, and whether their gamepad is connected right now.
		for(int i = 0; i < players.Length; i++) {
			playerColorBlocks[i].color = players[i].GetColor();
			playerReadyStatuses[i].text = players[i].GetReady() ? "Ready" : "Not Ready";
		}
		
		// Color each block in each team in the player color, or white, if no player occupied that.
		for(int i = 0; i < teamColorBlocks.Length; i++)
			teamColorBlocks[i].color =  playerByTeamBlockIndex[i] == null ? Color.white : playerByTeamBlockIndex[i].GetColor();
		
		// Display a warning if teams are unbalanced.
		warningBalance.enabled = amountOfPlayersInLeftTeam != amountOfPlayersInRightTeam;

		// Display a warning if someone has not yet chosen team.
		warningChoose.enabled = false;
		for(int i = 0; i < players.Length; i++)
			if(players[i].GetTeam() == Player.Team.Unselected)
				warningChoose.enabled = true;
		
		// Display a warning if someone is not yet ready.
		warningReady.enabled = false;
		for(int i = 0; i < players.Length; i++)
			if(players[i].GetReady() == false)
				warningReady.enabled = true;
	}
}
