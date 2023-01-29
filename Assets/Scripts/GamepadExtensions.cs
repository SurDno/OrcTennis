using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

// Holds some extension functions for gamepads that allow to more easily get control values, and prevents error messages if gamepad gets disconnected.
public class GamepadExtensions {
	// Gets input values of left stick.
	public static Vector2 GetLeftStick(Gamepad gamepadInstance) {
		return GamepadExtensions.IsGamepadConnected(gamepadInstance) ? new Vector2(gamepadInstance.leftStick.x.ReadValue(), gamepadInstance.leftStick.y.ReadValue()) : Vector2.zero;
	}
	
	// Gets whether the left stick is currently pressed.
	public static bool IsLeftStickPressed(Gamepad gamepadInstance) {
		return GamepadExtensions.IsGamepadConnected(gamepadInstance) ? gamepadInstance.leftStickButton.isPressed : false;
	}

	// Gets input values of right stick.
	public static Vector2 GetRightStick(Gamepad gamepadInstance) {
		return GamepadExtensions.IsGamepadConnected(gamepadInstance) ? new Vector2(gamepadInstance.rightStick.x.ReadValue(), gamepadInstance.rightStick.y.ReadValue()) : Vector2.zero;
	}
	
	// Gets whether the right stick is currently pressed.
	public static bool IsRightStickPressed(Gamepad gamepadInstance) {
		return GamepadExtensions.IsGamepadConnected(gamepadInstance) ? gamepadInstance.rightStickButton.isPressed : false;
	}
	
	// Checks if the device is connected by checking if it shares the id with any of the connected devices.
	public static bool IsGamepadConnected(Gamepad gamepadInstance) {
		return InputSystem.devices.Any(device => device.deviceId == gamepadInstance.deviceId);
	}
}