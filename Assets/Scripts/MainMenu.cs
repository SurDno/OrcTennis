using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public GameObject Camera1;
	public GameObject Camera2;
	public GameObject cameraMoving;
	public float cameraLerpValue;
	public bool setupMenuEnabled;
	public GameObject SetupGameObject;
	public GameObject MainMenuGameObject;
	
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		// lerp camera between two positions
        if(cameraLerpValue < 1 && setupMenuEnabled) 
		{
			cameraLerpValue += Time.deltaTime;
		}
		
		if(cameraLerpValue > 0 && ! setupMenuEnabled)
		{
			cameraLerpValue -= Time.deltaTime;
		}
		
		if(cameraLerpValue > 1) {
			cameraLerpValue = 1;
		}
		
		if(cameraLerpValue < 0) {
			cameraLerpValue = 0;
		}
		
		Vector3 positionOfCamera = Vector3.Lerp(Camera1.transform.position, Camera2.transform.position, cameraLerpValue);
		Vector3 rotationOfCamera = Vector3.Lerp(Camera1.transform.eulerAngles, Camera2.transform.eulerAngles, cameraLerpValue);
		
		cameraMoving.transform.position = positionOfCamera;
		cameraMoving.transform.eulerAngles = rotationOfCamera;
		
		// enable menu we need
		SetupGameObject.SetActive(setupMenuEnabled);
		MainMenuGameObject.SetActive(!setupMenuEnabled);
		
		if(setupMenuEnabled && Input.GetKey(KeyCode.Escape))
			GoBackToMainMenu();
    }
	
	public void GoToSetup() {
		setupMenuEnabled = true;
	}
	
	public void GoBackToMainMenu() {
		setupMenuEnabled = false;
	}
}
