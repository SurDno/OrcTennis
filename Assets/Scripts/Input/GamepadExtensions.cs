using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using System.Collections.Generic;

// Holds some extension functions for gamepads that allow to more easily get necessary control values and whether gamepad is connected.
// Also allows to use devices not labeled as gamepads (such as DualShock controllers that are registered as joysticks) as gamepads by accessing properties on a lower level.
public static class GamepadExtensions {
	// A dictionary of dictionaries of tuples to get info about last frame press and current frame press from gamepad device ID and button name.
	static Dictionary<int, Dictionary<string, (bool currentFramePresses, bool lastFramePresses, int frameCountForCurrent)>> buttonStates = new Dictionary<int, Dictionary<string, (bool, bool, int)>>();
	
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
			if (buttonStates.ContainsKey(gamepadInstance.deviceId) &&
				buttonStates[gamepadInstance.deviceId].ContainsKey(gamepadName)) {
				
				// If our current frame data for that key is outdated, move currentFramePresses to lastFramePresses and populare rest of dictionary with new data.
				if(buttonStates[gamepadInstance.deviceId][gamepadName].frameCountForCurrent != Time.frameCount)
					buttonStates[gamepadInstance.deviceId][gamepadName] = (GetButton(gamepadInstance, gamepadName, joystickName), buttonStates[gamepadInstance.deviceId][gamepadName].currentFramePresses, Time.frameCount);

				
			} else {
				// If there is no information for that gamepad, populate that data for the first time.
				if(!buttonStates.ContainsKey(gamepadInstance.deviceId))
					buttonStates[gamepadInstance.deviceId] = new Dictionary<string, (bool, bool, int)>();
				
				// If there is no information for that gamepad key, populate that data for the first time.
				buttonStates[gamepadInstance.deviceId][gamepadName] = (GetButton(gamepadInstance, gamepadName, joystickName), false, Time.frameCount);
			}
			
			bool currentlyPressed = buttonStates[gamepadInstance.deviceId][gamepadName].currentFramePresses;
			bool pressedBefore = buttonStates[gamepadInstance.deviceId][gamepadName].lastFramePresses;
			
			return onDown ? (currentlyPressed && !pressedBefore) : (!currentlyPressed && pressedBefore);
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