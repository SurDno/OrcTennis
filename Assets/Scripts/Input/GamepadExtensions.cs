using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;
using System.Collections.Generic;

// Holds some extension functions for gamepads that allow to more easily get necessary control values and whether gamepad is connected.
// Also allows to use devices not labeled as gamepads (such as DualShock controllers that are registered as joysticks) as gamepads by accessing properties on a lower level.
public static class GamepadExtensions {
	// A dictionary of dictionaries to get a dictionary of input values by device ID, and then last frame value by button name.
	public static Dictionary<int, Dictionary<string, bool>> lastFramePresses = new Dictionary<int, Dictionary<string, bool>>();
	
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
		bool currentlyPressed = GetButton(gamepadInstance, gamepadName, joystickName);
		bool pressedBefore;
		if(lastFramePresses.ContainsKey(gamepadInstance.deviceId) && lastFramePresses[gamepadInstance.deviceId].ContainsKey(gamepadName))
			pressedBefore = lastFramePresses[gamepadInstance.deviceId][gamepadName];
		else
			pressedBefore = false;
		
		if(!lastFramePresses.ContainsKey(gamepadInstance.deviceId))
			lastFramePresses[gamepadInstance.deviceId] = new Dictionary<string, bool>();
		lastFramePresses[gamepadInstance.deviceId][gamepadName] = currentlyPressed;
		
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
	
	// Checks if the device is connected by checking if it shares the id with any of the connected devices.
	public static bool IsGamepadConnected(InputDevice gamepadInstance) {
		return gamepadInstance == null ? false : InputSystem.devices.Any(device => device.deviceId == gamepadInstance.deviceId);
	}
}