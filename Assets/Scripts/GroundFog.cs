using UnityEngine;

public class GroundFog : MonoBehaviour {
    [SerializeField]private Color fogColor = Color.gray; 
    [SerializeField]private float density = 0.02f; 
    [SerializeField]private float startDistance = 0.1f; 
    [SerializeField]private float endDistance = 20f; 
	[SerializeField]private FogMode fogMode = FogMode.Linear;

	// Enable fog.
	private void Start() {
        RenderSettings.fog = true; 
		// In build, all fog settings are applied on Scene start.
		#if UNITY_STANDALONE
        RenderSettings.fogColor = fogColor; 
        RenderSettings.fogDensity = density;
        RenderSettings.fogMode = fogMode; 
        RenderSettings.fogStartDistance = startDistance; 
        RenderSettings.fogEndDistance = endDistance; 
		#endif
	}
	
    private void Update() {
		// In editor, fog settings are applied every frame for easier tweaking during play.
		#if UNITY_EDITOR
        RenderSettings.fogColor = fogColor; 
        RenderSettings.fogDensity = density;
        RenderSettings.fogMode = fogMode; 
        RenderSettings.fogStartDistance = startDistance; 
        RenderSettings.fogEndDistance = endDistance; 
		#endif
    }
}
