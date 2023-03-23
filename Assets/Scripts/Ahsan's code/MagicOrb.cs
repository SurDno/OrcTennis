using UnityEngine;


public class MagicOrb : MonoBehaviour
{
    public GameObject magicOrbPrefab; // prefab of the magic orb game object
    public int numberOfOrbsToSpawn; // number of orbs to spawn
    public float spawnRadius; // radius of the spawn area
    public float spawnHeight; // height of the spawn area
    private int teamColor;
    void Start()
    {
        for (int i = 0; i < numberOfOrbsToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), spawnHeight, Random.Range(-spawnRadius, spawnRadius)); // randomize spawn position within a radius
            GameObject magicOrb = Instantiate(magicOrbPrefab, spawnPosition, Quaternion.identity); // instantiate magic orb prefab at spawn position
            int teamColor = Random.Range(1, 7); // choose a random player id between 1 and 6
            magicOrb.GetComponent<MagicOrb>().SetPlayerId(teamColor); // set player id for magic orb
        }
    }

    // i can't fix this part 
    public void SetPlayerId(int id)
    {
        teamColor = id;
        GetComponent<Renderer>().material.color = teamColor == 6 ? Color.red : Color.green; // set color based on player id
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 50); // make magic orb spin
    }
}