using UnityEngine;

public class Bloom : MonoBehaviour
{
    public float intensity = 1f; // Set the intensity of the bloom effect
    public float threshold = 1f; // Set the threshold for the bloom effect
    public int iterations = 4; // Set the number of blur iterations
    public float blurSpread = 0.6f; // Set the blur spread
    public Material blurMaterial; // Set the blur material

    private RenderTexture bloomTexture; // Render texture to hold the bloom effect

    private void Start()
    {
        // Create a new render texture for the bloom effect
        int width = Screen.width / 4;
        int height = Screen.height / 4;
        bloomTexture = new RenderTexture(width, height, 0);
        bloomTexture.filterMode = FilterMode.Bilinear;
        bloomTexture.wrapMode = TextureWrapMode.Clamp;

        // Set the bloom shader properties
        Shader.SetGlobalFloat("_BloomThreshold", threshold);
        Shader.SetGlobalFloat("_BloomIntensity", intensity);
    }

    private void OnDestroy()
    {
        // Release the render texture when the script is destroyed
        if (bloomTexture != null)
        {
            bloomTexture.Release();
            bloomTexture = null;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Render the scene to the bloom texture
        Graphics.Blit(source, bloomTexture, blurMaterial, 0);

        // Apply the blur effect to the bloom texture
        RenderTexture blurredTexture = RenderTexture.GetTemporary(bloomTexture.width, bloomTexture.height);
        for (int i = 0; i < iterations; i++)
        {
            blurMaterial.SetFloat("_BlurSize", blurSpread * i);
            Graphics.Blit(bloomTexture, blurredTexture, blurMaterial, 1);
            Graphics.Blit(blurredTexture, bloomTexture, blurMaterial, 2);
        }
        RenderTexture.ReleaseTemporary(blurredTexture);

        // Combine the bloom texture with the original scene and output to the destination
        Graphics.Blit(source, destination);
        Graphics.Blit(bloomTexture, destination, blurMaterial, 3);
    }
}
