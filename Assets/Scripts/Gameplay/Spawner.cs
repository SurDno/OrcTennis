using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	[Header("Settings")]
	[SerializeField] private GameObject objectToSpawn;
    private float defaultSpawnDelay = 10f;
	
    void Start () {
        SpawnObject();
    }

    void SpawnObject () {
		// Find a random point within collider bounds.
		Collider collider = GetComponent<Collider>();
		Vector3 randomPoint = new Vector3(
			Random.Range(collider.bounds.min.x, collider.bounds.max.x),
			-3f,
			Random.Range(collider.bounds.min.z, collider.bounds.max.z)
		);

        GameObject newInst = Instantiate(objectToSpawn, randomPoint, Quaternion.identity);
		newInst.GetComponent<CollectibleObject>().AssignSpawner(this);
    }
	
	public IEnumerator RespawnAfterDelay() {
		yield return new WaitForSeconds(CalculateSpawnDelay());
		
		SpawnObject();
	}
	
	public float CalculateSpawnDelay() {
		return defaultSpawnDelay * 2 / PlayerHolder.GetPlayers().Length;
	}
}
