using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

// Holds some extension functions for gamepads that allow to more easily get control values, and prevents error messages if gamepad gets disconnected.
// Also allows to use devices not labeled as gamepads (such as DualShock controllers that are registered as joysticks) as gamepads.
// Note: Joystick support requires InputSystem library changes and won't work without them.
public class GamepadExtensions {
	// Gets input values of left stick.
	public static Vector2 GetLeftStick(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return new Vector2(((Gamepad)gamepadInstance).leftStick.x.ReadValue(), ((Gamepad)gamepadInstance).leftStick.y.ReadValue());
			else
				return new Vector2(((Joystick)gamepadInstance).leftStick.x.ReadValue(), ((Joystick)gamepadInstance).leftStick.y.ReadValue());
		} else
			return Vector2.zero;
	}
	
	// Gets whether the left stick is currently pressed.
	public static bool IsLeftStickPressed(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).leftStickButton.isPressed;
			else
				return ((Joystick)gamepadInstance).leftStickButton.isPressed;
		} else
			return false;
	}

	// Gets input values of right stick.
	public static Vector2 GetRightStick(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return new Vector2(((Gamepad)gamepadInstance).rightStick.x.ReadValue(), ((Gamepad)gamepadInstance).rightStick.y.ReadValue());
			else
				return new Vector2(((Joystick)gamepadInstance).rightStickX.ReadValue(), -((Joystick)gamepadInstance).reversedRightStickY.ReadValue());
		} else
			return Vector2.zero;
	}
	
	// Gets whether the right stick is currently pressed.
	public static bool IsRightStickPressed(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).rightStickButton.isPressed;
			else
				return ((Joystick)gamepadInstance).rightStickButton.isPressed;
		} else
			return false;
	}
	
	// Checks if the device is connected by checking if it shares the id with any of the connected devices.
	public static bool IsGamepadConnected(InputDevice gamepadInstance) {
		return InputSystem.devices.Any(device => device.deviceId == gamepadInstance.deviceId);
	}
}