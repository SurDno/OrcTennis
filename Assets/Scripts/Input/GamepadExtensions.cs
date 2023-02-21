using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;

// Holds some extension functions for gamepads that allow to more easily get control values, and prevents error messages if gamepad gets disconnected.
// Also allows to use devices not labeled as gamepads (such as DualShock controllers that are registered as joysticks) as gamepads by accessing properties on a lower level.
public static class GamepadExtensions {
	
	// Gets input values of left stick.
	public static Vector2 GetLeftStick(InputDevice gamepadInstance) {
		return GetAxes(gamepadInstance, "leftStick/x", "leftStick/y", "stick/x", "stick/y", false);
	}

	// Gets input values of right stick.
	public static Vector2 GetRightStick(InputDevice gamepadInstance) {
		// For reasons beyond me, "rz" returns -1 when the stick is in top position and 1 when it's in bottom position.
		// So for that specific scenario we'll need to multiply value by -1.
		return GetAxes(gamepadInstance, "leftStick/x", "leftStick/y", "z", "rz", true);
	}

	// Gets input values of D-Pad
	public static Vector2 GetDPad(InputDevice gamepadInstance) {
		return GetAxes(gamepadInstance, "dpad/x", "dpad/y", "hat/x", "hat/y", false);
	}
	
	// Gets whether the left stick is currently pressed.
	public static bool GetLeftStickButton(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "leftStickPress", "button11");
	}
	
	// Gets whether the right stick is currently pressed.
	public static bool GetRightStickButton(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "rightStickPress", "button12");
	}
	
	// Gets whether the west button (X on XBox, Square on PS) is currently pressed.
	public static bool GetWestButton(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "buttonWest", "button3");
	}
	
	
	// Gets whether the north button (Y on XBox, Triangle on PS) is currently pressed.
	public static bool GetNorthButton(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "buttonNorth", "button4");
	}
	
	
	// Gets whether the south button (A on XBox, Cross on PS) is currently pressed.
	public static bool GetSouthButton(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "buttonSouth", "button2");
	}
	
	
	// Gets whether the east button (B on XBox, Circle on PS) is currently pressed.
	public static bool GetEastButton(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "buttonEast", "trigger");
	}
	
	
	// Gets whether the left trigger is currently pressed.
	public static bool GetLeftTrigger(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "leftTrigger", "button7");
	}
	
	
	// Gets whether the right trigger is currently pressed.
	public static bool GetRightTrigger(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "rightTrigger", "button8");
	}
	
	// Gets whether the left shoulder is currently pressed.
	public static bool GetLeftShoulder(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "leftShoulder", "button5");
	}
	
	
	// Gets whether the right shoulder is currently pressed.
	public static bool GetRightShoulder(InputDevice gamepadInstance) {
		return GetButton(gamepadInstance, "rightShoulder", "button6");
	}
	
	// Common function for getting buttons.
	static bool GetButton(InputDevice gamepadInstance, string gamepadName, string joystickName) {
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
	
	// Common function for getting axes (from sticks / d-pad).
	static Vector2 GetAxes(InputDevice gamepadInstance, string gamepadNameX, string gamepadNameY, string joystickNameX, string joystickNameY, bool joystickReverseY) {
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