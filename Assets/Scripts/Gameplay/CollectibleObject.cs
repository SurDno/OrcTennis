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
		// Don't give Heal ability if periodical damage is disabled.
		if(MatchSettings.GetPeriodicalDamageEnabled())
			return abilities[Random.Range(0, abilities.Length)];
		else 
			return abilities[Random.Range(0, abilities.Length - 1)];
	}
	
	public void AssignSpawner(Spawner newSpawner) {
		spawner = newSpawner;
	}
}
