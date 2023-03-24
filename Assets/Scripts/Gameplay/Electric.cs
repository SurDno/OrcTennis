using UnityEngine;

// Extra code for ElectricShield abilitiy
public class Electric : MonoBehaviour {

    void OnCollisionEnter(Collision col) {
        col.gameObject.GetComponent<Ball>()?.AddSpeed(2);
		
		SoundManager.PlaySound("ElectricShieldHit");
		
		// Create an effect on top of the collision point.
		GameObject magicEffectPrefab = Resources.Load<GameObject>("Prefabs/Magic/ElectricityHit");
		Vector3 effectPosition = col.gameObject.transform.position;
		GameObject instance = Object.Instantiate(magicEffectPrefab, effectPosition, Quaternion.identity);
		
		this.gameObject.SetActive(false);
    }
}
