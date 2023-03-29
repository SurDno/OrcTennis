using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Allows instantly playing a sound by parsing just its name in the files. Can specify volume.
// Also allows playing looping audio and music.
public static class SoundManager {
	static AudioSource soundPlayer;
	static AudioSource musicPlayer;
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
	
	// Plays a sound once, then repeatedly again after delay.
	public static void PlaySoundLooped(string soundName, float volume, float delay) {
		// If this sound is already being played, ignore call.
		if (coroutineBySoundName.ContainsKey(soundName)) 
			return;
		
		AudioClip clip = GetClipByName(soundName);
		coroutineBySoundName[soundName] = CoroutinePlayer.StartCoroutine(PrepareForPlayLooped(clip, volume, delay));
	}
	
	// Stop sound from repeating further, but does NOT abruptly stop the currently playing sound.
	public static void StopSoundLooped(string soundName) {
		// If this sound is not being played, ignore call.
		if (!coroutineBySoundName.ContainsKey(soundName)) 
			return;

		CoroutinePlayer.StopCoroutine(coroutineBySoundName[soundName]);
	}
	
	public static void PlayMusic(string soundName, float volume, bool loop) {
		if(!musicPlayer)
			musicPlayer = CreatePlayer(true);
		
		AudioClip clip = GetClipByName(soundName);

		musicPlayer.Stop();
		musicPlayer.volume = volume;
		musicPlayer.clip = clip;
		musicPlayer.loop = loop;
		musicPlayer.Play();
	}
	
	public static void StopMusic() {
		musicPlayer?.Stop();
	}
	
	static void PrepareForPlay(AudioClip clip, float volume) {
		if(!soundPlayer)
			soundPlayer = CreatePlayer(false);
		
		soundPlayer.PlayOneShot(clip, volume);
	}
	
	static IEnumerator PrepareForPlayLooped(AudioClip clip, float volume, float delay) {
		if(!soundPlayer)
			soundPlayer = CreatePlayer(false);
		
		while(true) {
			soundPlayer.PlayOneShot(clip, volume);
			yield return new WaitForSeconds(clip.length);
			yield return new WaitForSeconds(delay);
		}
	}
	
	public static float GetClipLength(string soundName) {
		AudioClip clip = GetClipByName(soundName);
		
		return clip.length;
	}
	
	static AudioClip GetClipByName(string soundName) {
		// First search in SFX folder. 
		AudioClip potentialSound = Resources.Load<AudioClip>("Audio/SFX/" + soundName);
		
		// If not found, look in Music folder.
		if(potentialSound == null)
			potentialSound = Resources.Load<AudioClip>("Audio/Music/" + soundName);
		
		return potentialSound;
	}
	
	static AudioSource CreatePlayer(bool forMusic) {
		AudioSource instance = new GameObject().AddComponent<AudioSource>();
		instance.gameObject.name = "AudioPlayer (" + (forMusic ? "Music" : "SFX") + ")";
		instance.loop = forMusic;
		Object.DontDestroyOnLoad(instance.gameObject);
		return instance;
	}
}
