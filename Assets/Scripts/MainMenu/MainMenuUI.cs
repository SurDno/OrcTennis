using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quit!");
	}

	public void StartGame()
    {
        SceneManager.LoadScene("Setup");
    }

	public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
