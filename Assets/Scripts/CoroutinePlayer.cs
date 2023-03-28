using System.Collections;
using UnityEngine;

// Allows static classes to play their coroutines by creating a MonoBehaviour dummy instance that runs them.
public static class CoroutinePlayer {
	static CoroutineDummy player;

	public static Coroutine StartCoroutine(IEnumerator givenCoroutine) {
		if(!player)
			CreatePlayer();
		
		return player.StartCoroutine(givenCoroutine);
	}
	
	public static void StopCoroutine(Coroutine givenCoroutine) {
		if(givenCoroutine == null)
			return; 
		
		if(!player)
			CreatePlayer();
		
		player.StopCoroutine(givenCoroutine);
	}
	
	static void CreatePlayer() {
		player = new GameObject().AddComponent<CoroutineDummy>();
		player.gameObject.name = "StaticCoroutinePlayer";
		Object.DontDestroyOnLoad(player);
	}
}
