using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Hold core methods for switching between scenes and accesing information about current game state.
public static class GameController {
    public enum GameState {Setup, Match};


    // Info
    private static GameState currentState = GameState.Setup;
	
	public static IEnumerator LoadMatch() {
		CoroutinePlayer.StartCoroutine(SceneFader.FadeOut(0.5f));

		while(SceneFader.GetFadeInStatus() != SceneFader.FadeInStatus.Faded)
			yield return null;
		
		foreach (Player player in PlayerHolder.GetPlayers())
            player.GetCursor().HideCursor();

		// Load specific level environment first to apply lighting correctly.
		currentState = GameState.Match;
        UnityEngine.AsyncOperation asyncLoadOne = SceneManager.LoadSceneAsync(MatchSettings.GetMap().ToString(),  LoadSceneMode.Single);

        // Load core scene for gameplay additively.
        UnityEngine.AsyncOperation asyncLoadTwo = SceneManager.LoadSceneAsync("GameCore",  LoadSceneMode.Additive);

        while (asyncLoadOne.progress < 0.9f || asyncLoadTwo.progress < 0.9f)
            yield return null;

        // Wait more after the scene is loaded for nicer transition on better PCs.
        yield return new WaitForSeconds(0.5f);

		MatchController.Start();

        // Start music.
        SoundManager.PlayMusic("Match", 0.3f, true);

        CoroutinePlayer.StartCoroutine(SceneFader.FadeIn(0.5f));

    }

    // Resets all game info and goes back to game menu.
    public static IEnumerator ReturnToMenu() {
		CoroutinePlayer.StartCoroutine(SceneFader.FadeOut(0.5f));

        while (SceneFader.GetFadeInStatus() != SceneFader.FadeInStatus.Faded)
            yield return null;

        foreach (Player player in PlayerHolder.GetPlayers()) {
            player.GetCursor().ShowCursor();
            player.ResetPlayerData();
        }

        MatchController.ResetScoreData();
        PlayerSetup.ResetSetupData();

        currentState = GameState.Setup;
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Setup", LoadSceneMode.Single);

        while (asyncLoad.progress < 0.9f)
            yield return null;

        // Wait more after the scene is loaded for nicer transition on better PCs.
        yield return new WaitForSeconds(0.5f);

        // Stop music.
        SoundManager.StopMusic();

        CoroutinePlayer.StartCoroutine(SceneFader.FadeIn(0.5f));
    }
	
	public static GameState GetGameState() {
		return currentState;
	}
}
