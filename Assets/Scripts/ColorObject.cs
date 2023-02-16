using UnityEngine;

// Simply provides easier way to color objects without actually creating new materials in assets.
public class ColorObject : MonoBehaviour {
	public Color objColor;
	
    void Update() {
        GetComponent<Renderer>().material.color = objColor;
    }
}
