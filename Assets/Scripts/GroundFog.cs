using UnityEngine;

public class GroundFog : MonoBehaviour
{
    public Color fogColor = Color.gray; // Set the color of the fog
    public float density = 0.02f; // Set the density of the fog
    public float startDistance = 0.1f; // Set the start distance of the fog
    public float endDistance = 20f; // Set the end distance of the fog

    private void Update()
    {
        RenderSettings.fog = true; // Enable fog
        RenderSettings.fogColor = fogColor; // Set fog color
        RenderSettings.fogDensity = density; // Set fog density
        RenderSettings.fogMode = FogMode.Linear; // Set fog mode to linear
        RenderSettings.fogStartDistance = startDistance; // Set fog start distance
        RenderSettings.fogEndDistance = endDistance; // Set fog end distance
    }
}
