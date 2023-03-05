using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public GameObject magicOrbPrefab;
    public Transform[] magicOrbRespawnPoints;
    public float magicOrbRespawnTime = 30f;
    public KeyCode fireSpellKey = KeyCode.F;

    private Rigidbody rb;
    private bool hasMagicOrb = false;
    private GameObject magicOrbInstance;
    private float lastMagicOrbCollectedTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        rb.AddForce(movement * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(fireSpellKey) && hasMagicOrb)
        {
            // TODO: Implement spell-casting logic
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MagicOrb"))
        {
            
            Destroy(other.gameObject);
            hasMagicOrb = true;

            Invoke("SpawnMagicOrb", magicOrbRespawnTime);

            lastMagicOrbCollectedTime = Time.time;
        }
    }

    void SpawnMagicOrb()
    {
        int respawnIndex = Random.Range(0, magicOrbRespawnPoints.Length);
        Vector3 respawnPosition = magicOrbRespawnPoints[respawnIndex].position;

        magicOrbInstance = Instantiate(magicOrbPrefab, respawnPosition, Quaternion.identity);

        hasMagicOrb = false;
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                return true;
            }
        }
        return false;
    }
}
