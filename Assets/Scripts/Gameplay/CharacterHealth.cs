using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterOwner))]
public class CharacterHealth : MonoBehaviour {
	// Events
	public event Action OnHealthChanged;
	public event Action OnDeath;
	
	[Header("Settings")]
	[SerializeField] private int maxHealth = 100;
	[SerializeField] private float damageOffset = 1.5f;
	[SerializeField] private int periodicalDamage = 3;
	[SerializeField] private float damagePeriod = 0.5f;
	
	[Header("Current Values")]
    private Player.Team team;
	private int curHealth;
	private bool dead;
	
	void Start() {
		team = GetComponent<CharacterOwner>().GetOwner().GetTeam();
		if(MatchSettings.GetPeriodicalDamageEnabled())
			StartCoroutine(CheckForPeriodicalDamage());
		
		curHealth = maxHealth;
	}
	
	IEnumerator CheckForPeriodicalDamage() {
		while(true) {
			if(MatchController.GetMatchState() !=  MatchController.MatchState.Victory && !dead) {
				switch(team) {
					case Player.Team.Red:
						if(transform.position.x < -damageOffset)
							DealPeriodicalDamage();
						break;
					case Player.Team.Green:
						if(transform.position.x > damageOffset)
							DealPeriodicalDamage();
						break;
				}
			}
			yield return new WaitForSeconds(damagePeriod);
		}
	}
	
	public void DealPeriodicalDamage() {
		// Create an effect on top of the hit point.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/FireHit");
		GameObject instance = Instantiate(magicEffectPrefab, transform.position, Quaternion.identity);
		
		// Play sound.	
		SoundManager.PlaySound(new string[] {"FireHit4", "FireHit5"}, 0.3f);
		
		// Deal damage.
		DealDamage(periodicalDamage);
	}
	
	public void DealDamage(int damage) {
		
		curHealth -= damage;
		curHealth = Mathf.Max(curHealth, 0);
		OnHealthChanged?.Invoke();
		
		if(curHealth == 0 && !dead)
			Die();
	}
	
	public void Heal(int hp) {
		dead = false;
		curHealth += hp;
		curHealth = Mathf.Min(curHealth, maxHealth);
		OnHealthChanged?.Invoke();
	}
	
	// If no value is parsed, heal to full HP.
	public void Heal() {
		Heal(maxHealth);
	}
	
	void Die() {
		dead = true;
		OnDeath?.Invoke();
		MatchController.RegisterDeath();
	}
	
	public float GetHealthPercentage() {
		return (float)curHealth / (float)maxHealth;
	}
	
	public bool IsDead() {
		return dead;
	}
}
