using UnityEngine;

[RequireComponent(typeof(CharacterOwner))]
[RequireComponent(typeof(CharacterHit))]
public class CharacterAbilities : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	private CharacterOwner characterOwner;
	private CharacterHit characterHit;
	
	[Header("Current Values")]
	private int selectedAbilityIndex;
	private Spell[] abilities = new Spell[4];
	
	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterHit = GetComponent<CharacterHit>();
		
		// Give initial abilities.
		abilities[2] = new Knockback();
		abilities[3] = new Fast();
		
		// Get initial ability selection.
		SelectOneAbility(2);
    }

    // Update is called once per frame
    void Update() {
        if(characterOwner.GetOwner() == null)
			return;
		
		// Only cast the ability from here if it's an instant cast and not a hit type.
		if(GamepadInput.GetRightTriggerDown(characterOwner.GetOwner().GetGamepad()))
			if(GetSelectedAbility().GetCastType() == Spell.CastType.Instant)
					CastSelectedAbility();
		
		// If we're not charging right now, switch between abilities using ABXY gamepad buttons.
		if(!characterHit.GetCharging()) {
			if(GamepadInput.GetNorthButtonDown(characterOwner.GetOwner().GetGamepad()))
				SelectAbilitySafe(0);
			else if(GamepadInput.GetWestButtonDown(characterOwner.GetOwner().GetGamepad()))
				SelectAbilitySafe(1);
			else if(GamepadInput.GetSouthButtonDown(characterOwner.GetOwner().GetGamepad()))
				SelectAbilitySafe(2);
			else if(GamepadInput.GetEastButtonDown(characterOwner.GetOwner().GetGamepad()))
				SelectAbilitySafe(3);
		}
    }
	
	void SelectAbilitySafe(int abilityIndex) {
		// Only select an ability if there's something in that slot.
		if(!CheckAbilitySlot(abilityIndex))
			return;
		
		SelectOneAbility(abilityIndex);
	}
	
	// For abilities that are not hit types but merely need their Cast() function to be called.
	void CastSelectedAbility() {
		StartCoroutine(GetSelectedAbility().Cast(this));
		DestroySelectedAbility();
	}
	
	// Destroys the selected ability if it needs to be destroyed.
	public void DestroySelectedAbility() {
		if(GetSelectedAbility().GetSingleUse()) {
			abilities[selectedAbilityIndex] = null;
			SelectOneAbility(2);
		}
	}
	
	// Changes current selected ability by index.
	void SelectOneAbility(int abilityIndex) {
		selectedAbilityIndex = abilityIndex;
	}
	
	// Returns the spell that is currently selected.
	public Spell GetSelectedAbility() {
		return abilities[selectedAbilityIndex];
	}
	
	// Returns the spell by its index.
	public Spell GetAbilityByIndex(int abilityIndex) {
		return abilities[abilityIndex];
	}
	
	// Returns whether the ability slot is occupied.
	public bool CheckAbilitySlot(int abilityIndex) {
		return abilities[abilityIndex] != null;
	}
	
	// Returns the current selected ability index.
	public int GetSelectedAbilityIndex() {
		return selectedAbilityIndex;
	}
	
	// Puts a new ability into an empty slot if there is one.
	public void ReceiveAbility(Spell newAbility) {
		if(!CheckAbilitySlot(0))
			abilities[0] = newAbility;
		else if(!CheckAbilitySlot(0))
			abilities[1] = newAbility;
	}
}
