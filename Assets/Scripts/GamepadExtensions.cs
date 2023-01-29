using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

// Holds some extension functions for gamepads that allow to more easily get control values.
public class GamepadExtensions {
	// Gets input values of left stick.
	public static Vector2 GetLeftStick(Gamepad gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance))
			return new Vector2(gamepadInstance.leftStick.x.ReadValue(), gamepadInstance.leftStick.y.ReadValue());
		else
			return Vector2.zero;
	}

	// Gets input values of right stick.
	public static Vector2 GetRightStick(Gamepad gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance))
			return new Vector2(gamepadInstance.rightStick.x.ReadValue(), gamepadInstance.rightStick.y.ReadValue());
		else
			return Vector2.zero;
	}
	
	// Checks if the device is connected by checking if it shares the id with any of the connected devices.
	public static bool IsGamepadConnected(Gamepad gamepadInstance) {
		return InputSystem.devices.Any(device => device.deviceId == gamepadInstance.deviceId);
	}
}

