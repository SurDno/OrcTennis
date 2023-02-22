using UnityEngine;
using UnityEngine.InputSystem;

// Holds some extension functions that allows to easily access necessary extensions in GamepadExtensions without knowing button and axis names.
public static class GamepadInput {
	
	// VECTORS 
	
	// Gets input values of left stick.
	public static Vector2 GetLeftStick(InputDevice gamepadInstance) {
		return GamepadExtensions.GetAxes(gamepadInstance, "leftStick/x", "leftStick/y", "stick/x", "stick/y", false);
	}

	// Gets input values of right stick.
	public static Vector2 GetRightStick(InputDevice gamepadInstance) {
		// For reasons beyond me, "rz" returns -1 when the stick is in top position and 1 when it's in bottom position.
		// So for that specific scenario we'll need to multiply value by -1.
		return GamepadExtensions.GetAxes(gamepadInstance, "leftStick/x", "leftStick/y", "z", "rz", true);
	}

	// Gets input values of D-Pad
	public static Vector2 GetDPad(InputDevice gamepadInstance) {
		return GamepadExtensions.GetAxes(gamepadInstance, "dpad/x", "dpad/y", "hat/x", "hat/y", false);
	}
	
	// BUTTONS 
	
	// Gets whether the left stick is currently pressed.
	public static bool GetLeftStickButton(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "leftStickPress", "button11");
	}
	
	// Gets whether the left stick is currently pressed but was not pressed before.
	public static bool GetLeftStickButtonDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "leftStickPress", "button11", true);
	}
	
	// Gets whether the left stick is currently not pressed but was pressed before.
	public static bool GetLeftStickButtonUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "leftStickPress", "button11", false);
	}
	
	// Gets whether the right stick is currently pressed.
	public static bool GetRightStickButton(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "rightStickPress", "button12");
	}
	
	// Gets whether the right stick is currently pressed but was not pressed before.
	public static bool GetRightStickButtonDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "rightStickPress", "button12", true);
	}
	
	// Gets whether the right stick is currently not pressed but was pressed before.
	public static bool GetRightStickButtonUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "rightStickPress", "button12", false);
	}
	
	// Gets whether the west button (X on XBox, Square on PS) is currently pressed.
	public static bool GetWestButton(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "buttonWest", "button3");
	}
	
	// Gets whether the west button (X on XBox, Square on PS) is currently pressed but was not pressed before.
	public static bool GetWestButtonDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonWest", "button3", true);
	}
	
	// Gets whether the west button (X on XBox, Square on PS) is currently not pressed but was pressed before.
	public static bool GetWestButtonUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonWest", "button3", false);
	}
	
	// Gets whether the north button (Y on XBox, Triangle on PS) is currently pressed.
	public static bool GetNorthButton(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "buttonNorth", "button4");
	}
	
	// Gets whether the north button (Y on XBox, Triangle on PS) is currently pressed but was not pressed before.
	public static bool GetNorthButtonDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonNorth", "button4", true);
	}
	
	// Gets whether the north button (Y on XBox, Triangle on PS) is currently not pressed but was pressed before.
	public static bool GetNorthButtonUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonNorth", "button4", false);
	}
	
	// Gets whether the south button (A on XBox, Cross on PS) is currently pressed.
	public static bool GetSouthButton(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "buttonSouth", "button2");
	}
	
	// Gets whether the south button (A on XBox, Cross on PS) is currently pressed but was not pressed before.
	public static bool GetSouthButtonDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonSouth", "button2", true);
	}
	
	// Gets whether the south button (A on XBox, Cross on PS) is currently not pressed but was pressed before.
	public static bool GetSouthButtonUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonSouth", "button2", false);
	}
	
	// Gets whether the east button (B on XBox, Circle on PS) is currently pressed.
	public static bool GetEastButton(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "buttonEast", "trigger");
	}
	
	// Gets whether the east button (B on XBox, Circle on PS) is currently pressed but was not pressed before.
	public static bool GetEastButtonDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonEast", "trigger", true);
	}
	
	// Gets whether the east button (B on XBox, Circle on PS) is currently not pressed but was pressed before.
	public static bool GetEastButtonUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "buttonEast", "trigger", false);
	}
	
	// Gets whether the left trigger is currently pressed.
	public static bool GetLeftTrigger(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "leftTrigger", "button7");
	}
	
	// Gets whether the left trigger is currently pressed but was not pressed before.
	public static bool GetLeftTriggerDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "leftTrigger", "button7", true);
	}
	
	// Gets whether the left trigger is currently not pressed but was pressed before.
	public static bool GetLeftTriggerUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "leftTrigger", "button7", false);
	}
	
	// Gets whether the right trigger is currently pressed.
	public static bool GetRightTrigger(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "rightTrigger", "button8");
	}
	
	// Gets whether the right trigger is currently pressed but was not pressed before.
	public static bool GetRightTriggerDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "rightTrigger", "button8", true);
	}
	
	// Gets whether the right trigger is currently not pressed but was pressed before.
	public static bool GetRightTriggerUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "rightTrigger", "button8", false);
	}
	
	// Gets whether the left shoulder is currently pressed.
	public static bool GetLeftShoulder(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "leftShoulder", "button5");
	}
	
	// Gets whether the left shoulder is currently pressed but was not pressed before.
	public static bool GetLeftShoulderDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "leftShoulder", "button5", true);
	}
	
	// Gets whether the left shoulder is currently not pressed but was pressed before.
	public static bool GetLeftShoulderUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "leftShoulder", "button5", false);
	}
	
	// Gets whether the right shoulder is currently pressed.
	public static bool GetRightShoulder(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButton(gamepadInstance, "rightShoulder", "button6");
	}
	
	// Gets whether the left shoulder is currently pressed but was not pressed before.
	public static bool GetRightShoulderDown(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "rightShoulder", "button6", true);
	}
	
	// Gets whether the left shoulder is currently not pressed but was pressed before.
	public static bool GetRightShoulderUp(InputDevice gamepadInstance) {
		return GamepadExtensions.GetButtonChange(gamepadInstance, "rightShoulder", "button6", false);
	}
	
}