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
	[SerializeField]private Image previousMapButton;
	[SerializeField]private Image nextMapButton;
	[SerializeField]private Image mapImage;
	[SerializeField]private Image previousGameModeButton;
	[SerializeField]private Image nextGameModeButton;
	[SerializeField]private Text gameModeText;
	[SerializeField]private Sprite[] mapImages;
	[SerializeField]private GameObject goalsSettings;
	[SerializeField]private GameObject timeSettings;
	[SerializeField]private Image lessTimeButton;
	[SerializeField]private Image moreTimeButton;
	[SerializeField]private Text timeText;
	[SerializeField]private Image lessGoalsButton;
	[SerializeField]private Image moreGoalsButton;
	[SerializeField]private Text goalsText;
	[SerializeField]private Image periodicalDamageSwitch;
	[SerializeField]private Sprite[] switchStates;
	
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
			playerColorBlocks[i].gameObject.SetActive(i < players.Length);
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
		warningBalance.gameObject.transform.parent.gameObject.SetActive(PlayerSetup.GetAmountOfPlayersInLeftTeam() != PlayerSetup.GetAmountOfPlayersInRightTeam());

		// Display a warning if someone has not yet chosen team.
		warningChoose.gameObject.transform.parent.gameObject.SetActive(false);
		for(int i = 0; i < players.Length; i++)
			if(players[i].GetTeam() == Player.Team.Unselected) {
				warningChoose.gameObject.transform.parent.gameObject.SetActive(true);
				break;
			}
		
		// Display a warning if someone is not yet ready.
		warningReady.gameObject.transform.parent.gameObject.SetActive(false);
		for(int i = 0; i < players.Length; i++)
			if(players[i].GetReady() == false) {
				warningReady.gameObject.transform.parent.gameObject.SetActive(true);
				break;
			}
			
		// Display the map image for the currently selected map.
		mapImage.sprite = mapImages[PlayerSetup.GetSelectedMapIndex()];
		
		// Display the game mode name for currently selected game mode.
		gameModeText.text = PlayerSetup.GetGameModeIndex() == 0 ? "Classic" : "Time Attack";
		
		// Display the other setting according to selected game mode.
		goalsSettings.SetActive(PlayerSetup.GetGameModeIndex() == 0);
		timeSettings.SetActive(PlayerSetup.GetGameModeIndex() == 1);
		
		timeText.text = PlayerSetup.GetTimerLength().ToString();
		goalsText.text = PlayerSetup.GetGoalsNeeded().ToString();
		
		periodicalDamageSwitch.sprite = switchStates[PlayerSetup.GetPeriodicalDamageEnabled() ? 1 : 0];
		
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
	
	public Image GetPreviousMapButton() {
		return previousMapButton;
	}
	
	public Image GetNextMapButton() {
		return nextMapButton;
	}
	
	public Image GetPreviousGameModeButton() {
		return previousGameModeButton;
	}
	
	public Image GetNextGameModeButton() {
		return nextGameModeButton;
	}
	
	public Image GetLessTimeButton() {
		return lessTimeButton;
	}

	public Image GetMoreTimeButton() {
		return moreTimeButton;
	}
	
	public Image GetLessGoalsButton() {
		return lessGoalsButton;
	}
	
	public Image GetMoreGoalsButton() {
		return moreGoalsButton;
	}
	
	public Image GetPeriodicalDamageSwitch() {
		return periodicalDamageSwitch;
	}
}
