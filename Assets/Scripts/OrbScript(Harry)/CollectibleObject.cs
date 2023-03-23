using UnityEngine;

// Gives a random ability upon pickup.
public class CollectibleObject : MonoBehaviour {
	private Spawner spawner;
	private Spell[] abilities = {
		new SuperKnockback(),
		new BeastHaste(),
		new Telekinesis(),
		new EarthSlam(),
		new RandomBlink(),
		new ElectricShield(),
		new Dash()
	};

	private void OnTriggerEnter(Collider other) {
		Spell newAbility = GenerateSpell();
		other.GetComponent<CharacterAbilities>().ReceiveAbility(newAbility);
		spawner.StartCoroutine(spawner.RespawnAfterDelay());
		Destroy(this.gameObject);
	}
	
	Spell GenerateSpell() {
		return abilities[Random.Range(0, abilities.Length)];
	}
	
	public void AssignSpawner(Spawner newSpawner) {
		spawner = newSpawner;
	}
}
