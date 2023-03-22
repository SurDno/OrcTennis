using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Controls the behaviour of individual UI elements related to each particular character.
public class CharacterUI : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Image[] abilityImages;
	[SerializeField]private Image abilityBackground;
	[SerializeField]private Image playerColorIndicator;
	[SerializeField]private Text abilityText;
	private CharacterAbilities characterAbilities;
	private CharacterOwner characterOwner;
	
	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterAbilities = GetComponent<CharacterAbilities>();
		
		// Color ability background into player color.
		playerColorIndicator.color = characterOwner.GetOwner().GetColor(); 
		
		// Subscribe onto ability related events.
		characterAbilities.OnAbilityChanged += UpdateAbilityNames;
		characterAbilities.OnAbilityChanged += UpdateAbilityHighlight;
		characterAbilities.OnAbilityAddedOrRemoved += UpdateAbilityIcons;
		
		// Call functions once before events are called.
		UpdateAbilityIcons();
		UpdateAbilityHighlight();
    }
	
	void OnDisable() {
		// Unsubscribe from ability related events.
		characterAbilities.OnAbilityChanged -= UpdateAbilityNames;
		characterAbilities.OnAbilityChanged -= UpdateAbilityHighlight;
		characterAbilities.OnAbilityAddedOrRemoved -= UpdateAbilityIcons;
	}
	
	// Called once when an ability is acquired or lost.
	void UpdateAbilityIcons() {
		// Grey out all the other abilities.
		for(int i = 0; i < abilityImages.Length; i++)
			abilityImages[i].sprite = characterAbilities.CheckAbilitySlot(i) ? characterAbilities.GetAbilityByIndex(i).GetIcon() : null;
		
	}
	
	// Called once when the selected ability changes.
	void UpdateAbilityHighlight() {
		// Grey out the inactive abilities 
		for(int i = 0; i < abilityImages.Length; i++)
			abilityImages[i].color = characterAbilities.CheckAbilitySlot(i) ? new Color32(90, 90, 90, 255): new Color32(41, 41, 41, 255);
		
		// Light the one we have selected.
		abilityImages[characterAbilities.GetSelectedAbilityIndex()].color = Color.white;	
	}
	
	// Dummy method cause we can't call coroutines from actions.
	void UpdateAbilityNames() {
		StopAllCoroutines();
		StartCoroutine(ShowNewAbilityName());
	}
	
	public IEnumerator ShowNewAbilityName() {
		abilityText.text = characterAbilities.GetSelectedAbility().GetName();
		abilityText.color = Color.white;
		
		yield return new WaitForSeconds(1);
		
		// Slowly fade the text out.
		byte alpha = 255;
		while(alpha > 0) {
			alpha -= 3;
			abilityText.color = new Color32(255, 255, 255, alpha);
			yield return new WaitForFixedUpdate();
		}
	}
	
	public void DisableAbilityShowing() {
		abilityBackground.gameObject.SetActive(false);
	}
}
