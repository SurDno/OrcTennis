using UnityEngine;

// Gives a random ability upon pickup.
public class CollectibleObject : MonoBehaviour {
	private Spawner spawner;
	
	private void OnTriggerEnter(Collider other) {
		other.GetComponent<CharacterAbilities>().ReceiveAbility(GenerateSpell());
		spawner.StartCoroutine(spawner.RespawnAfterDelay());
		Destroy(this.gameObject);
	}
	
	Spell GenerateSpell() {
		int i = Random.Range(0, 2);
		
		switch(i) {
			case 0:
				return new SuperKnockback();
			case 1:
				return new BeastHaste();
		}
		
		return null;
	}
	
	public void AssignSpawner(Spawner newSpawner) {
		spawner = newSpawner;
	}
}
