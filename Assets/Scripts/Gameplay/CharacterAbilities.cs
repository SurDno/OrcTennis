using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterOwner))]
public class CharacterAbilities : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Image[] abilityImages;
	[SerializeField]private Image abilityBackground;
	[SerializeField]private Image playerColorIndicator;
	private CharacterOwner characterOwner;
	
	[Header("Settings")]
	private bool temp;
	
	[Header("Current Values")]
	private bool temps;
	
	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		
		// Get initial ability selection.
		SelectOneAbility(2);
		
		// Color ability background into player color.
		playerColorIndicator.color = characterOwner.GetOwner().GetColor(); 
    }

    // Update is called once per frame
    void Update() {
		if(GamepadInput.GetNorthButtonDown(characterOwner.GetOwner().GetGamepad()))
			SelectOneAbility(0);
		else if(GamepadInput.GetWestButtonDown(characterOwner.GetOwner().GetGamepad()))
			SelectOneAbility(1);
		else if(GamepadInput.GetSouthButtonDown(characterOwner.GetOwner().GetGamepad()))
			SelectOneAbility(2);
		else if(GamepadInput.GetEastButtonDown(characterOwner.GetOwner().GetGamepad()))
			SelectOneAbility(3);
    }
	
	void SelectOneAbility(int abilityIndex) {
		// Grey out all the other abilities.
		foreach(Image abilityImage in abilityImages)
			abilityImage.color = Color.gray;
		
		// Light the one we have selected.
		abilityImages[abilityIndex].color = Color.white;
	}
	
	public void DisableAbilityShowing() {
		abilityBackground.gameObject.SetActive(false);
	}
}
