using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerSetup))]
public class UISetup : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Text tip;
	[SerializeField]private Text[] playerNumbers;
	[SerializeField]private Image[] playerColorBlocks;
	[SerializeField]private Text[] playerReadyStatuses;
	[SerializeField]private Image[] teamColorBlocks;
	[SerializeField]private Text warningBalance;
	[SerializeField]private Text warningChoose;
	[SerializeField]private Text warningReady;
	[SerializeField]private Image leaveTeamButton;
	[SerializeField]private Image readyButton;
	
	void Update() {
		UpdateUI();
	}
	
	void UpdateUI() {
		// Get active players for current frame.
		Player[] players = PlayerHolder.GetPlayers();
		
		// Activate a tip if no gamepads are connected.
		tip.enabled = players.Length == 0;
		
		// Activate UI elements for players that are present.
		for(int i = 0; i < playerNumbers.Length; i++) {
			playerNumbers[i].enabled = i < players.Length;
			playerColorBlocks[i].enabled = i < players.Length;
			playerReadyStatuses[i].enabled = i < players.Length;
		}
		
		// Get individual states for whether the player pressed the ready button and what their color is.
		for(int i = 0; i < players.Length; i++) {
			playerColorBlocks[i].color = players[i].GetColor();
			playerReadyStatuses[i].text = players[i].GetReady() ? "Ready" : "Not Ready";
		}
		
		// Color each block in each team in the player color, or white, if no player occupied that.
		for(int i = 0; i < teamColorBlocks.Length; i++)
			teamColorBlocks[i].color = PlayerSetup.GetPlayerByTeamBlockIndex()[i] == null ? Color.white : PlayerSetup.GetPlayerByTeamBlockIndex()[i].GetColor();
		
		// Display a warning if teams are unbalanced.
		warningBalance.enabled = PlayerSetup.GetAmountOfPlayersInLeftTeam() != PlayerSetup.GetAmountOfPlayersInRightTeam();

		// Display a warning if someone has not yet chosen team.
		warningChoose.enabled = false;
		for(int i = 0; i < players.Length; i++)
			if(players[i].GetTeam() == Player.Team.Unselected) {
				warningChoose.enabled = true;
				break;
			}
		
		// Display a warning if someone is not yet ready.
		warningReady.enabled = false;
		for(int i = 0; i < players.Length; i++)
			if(players[i].GetReady() == false) {
				warningReady.enabled = true;
				break;
			}
	}
	
	public Image[] GetTeamColorBlocks() {
		return teamColorBlocks;
	}
	
	public Image GetLeaveTeamButton() {
		return leaveTeamButton;
	}
	
	public Image GetReadyButton() {
		return readyButton;
	}
}
