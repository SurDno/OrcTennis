using UnityEngine;
using UnityEngine.UI;

// Controls the behaviour of individual UI elements related to each particular character.
public class CharacterUI : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Image[] abilityImages;
	[SerializeField]private Image abilityBackground;
	[SerializeField]private Image playerColorIndicator;
	private CharacterAbilities characterAbilities;
	private CharacterOwner characterOwner;
	
	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterAbilities = GetComponent<CharacterAbilities>();
		
		// Color ability background into player color.
		playerColorIndicator.color = characterOwner.GetOwner().GetColor(); 
    }
	
	void Update() {
		UpdateAbilityIcons();
		UpdateAbilityHighlight();
	}
	
	// Ideally this should only be called once when an ability is acquired or lost. Right now this is called in update.
	void UpdateAbilityIcons() {
		// Grey out all the other abilities.
		for(int i = 0; i < abilityImages.Length; i++)
			abilityImages[i].sprite = characterAbilities.CheckAbilitySlot(i) ? characterAbilities.GetAbilityByIndex(i).GetIcon() : null;
		
	}
	
	// Ideally this should only be called once when the selected ability changes. Right now this is called in update.
	void UpdateAbilityHighlight() {
		// Grey out the inactive abilities 
		for(int i = 0; i < abilityImages.Length; i++)
			abilityImages[i].color = characterAbilities.CheckAbilitySlot(i) ? new Color32(90, 90, 90, 255): new Color32(41, 41, 41, 255);
		
		// Light the one we have selected.
		abilityImages[characterAbilities.GetSelectedAbilityIndex()].color = Color.white;	
	}
	
	public void DisableAbilityShowing() {
		abilityBackground.gameObject.SetActive(false);
	}
}
