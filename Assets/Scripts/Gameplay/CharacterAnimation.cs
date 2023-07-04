using UnityEngine;
using System.Linq;

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
    private void Start() {
		characterOwner = GetComponent<CharacterOwner>();
		characterControls = GetComponent<CharacterControls>();
		characterHit = GetComponent<CharacterHit>();
		characterHealth = GetComponent<CharacterHealth>();
		
		// Get default charge speed.
		defaultChargeTime = anim.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name == "Orc_rig|HitCharge").length;

		// Subscribe on death event.
		characterHealth.OnDeath += StartDeathAnimation;
		characterHit.OnCharge += StartChargeAnimation;
		characterHit.OnHit += StartHitAnimation;
		characterHit.OnCancel += CancelChargeAnimation;
    }
	
	private void OnDisable() {
		// Unsubscribe from death event.
		characterHealth.OnDeath -= StartDeathAnimation;
		characterHit.OnCharge -= StartChargeAnimation;
		characterHit.OnHit -= StartHitAnimation;
		characterHit.OnCancel -= CancelChargeAnimation;
	}
	
	private void Update() {
		// Adjust walking animation speed depending on the speed of the character.
		int effects = characterControls.GetEffects();
		float walkSpeed = (effects == 0) ? 1f : (effects > 0) ? 2.0f : 0.5f;
		anim.SetFloat("WalkSpeed", walkSpeed);
		
		// Adjust charging speed animation so that it gets covered in maxChargeTime seconds.
		anim.SetFloat("ChargeSpeed", defaultChargeTime / characterHit.GetChargeMaxTime());
		
		// Check if we're moving every frame.
		anim.SetBool("Moving", characterControls.GetMoving());
	}
	
	public void StartVictoryAnimations() => anim.SetTrigger("Win" + Random.Range(1, 4));
	public void StartDefeatAnimations() => anim.SetTrigger("Lose");
	private void StartDeathAnimation() => anim.SetTrigger("Death");
	private void StartChargeAnimation() => anim.SetTrigger("StartCharge");
	private void CancelChargeAnimation() => anim.SetTrigger("CancelCharge");
	private void StartHitAnimation() => anim.SetTrigger("Hit");
	public void StartResurrectAnimation() => anim.SetTrigger("Resurrect");
	
	public void ResetAnimation() {
		anim.Rebind();
		anim.Update(0f);
	}
}
