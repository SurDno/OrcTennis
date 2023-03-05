using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int collectedMagicOrbs = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MagicOrb"))
        {
            collectedMagicOrbs++;
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MagicOrb") && Input.GetKeyDown(KeyCode.Space))
        {
            MagicOrb magicOrb = other.gameObject.GetComponent<MagicOrb>();
            if (magicOrb != null && !magicOrb.collected)
            {
                magicOrb.collected = true;
                collectedMagicOrbs++;
                Debug.Log("Collected Magic Orb! Total: " + collectedMagicOrbs);
                Destroy(other.gameObject);
            }
        }
    }

    private class MagicOrb
    {
        public bool collected { get; internal set; }
    }
}
