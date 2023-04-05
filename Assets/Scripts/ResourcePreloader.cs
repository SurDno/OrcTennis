using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResourcePreloader : MonoBehaviour {
	[SerializeField]private Text loadingText;
	[SerializeField]private Image loadingImage;
	
	UnityEngine.Object[] resources;
	
    void Start() {
		UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadResources());
    }
	
	void Update() {
		if(loadingImage != null)
			loadingImage.transform.eulerAngles = new Vector3(0, 0, loadingImage.transform.eulerAngles.z - Time.deltaTime * 5);
	}

	IEnumerator LoadResources() {
		loadingText.text = "Preloading Audio...";
		yield return null;
		UnityEngine.Object[] audio = Resources.LoadAll("Audio", typeof(UnityEngine.Object));
		
		loadingText.text = "Preloading Materials...";
		yield return null;
		UnityEngine.Object[] materials = Resources.LoadAll("Materials", typeof(UnityEngine.Object));
		
		loadingText.text = "Preloading Meshes...";
		yield return null;
		UnityEngine.Object[] models = Resources.LoadAll("Models", typeof(UnityEngine.Object));
		
		loadingText.text = "Preloading Textures...";
		yield return null;
		UnityEngine.Object[] textures = Resources.LoadAll("Prefabs", typeof(UnityEngine.Object));
		
		loadingText.text = "Preloading Sprites...";
		yield return null;
		UnityEngine.Object[] sprites = Resources.LoadAll("Prefabs", typeof(UnityEngine.Object));
		
		loadingText.text = "Preloading Prefabs...";
		yield return null;
		UnityEngine.Object[] prefabs = Resources.LoadAll("Prefabs", typeof(UnityEngine.Object));
		
		loadingText.text = "Saving References...";
		yield return null;
		resources = new UnityEngine.Object[audio.Length + materials.Length + models.Length + textures.Length + sprites.Length + prefabs.Length];
		Array.Copy(audio, 0, resources, 0, audio.Length);
		Array.Copy(materials, 0, resources, audio.Length, materials.Length);
		Array.Copy(models, 0, resources, audio.Length + materials.Length, models.Length);
		Array.Copy(textures, 0, resources, audio.Length + materials.Length + models.Length, textures.Length);
		Array.Copy(sprites, 0, resources, audio.Length + materials.Length + models.Length + textures.Length, sprites.Length);
		Array.Copy(prefabs, 0, resources, audio.Length + materials.Length + models.Length + textures.Length + sprites.Length, prefabs.Length);
		
		yield return null;
		CoroutinePlayer.StartCoroutine(GameController.ReturnToMenu());
	}
}
