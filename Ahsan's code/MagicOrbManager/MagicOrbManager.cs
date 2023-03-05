using UnityEngine;


namespace MagicOrbManager
{


    public class MagicOrbManager : MonoBehaviour
    {
        public GameObject magicOrbPrefab;
        public int numOfMagicOrbs = 10;
        public float magicOrbRespawnTime = 30f;
        public float magicOrbDisappearTime = 10f;

        private int magicOrbCount = 0;
        private float lastMagicOrbSpawnTime = -Mathf.Infinity;

        void Start()
        {
            SpawnMagicOrb();
        }

        void Update()
        {
            if (magicOrbCount < numOfMagicOrbs && Time.time - lastMagicOrbSpawnTime >= magicOrbRespawnTime)
            {
                SpawnMagicOrb();
            }
        }

        void SpawnMagicOrb()
        {
            GameObject magicOrb = Instantiate(magicOrbPrefab);
            magicOrb.transform.position = new Vector3(Random.Range(-10f, 10f), 0.5f, Random.Range(-10f, 10f));


            Destroy(magicOrb, magicOrbDisappearTime);

            magicOrbCount++;
            lastMagicOrbSpawnTime = Time.time;

            Debug.Log("Magic orb count: " + magicOrbCount);
        }

        public void CollectMagicOrb()
        {
            magicOrbCount--;
            Debug.Log("Magic orb collected! Count: " + magicOrbCount);
        }
    }
}
