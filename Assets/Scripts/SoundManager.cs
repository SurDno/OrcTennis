using UnityEngine;

public static class SoundManager {
	static AudioSource player;
	
	// When passed a sound, play it instantly.
	public static void PlaySound(string soundName) {
		if(!player)
			CreatePlayer();
		
		player.PlayOneShot(GetClipByName(soundName));
	}
	
	// When passed an array, select one sound from it and play it.
	public static void PlaySound(string[] soundNames) {
		if(!player)
			CreatePlayer();
		
		player.PlayOneShot(GetClipByName(soundNames[Random.Range(0, soundNames.Length)]));
	}
	
	// When passed a sound with volume, play it instantly with given volume.
	public static void PlaySound(string soundName, float volume) {
		if(!player)
			CreatePlayer();
		
		player.PlayOneShot(GetClipByName(soundName), volume);
	}
	
	// When passed an array with volume, select one sound from it and play it with given volume.
	public static void PlaySound(string[] soundNames, float volume) {
		if(!player)
			CreatePlayer();
		
		player.PlayOneShot(GetClipByName(soundNames[Random.Range(0, soundNames.Length)]), volume);
	}
	
	
	static AudioClip GetClipByName(string soundName) {
		return Resources.Load<AudioClip>("Audio/SFX/" + soundName);
	}
	
	static void CreatePlayer() {
		player = new GameObject().AddComponent<AudioSource>();
		player.gameObject.name = "AudioPlayer";
		Object.DontDestroyOnLoad(player);
	}
}
