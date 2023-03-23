using UnityEngine;

// Extra code for ElectricShield abilitiy
public class Electric : MonoBehaviour {

    void OnCollisionEnter(Collision col) {
        col.gameObject.GetComponent<Ball>()?.AddSpeed(2);
		
		SoundManager.PlaySound("ElectricShieldHit");
		
		this.gameObject.SetActive(false);
    }
}
