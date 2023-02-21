using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;

// Holds some extension functions for gamepads that allow to more easily get control values, and prevents error messages if gamepad gets disconnected.
// Also allows to use devices not labeled as gamepads (such as DualShock controllers that are registered as joysticks) as gamepads by accessing properties on a lower level.
public static class GamepadExtensions {
	
	// Gets input values of left stick.
	public static Vector2 GetLeftStick(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return new Vector2(((Gamepad)gamepadInstance).leftStick.x.ReadValue(), ((Gamepad)gamepadInstance).leftStick.y.ReadValue());
			else
				return new Vector2(((Joystick)gamepadInstance).GetChildControl<StickControl>("stick").x.ReadValue(), ((Joystick)gamepadInstance).GetChildControl<StickControl>("stick").y.ReadValue());
		} else
			return Vector2.zero;
	}

	// Gets input values of right stick.
	public static Vector2 GetRightStick(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return new Vector2(((Gamepad)gamepadInstance).rightStick.x.ReadValue(), ((Gamepad)gamepadInstance).rightStick.y.ReadValue());
			else
				// For reasons unknown to me, "rz" returns -1 when the stick is in top position and 1 when it's in bottom position, so we need to multuply Y value by -1
				return new Vector2(((Joystick)gamepadInstance).GetChildControl<AxisControl>("z").ReadValue(), -((Joystick)gamepadInstance).GetChildControl<AxisControl>("rz").ReadValue());
		} else
			return Vector2.zero;
	}

	// Gets input values of D-Pad
	public static Vector2 GetDPad(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return new Vector2(((Gamepad)gamepadInstance).dpad.x.ReadValue(), ((Gamepad)gamepadInstance).dpad.y.ReadValue());
			else
				return new Vector2(((Joystick)gamepadInstance).GetChildControl<DpadControl>("hat").x.ReadValue(), ((Joystick)gamepadInstance).GetChildControl<DpadControl>("hat").y.ReadValue());
		} else
			return Vector2.zero;
	}
	
	// Gets whether the left stick is currently pressed.
	public static bool GetLeftStickButton(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).leftStickButton.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button11").isPressed;
		} else
			return false;
	}
	
	// Gets whether the right stick is currently pressed.
	public static bool GetRightStickButton(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).rightStickButton.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button12").isPressed;
		} else
			return false;
	}
	
	// Gets whether the west button (X on XBox, Square on PS) is currently pressed.
	public static bool GetWestButton(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).buttonWest.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button3").isPressed;
		} else
			return false;
	}
	
	
	// Gets whether the north button (Y on XBox, Triangle on PS) is currently pressed.
	public static bool GetNorthButton(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).buttonNorth.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button4").isPressed;
		} else
			return false;
	}
	
	
	// Gets whether the south button (A on XBox, Cross on PS) is currently pressed.
	public static bool GetSouthButton(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).buttonSouth.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button2").isPressed;
		} else
			return false;
	}
	
	
	// Gets whether the east button (B on XBox, Circle on PS) is currently pressed.
	public static bool GetEastButton(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).buttonEast.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("trigger").isPressed;
		} else
			return false;
	}
	
	
	// Gets whether the left trigger is currently pressed.
	public static bool GetLeftTrigger(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).leftTrigger.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button7").isPressed;
		} else
			return false;
	}
	
	
	// Gets whether the right trigger is currently pressed.
	public static bool GetRightTrigger(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).rightTrigger.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button8").isPressed;
		} else
			return false;
	}
	
	// Gets whether the left shoulder is currently pressed.
	public static bool GetLeftShoulder(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).leftShoulder.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button5").isPressed;
		} else
			return false;
	}
	
	
	// Gets whether the right shoulder is currently pressed.
	public static bool GetRightShoulder(InputDevice gamepadInstance) {
		if(GamepadExtensions.IsGamepadConnected(gamepadInstance)) {
			if(gamepadInstance is Gamepad)
				return ((Gamepad)gamepadInstance).rightShoulder.isPressed;
			else
				return ((Joystick)gamepadInstance).GetChildControl<ButtonControl>("button6").isPressed;
		} else
			return false;
	}
	
	// Checks if the device is connected by checking if it shares the id with any of the connected devices.
	public static bool IsGamepadConnected(InputDevice gamepadInstance) {
		return gamepadInstance == null ? false : InputSystem.devices.Any(device => device.deviceId == gamepadInstance.deviceId);
	}
}