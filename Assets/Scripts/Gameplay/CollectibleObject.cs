using UnityEngine;

// Gives a random ability upon pickup.
public class CollectibleObject : MonoBehaviour {
	private Spawner spawner;
	private Spell[] abilities = {
		new SuperKnockback(),
		new BeastHaste(),
		new Telekinesis(),
		new EarthSlam(),
		new ElectricShield(),
		new Dash(),
		new FreezingField(),
		new KnockbackImmunity(),
		new FireShield(),
		new MassHeal(),
		new RaiseUndead()
	};

	private void OnTriggerEnter(Collider other) {
		Spell newAbility = GenerateSpell();
		
		bool success;
		other.GetComponent<CharacterAbilities>().ReceiveAbility(newAbility, out success);
		
		// If we were able to find a place for the ability, destroy it. If not, leave it there.
		if(success) {
			spawner.StartCoroutine(spawner.RespawnAfterDelay());
			
			// Create an effect on top of the pickup.
			GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/MagicOrbPickup");
			Vector3 effectPosition = transform.position;
			Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
			
			Destroy(this.gameObject);
		}
	}
	
	Spell GenerateSpell() {
		// Don't give Mass Heal, Raise Undead and Fire Shield abilities if periodic damage is disabled.
		if(!MatchSettings.GetPeriodicalDamageEnabled())
			return abilities[Random.Range(0, abilities.Length - 3)];
		// Don't give Mass Heal and Raise Undead if playing 1v1. TODO: Actually check if the team has allies because 2v1 is still possible.
		else if(PlayerHolder.GetPlayers().Length <= 2)
			return abilities[Random.Range(0, abilities.Length - 2)];
		else
			return abilities[Random.Range(0, abilities.Length)];
	}
	
	public void AssignSpawner(Spawner newSpawner) {
		spawner = newSpawner;
	}
}
