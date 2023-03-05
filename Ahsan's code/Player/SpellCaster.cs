using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public GameObject spellPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject spell = Instantiate(spellPrefab);
            spell.transform.position = transform.position + transform.forward;
            spell.transform.forward = transform.forward;
        }
    }
}
