using UnityEngine;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Text[] playerNumbers;
	[SerializeField]private Image[] playerColorBlocks;
	[SerializeField]private Text[] playerReadyStatuses;
	[SerializeField]private Text[] playerConnectionStatuses;
	[SerializeField]private Text[] playerDisconnectButtons;
	[SerializeField]private PlayerHolder holder;
	
	// Initializes cached GameObjects and Components.
	void Start() {
		holder = GameObject.Find("PlayerHolder").GetComponent<PlayerHolder>();
    }

    // Update is called once per frame
    void Update() {
		// Deactivate all UI elements before enabling them back selectively.
		DeactivateAllUI();
		
		// Get active players for current frame.
		Player[] players = holder.GetPlayers();
		
		// Activate UI elements for players that are present.
		for(int i = 0; i < players.Length; i++) {
			playerNumbers[i].enabled = true;
			playerColorBlocks[i].enabled = true;
			playerReadyStatuses[i].enabled = true;
			playerConnectionStatuses[i].enabled = true;
		}
		
		for(int i = 0; i < players.Length; i++) {
			playerColorBlocks[i].color = players[i].GetColor();
			playerReadyStatuses[i].text = players[i].GetReady() ? "Ready" : "Not Ready";
			playerConnectionStatuses[i].text = players[i].GetDisconnected() ? "Disconnected" : "Connected";
			playerDisconnectButtons[i].enabled = players[i].GetDisconnected();
		}
    }
	
	// Deactivates all UI objects before enabling them individually.
	void DeactivateAllUI() {
		foreach(Text obj in playerNumbers)
			obj.enabled = false;
		
		foreach(Image obj in playerColorBlocks)
			obj.enabled = false;
			
		foreach(Text obj in playerReadyStatuses)
			obj.enabled = false;
			
		foreach(Text obj in playerConnectionStatuses)
			obj.enabled = false;
			
		foreach(Text obj in playerDisconnectButtons)
			obj.enabled = false;
	}
}
