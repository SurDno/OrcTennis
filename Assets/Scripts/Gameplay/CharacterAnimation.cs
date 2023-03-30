using UnityEngine;

[RequireComponent(typeof(CharacterOwner))]
[RequireComponent(typeof(CharacterControls))]
[RequireComponent(typeof(CharacterHit))]
public class CharacterAnimation : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private Animator anim;
	private CharacterOwner characterOwner;
	private CharacterControls characterControls;
	private CharacterHit characterHit;
	private CharacterHealth characterHealth;
	
	[Header("Settings")]
	private float defaultChargeTime;

	// Initializing cached GameObjects and Components.
    void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterControls = GetComponent<CharacterControls>();
		characterHit = GetComponent<CharacterHit>();
		characterHealth = GetComponent<CharacterHealth>();
		
		// Get default charge speed.
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
		foreach(AnimationClip clip in clips)
			if(clip.name == "Orc_rig|HitCharge")
				defaultChargeTime = clip.length; 
		
		
		// Subscribe on death event.
		characterHealth.OnDeath += StartDeathAnimation;
    }
	
	void OnDisable() {
		// Unsubscribe from death event.
		characterHealth.OnDeath -= StartDeathAnimation;
	}
	
	
	void Update() {
		// Adjust walking animation speed depending on the speed of the character.
		int effects = characterControls.GetEffects();
		float walkSpeed = (effects == 0) ? 1f : (effects > 0) ? 2.0f : 0.5f;
		anim.SetFloat("WalkSpeed", walkSpeed);
		
		// Adjust charging speed animation so that it gets covered in maxChargeTime seconds.
		float chargeSpeed = defaultChargeTime / characterHit.GetChargeMaxTime();
		anim.SetFloat("ChargeSpeed", chargeSpeed);
		
		// we need to get all necessary values from other scripts firts
		bool moving = characterControls.GetMoving();
		bool startCharge = GamepadInput.GetRightTrigger(characterOwner.GetOwner().GetGamepad());
		bool hit = !GamepadInput.GetRightTrigger(characterOwner.GetOwner().GetGamepad());
		
		// And then use it for animation data
		anim.SetBool("Moving", moving);
		anim.SetBool("StartCharge", startCharge);
		anim.SetBool("Hit", hit);
	}
	
	public void StartVictoryAnimations() {
		anim.SetTrigger("Win" + Random.Range(1, 4));
		
	}
	
	public void StartDefeatAnimations() {
		anim.SetTrigger("Lose");
	}
	
	void StartDeathAnimation() {
		anim.SetTrigger("Death");
	}
	
	public void ResetAnimation() {
		anim.Rebind();
		anim.Update(0f);
	}
}
