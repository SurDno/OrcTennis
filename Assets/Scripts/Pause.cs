using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
	[Header("Prefabs and Cached Objects")]
	[SerializeField]private GameObject pauseObject;
	[SerializeField]private Image initiatorImage;
	[SerializeField]private Text countdownText;
	
	[Header("Current Values")]
	private bool paused;
	private bool noReturn;
	private Player initiator;
	private Coroutine countdownCoroutine;
	
    void Update() {
		// If we passed the point of no return, return. :D
		if(noReturn)
			return;
		
		// If we're not paused currently, switch between players to find if someone wants to pause.
        if(!paused) {
		
			// Get active players for current frame.
			Player[] players = PlayerHolder.GetPlayers();
		
			foreach(Player player in players) {
				if(GamepadInput.GetLeftShoulder(player.GetGamepad()) && GamepadInput.GetRightShoulder(player.GetGamepad())) {
					paused = true;
					initiator = player;
					Time.timeScale = 0;
					pauseObject.SetActive(true);
					initiatorImage.color = player.GetColor();
					countdownCoroutine = StartCoroutine(Countdown());
					return;
				}
				
			}
		// If we're paused, see if the initiator unpauses.
		} else {
			if(!GamepadInput.GetLeftShoulder(initiator.GetGamepad()) || !GamepadInput.GetRightShoulder(initiator.GetGamepad())) {
				paused = false;
				Time.timeScale = 1;
				pauseObject.SetActive(false);
				StopCoroutine(countdownCoroutine);
			}
		}
    }
	
	IEnumerator Countdown() {
		float timer = 5.5f;
		while(timer > 0) {
			timer -= Time.unscaledDeltaTime;
			countdownText.text = "Going back to Main Menu in " + (int)timer  + "...";
			yield return null;
		}
		
		noReturn = true;
		Time.timeScale = 1;
		CoroutinePlayer.StartCoroutine(GameController.ReturnToMenu());
	}
}
