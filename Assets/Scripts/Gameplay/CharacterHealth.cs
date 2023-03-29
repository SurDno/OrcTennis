using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(CharacterOwner))]
public class CharacterHealth : MonoBehaviour {
	// Events
	public event Action OnHealthChanged;
	
	[Header("Settings")]
	[SerializeField] private int maxHealth = 150;
	[SerializeField] private float damageOffset = 1.5f;
	[SerializeField] private int periodicalDamage = 4;
	[SerializeField] private float damagePeriod = 0.5f;
	
	[Header("Current Values")]
    private Player.Team team;
	private int curHealth;
	
	void Start() {
		team = GetComponent<CharacterOwner>().GetOwner().GetTeam();
		StartCoroutine(CheckForPeriodicalDamage());
		
		curHealth = maxHealth;
	}
	
	IEnumerator CheckForPeriodicalDamage() {
		while(true) {
			switch(team) {
				case Player.Team.Red:
					if(transform.position.x < -damageOffset)
						DealDamage(periodicalDamage);
					break;
				case Player.Team.Green:
					if(transform.position.x > damageOffset)
						DealDamage(periodicalDamage);
					break;
			}
			yield return new WaitForSeconds(damagePeriod);
		}
	}
	
	public void DealDamage(int damage) {
		curHealth -= damage;
		curHealth = Mathf.Max(curHealth, 0);
		OnHealthChanged?.Invoke();
	}
	
	public void Heal(int hp) {
		curHealth += hp;
		curHealth = Mathf.Min(curHealth, maxHealth);
		OnHealthChanged?.Invoke();
	}
	
	public float GetHealthPercentage() {
		return (float)curHealth / (float)maxHealth;
	}
	
	public bool IsDead() {
		return curHealth <= 0;
	}
}
