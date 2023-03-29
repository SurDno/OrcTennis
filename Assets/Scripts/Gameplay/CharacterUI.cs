using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Controls the behaviour of individual UI elements related to each particular character.
[RequireComponent(typeof(CharacterOwner))]
[RequireComponent(typeof(CharacterAbilities))]
[RequireComponent(typeof(CharacterHealth))]
public class CharacterUI : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Image[] abilityImages;
	[SerializeField]private Image abilityBackground;
	[SerializeField]private Image[] playerColorIndicators;
	[SerializeField]private Text abilityText;
	[SerializeField]private Image healthbar;
	private CharacterAbilities characterAbilities;
	private CharacterHealth characterHealth;
	private CharacterOwner characterOwner;
	
	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterAbilities = GetComponent<CharacterAbilities>();
		characterHealth = GetComponent<CharacterHealth>();
		
		// Color small circles representing players into player color.
		foreach(Image playerColorIndicator in playerColorIndicators)
			playerColorIndicator.color = characterOwner.GetOwner().GetColor(); 
		
		// Subscribe onto ability related events.
		characterAbilities.OnAbilityChanged += UpdateAbilityNames;
		characterAbilities.OnAbilityChanged += UpdateAbilityHighlight;
		characterAbilities.OnAbilityAddedOrRemoved += UpdateAbilityIcons;
		characterAbilities.OnAbilityAddedOrRemoved += UpdateAbilityHighlight;
		
		// Subscribe onto health event.
		characterHealth.OnHealthChanged += UpdateHealthbarFill;
		
		// Call functions once before events are called.
		UpdateAbilityIcons();
		UpdateAbilityHighlight();
    }
	
	void OnDisable() {
		// Unsubscribe from ability related events.
		characterAbilities.OnAbilityChanged -= UpdateAbilityNames;
		characterAbilities.OnAbilityChanged -= UpdateAbilityHighlight;
		characterAbilities.OnAbilityAddedOrRemoved -= UpdateAbilityIcons;
		characterAbilities.OnAbilityAddedOrRemoved -= UpdateAbilityHighlight;
		
		// Unsubscribe from health event.
		characterHealth.OnHealthChanged += UpdateHealthbarFill;
	}
	
	void Update() {
		UpdateHealthbarPosition();
	}
	
	// Called once when an ability is acquired or lost.
	void UpdateAbilityIcons() {
		// Give images to the abilities we have, stop rendering those we don't.
		for(int i = 0; i < abilityImages.Length; i++) {
			abilityImages[i].sprite = characterAbilities.CheckAbilitySlot(i) ? characterAbilities.GetAbilityByIndex(i).GetIcon() : null;
			abilityImages[i].gameObject.transform.parent.gameObject.SetActive(characterAbilities.CheckAbilitySlot(i));
		}
	}
	
	// Called once when the selected ability changes.
	void UpdateAbilityHighlight() {
		// Grey out the inactive abilities 
		for(int i = 0; i < abilityImages.Length; i++)
			abilityImages[i].color = new Color32(90, 90, 90, 255);
		
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
	
	public void DisablePlayerSpecificUI() {
		abilityBackground.gameObject.SetActive(false);
		healthbar.gameObject.transform.parent.gameObject.SetActive(false);
	}
	
	void UpdateHealthbarPosition()  {
		// Get new healthbar position from player position.
		Vector3 newHealthbarPos = Camera.main.WorldToScreenPoint(transform.position);
		//newHealthbarPos.z = 0;
		newHealthbarPos.y += 60;
		
		// Move the healthbar.
		healthbar.gameObject.transform.parent.gameObject.transform.position = newHealthbarPos;
	}
	
	void UpdateHealthbarFill() {
		healthbar.fillAmount = characterHealth.GetHealthPercentage();
	}
}
