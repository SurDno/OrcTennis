using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using System.Collections.Generic;

// Holds some extension functions for gamepads that allow to more easily get necessary control values and whether gamepad is connected.
// Also allows to use devices not labeled as gamepads (such as DualShock controllers that are registered as joysticks) as gamepads by accessing properties on a lower level.
public static class GamepadExtensions {
	// A dictionary of dictionaries to get a dictionary of input values by device ID, and then current frame value by button name, last frame value by button name and frame stamp to check if data is up-to-date.
	static Dictionary<int, Dictionary<string, bool>> currentFramePresses = new Dictionary<int, Dictionary<string, bool>>();
	static Dictionary<int, Dictionary<string, bool>> lastFramePresses = new Dictionary<int, Dictionary<string, bool>>();
	static Dictionary<int, Dictionary<string, int>> frameCountForCurrent = new Dictionary<int, Dictionary<string, int>>();
	
	// A value under which input from sticks may be ignored.
	const float deadzone = 0.05f;
	
	// Common function for getting buttons.
	public static bool GetButton(InputDevice gamepadInstance, string gamepadName, string joystickName) {
		if (!IsGamepadConnected(gamepadInstance))
			return false;

		switch (gamepadInstance) {
			case Gamepad gamepad:
				return gamepad.GetChildControl<ButtonControl>(gamepadName).isPressed;
			case Joystick joystick:
				return joystick.GetChildControl<ButtonControl>(joystickName).isPressed;
			default:
				return false;
		}
	}
	
	// Gets whether the button is currently pressed but was not pressed before or is currently not pressed but was pressed before.
	public static bool GetButtonChange(InputDevice gamepadInstance, string gamepadName, string joystickName, bool onDown) {
		if (!IsGamepadConnected(gamepadInstance))
			return false;
		
		// Check if there already are entires for the current frame of that key.
		if (frameCountForCurrent.ContainsKey(gamepadInstance.deviceId) &&
			frameCountForCurrent[gamepadInstance.deviceId].ContainsKey(gamepadName)) {
			
			// Check if our current frame data for that key is outdated.
			if(frameCountForCurrent[gamepadInstance.deviceId][gamepadName] != Time.frameCount) {
				// Move presses to last frame dictionary.
				lastFramePresses[gamepadInstance.deviceId][gamepadName] = currentFramePresses[gamepadInstance.deviceId][gamepadName];
				
				// Populate dictionaries with current presses and frame amounts.
				currentFramePresses[gamepadInstance.deviceId][gamepadName] = GetButton(gamepadInstance, gamepadName, joystickName);
				frameCountForCurrent[gamepadInstance.deviceId][gamepadName] = Time.frameCount;
			}
			
		} else {
			// If there is no information for that gamepad, populate that data for the first time.
			if(!currentFramePresses.ContainsKey(gamepadInstance.deviceId))
				currentFramePresses[gamepadInstance.deviceId] = new Dictionary<string, bool>();
			if(!frameCountForCurrent.ContainsKey(gamepadInstance.deviceId))
				frameCountForCurrent[gamepadInstance.deviceId] = new Dictionary<string, int>();
			if(!lastFramePresses.ContainsKey(gamepadInstance.deviceId))
				lastFramePresses[gamepadInstance.deviceId] = new Dictionary<string, bool>();
			
			// If there is no information for that gamepad key, populate that data for the first time.
			currentFramePresses[gamepadInstance.deviceId][gamepadName] = GetButton(gamepadInstance, gamepadName, joystickName);
			frameCountForCurrent[gamepadInstance.deviceId][gamepadName] = Time.frameCount;
			lastFramePresses[gamepadInstance.deviceId][gamepadName] = false;
		}
		
		bool currentlyPressed = currentFramePresses[gamepadInstance.deviceId][gamepadName];
		bool pressedBefore = lastFramePresses[gamepadInstance.deviceId][gamepadName];
		
		if(onDown)
			return currentlyPressed && !pressedBefore;
		else
			return !currentlyPressed && pressedBefore;
	}
	
	// Common function for getting axes (from sticks / d-pad).
	public static Vector2 GetAxes(InputDevice gamepadInstance, string gamepadNameX, string gamepadNameY, string joystickNameX, string joystickNameY, bool joystickReverseY) {
		if (!IsGamepadConnected(gamepadInstance))
			return Vector2.zero;
		
		switch (gamepadInstance) {
			case Gamepad gamepad:
				return new Vector2(gamepad.GetChildControl<AxisControl>(gamepadNameX).ReadValue(), gamepad.GetChildControl<AxisControl>(gamepadNameY).ReadValue());
			case Joystick joystick:
				if(!joystickReverseY)
					return new Vector2(joystick.GetChildControl<AxisControl>(joystickNameX).ReadValue(), joystick.GetChildControl<AxisControl>(joystickNameY).ReadValue());
				else
					return new Vector2(joystick.GetChildControl<AxisControl>(joystickNameX).ReadValue(), -joystick.GetChildControl<AxisControl>(joystickNameY).ReadValue());
			default:
				return new Vector2(0, 0);
		}
	}
	
	// Checks if the input values are higher than deadzone constant. If they are not, usually input can be ignored.
	public static bool InputMoreThanDeadzone(Vector2 normalizedVector) {
		return Mathf.Abs(normalizedVector.x) >= deadzone || Mathf.Abs(normalizedVector.y) >= deadzone;
	}
	
	// Checks if the device is connected by checking if it shares the id with any of the connected devices.
	public static bool IsGamepadConnected(InputDevice gamepadInstance) {
		return gamepadInstance == null ? false : InputSystem.devices.Any(device => device.deviceId == gamepadInstance.deviceId);
	}
}