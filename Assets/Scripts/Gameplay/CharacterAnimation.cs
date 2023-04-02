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
		characterHit.OnCharge += StartChargeAnimation;
		characterHit.OnHit += StartHitAnimation;
		characterHit.OnCancel += CancelChargeAnimation;
    }
	
	void OnDisable() {
		// Unsubscribe from death event.
		characterHealth.OnDeath -= StartDeathAnimation;
		characterHit.OnCharge -= StartChargeAnimation;
		characterHit.OnHit -= StartHitAnimation;
		characterHit.OnCancel -= CancelChargeAnimation;
	}
	
	
	void Update() {
		// Adjust walking animation speed depending on the speed of the character.
		int effects = characterControls.GetEffects();
		float walkSpeed = (effects == 0) ? 1f : (effects > 0) ? 2.0f : 0.5f;
		anim.SetFloat("WalkSpeed", walkSpeed);
		
		// Adjust charging speed animation so that it gets covered in maxChargeTime seconds.
		float chargeSpeed = defaultChargeTime / characterHit.GetChargeMaxTime();
		anim.SetFloat("ChargeSpeed", chargeSpeed);
		
		// Check if we're moving every frame.
		anim.SetBool("Moving", characterControls.GetMoving());
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
	
	void StartChargeAnimation() {
		anim.SetTrigger("StartCharge");
	}
	
	void CancelChargeAnimation() {
		anim.SetTrigger("CancelCharge");
	}
	
	void StartHitAnimation() {
		anim.SetTrigger("Hit");
	}
	
	public void StartResurrectAnimation() {
		anim.SetTrigger("Resurrect");
	}
	
	public void ResetAnimation() {
		anim.Rebind();
		anim.Update(0f);
	}
}
