using UnityEngine;

// Controls the general flow of the match. Handles restarting, scoring, winning conditions, etc.
public static class MatchController {
	
    public static void Restart() {
        // Get all players to get back to their spawn points.
        foreach (Object player in Object.FindObjectsOfType(typeof(CharacterControls)))
            ((CharacterControls)player).Respawn();

        // Remove electric shield if it's present.
        GameObject.Find("BallWall")?.gameObject.SetActive(false);
		
        // Reset the ball.
        ((Ball)Object.FindObjectOfType(typeof(Ball))).ResetBall();
    }
}
