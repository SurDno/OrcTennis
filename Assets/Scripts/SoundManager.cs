using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Allows instantly playing a sound by parsing just its name in the files. Can specify volume.
// Also allows playing looping audio, such as music.
public static class SoundManager {
	static AudioSource player;
	static Dictionary<string, Coroutine> coroutineBySoundName = new Dictionary<string, Coroutine>();
	
	// When passed a sound, assume 100% volume.
	public static void PlaySound(string soundName) {
		PlaySound(soundName, 1.0f);
	}
	
	// When passed an array, assume 100% volume.
	public static void PlaySound(string[] soundNames) {
		PlaySound(soundNames, 1.0f);
	}
	
	// When passed a sound with volume, play it instantly with given volume.
	public static void PlaySound(string soundName, float volume) {
		AudioClip clip = GetClipByName(soundName);
		PrepareForPlay(clip, volume);
	}
	
	// When passed an array with volume, select one sound from it and play it with given volume.
	public static void PlaySound(string[] soundNames, float volume) {
		AudioClip clip = GetClipByName(soundNames[Random.Range(0, soundNames.Length)]);
		PrepareForPlay(clip, volume);
	}
	
	public static void PlaySoundLooped(string soundName, float volume) {
		// If this sound is already being played, ignore call.
		if (coroutineBySoundName.ContainsKey(soundName)) 
			return;
		
		AudioClip clip = GetClipByName(soundName);
		coroutineBySoundName[soundName] = CoroutinePlayer.StartCoroutine(PrepareForPlayLooped(clip, volume));
	}
	
	public static void StopSoundLooped(string soundName) {
		// If this sound is not being played, ignore call.
		if (!coroutineBySoundName.ContainsKey(soundName)) 
			return;

		CoroutinePlayer.StopCoroutine(coroutineBySoundName[soundName]);
	}
	
	static void PrepareForPlay(AudioClip clip, float volume) {
		if(!player)
			CreatePlayer();
		
		player.PlayOneShot(clip, volume);
	}
	
	static IEnumerator PrepareForPlayLooped(AudioClip clip, float volume) {
		if(!player)
			CreatePlayer();
		
		while(true) {
			player.PlayOneShot(clip, volume);
			yield return new WaitForSeconds(clip.length);
		}
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
