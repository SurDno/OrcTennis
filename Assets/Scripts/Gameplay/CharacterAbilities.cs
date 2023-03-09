using UnityEngine;

[RequireComponent(typeof(CharacterOwner))]
public class CharacterAbilities : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	private CharacterOwner characterOwner;
	
	[Header("Settings")]
	private bool temp;
	
	[Header("Current Values")]
	private bool temp;
	
	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
    }

    // Update is called once per frame
    void Update() 
		//if(GamepadInput.Get(characterOwner.GetOwner().GetGamepad())) {
		//	// Check if we can hit right now.
		//	if(!charging && !onCooldown)
		//		StartCharge();
		//}
    }
}
