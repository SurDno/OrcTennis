using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterUI))]
public class CharacterOwner : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private SkinnedMeshRenderer skinnedMeshRenderer;
	
	[Header("Settings")]
    [SerializeField]private Player owner;
	[SerializeField]private PlayerCursor ownerCursor;
	[SerializeField]private Player.Team characterTeam = Player.Team.Unselected;
	
	void Awake() {
		FindNewOwner(true);
		
		// If we haven't found a potential owner, disable ability UI and destroy gameobject.
		if(owner == null) {
			GetComponent<CharacterUI>().DisableAbilityShowing();
			Destroy(this.gameObject);
			
		}
	}
	
	void Update() {
		if(owner != null && owner.GetDisconnected()) {
			owner.SetCharacter(null);
			owner = null;
		}
			
		if(owner == null)
			FindNewOwner(false);
	}
	
	// Finds a player instance to be the owner.
	void FindNewOwner(bool lookForTeam) {
		// Get active players for current frame.
		Player[] players = PlayerHolder.GetPlayers();
		
		// Find a player of needed team with no character assigned yet and give them ownership of this unit.
		foreach(Player player in players)
			if(player.GetCharacter() == null && (characterTeam == player.GetTeam()) == lookForTeam) {
				TransferOwnership(player);
				player.SetCharacter(this);
				player.SetTeam(characterTeam);
				return;
			}
	}
	
	// Used to give ownership of the unit to any specific player.
	public void TransferOwnership(Player newOwner) {
		owner = newOwner;
		ownerCursor = owner != null ? owner.GetCursor() : null;
		
		// Apply new color.
		Color32 newColor;
		if(owner == null)
			newColor = Color.black;
		else {
			// Get color, preserve hue, max the saturation and value.
			float hue;
			Color.RGBToHSV(owner.GetColor(), out hue, out _, out  _);
			newColor = Color.HSVToRGB(hue, 1, 1);
		}
		skinnedMeshRenderer.material.SetColor("_EmissionColor", newColor);
	}
	
	public PlayerCursor GetCursor() {
		return ownerCursor;
	}
	
	public Player GetOwner() {
		return owner;
	}
}
