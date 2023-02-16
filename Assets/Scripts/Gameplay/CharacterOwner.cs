using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOwner : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private ColorObject teamColor;
	
	[Header("Settings")]
    [SerializeField]private Player owner;
	[SerializeField]private PlayerCursor ownerCursor;
	[SerializeField]private Player.Team characterTeam = Player.Team.Unselected;
	
	// Finds a player instance to be the owner.
	void Awake() {
		PlayerHolder holder = GameObject.FindObjectOfType<PlayerHolder>();
		
		// Get active players for current frame.
		Player[] players = holder.GetPlayers();
		
		// Find a player of needed team with no character assigned yet and give them ownership of this unit.
		foreach(Player player in players)
			if(player.GetCharacter() == null && characterTeam == player.GetTeam()) {
				TransferOwnership(player);
				player.SetCharacter(this);
				return;
			}
		
		// If we haven't found a potential owner, destroy gameobject.
		Destroy(this.gameObject);
	}
	
	
	// Used to give ownership of the unit to any specific player.
	public void TransferOwnership(Player newOwner) {
		owner = newOwner;
		ownerCursor = owner != null ? owner.GetCursor() : null;
		teamColor.objColor = newOwner.GetColor();
	}
	
	public PlayerCursor GetCursor() {
		return ownerCursor;
	}
	
	public Player GetOwner() {
		return owner;
	}
}
