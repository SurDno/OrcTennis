using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// A modified version of standard Input System Joystick.cs that implements basic gamepad support for joysticks identified as gamepads.
// I'll be honest: I've got no idea what happens in half of this code. It's too low level. I just poked around until I got something working.
// It only implements basic controls, there is no rumble support.
namespace UnityEngine.InputSystem.LowLevel {
    internal struct JoystickState : IInputStateTypeInfo {
        public static FourCC kFormat => new FourCC('J', 'O', 'Y');
        public FourCC format => kFormat;
    }
}

namespace UnityEngine.InputSystem {
    [InputControlLayout(stateType = typeof(JoystickState), isGenericTypeOfDevice = true)]
    public class Joystick : InputDevice {
		// New by SurDno: replaced joystick-specific variables for gamepad-specific, with separate axes for right stick.
        public ButtonControl buttonWest { get; protected set; }
        public ButtonControl buttonNorth { get; protected set; }
        public ButtonControl buttonSouth { get; protected set; }
        public ButtonControl buttonEast { get; protected set; }
        public ButtonControl leftStickButton { get; protected set; }
        public ButtonControl rightStickButton { get; protected set; }
        public ButtonControl startButton { get; protected set; }
        public ButtonControl selectButton { get; protected set; }
        public DpadControl dpad { get; protected set; }
        public ButtonControl leftShoulder { get; protected set; }
        public ButtonControl rightShoulder { get; protected set; }
        public StickControl leftStick { get; protected set; }
        public AxisControl rightStickX { get; protected set; }
        public AxisControl reversedRightStickY { get; protected set; }
		public ButtonControl leftTrigger { get; protected set; }
		public ButtonControl rightTrigger { get; protected set; }
        
        public static Joystick current { get; private set; }
        public new static ReadOnlyArray<Joystick> all => new ReadOnlyArray<Joystick>(s_Joysticks, 0, s_JoystickCount);
        
        protected override void FinishSetup() {
			// New by SurDno: replaced joystick-specific variables for gamepad-specific, with separate axes for right stick.
            buttonWest = GetChildControl<ButtonControl>("button3");
            buttonNorth = GetChildControl<ButtonControl>("button4");
            buttonSouth = GetChildControl<ButtonControl>("button2");
            buttonEast = GetChildControl<ButtonControl>("trigger");

            startButton = GetChildControl<ButtonControl>("button9");
            selectButton = GetChildControl<ButtonControl>("button10");

            leftStickButton = GetChildControl<ButtonControl>("button11");
            rightStickButton = GetChildControl<ButtonControl>("button12");

            dpad = GetChildControl<DpadControl>("hat");

            leftShoulder = GetChildControl<ButtonControl>("button5");
            rightShoulder = GetChildControl<ButtonControl>("button6");

            leftStick = GetChildControl<StickControl>("stick");
            rightStickX = GetChildControl<AxisControl>("z");
			// For reasons unknown to me, "rz" returns -1 when the stick is in top position and 1 when it's in bottom position, so that's reflected in the name.
            reversedRightStickY = GetChildControl<AxisControl>("rz");

            leftTrigger = GetChildControl<ButtonControl>("button7");
            rightTrigger = GetChildControl<ButtonControl>("button8");

            base.FinishSetup();
        }
		
        public override void MakeCurrent() {
            base.MakeCurrent();
            current = this;
        }
        
        protected override void OnAdded() {
            ArrayHelpers.AppendWithCapacity(ref s_Joysticks, ref s_JoystickCount, this);
        }
        
        protected override void OnRemoved() {
            base.OnRemoved();

            if (current == this)
                current = null;

            // Remove from `all`.
            var index = ArrayHelpers.IndexOfReference(s_Joysticks, this, s_JoystickCount);
            if (index != -1)
                ArrayHelpers.EraseAtWithCapacity(s_Joysticks, ref s_JoystickCount, index);
            else {
                Debug.Assert(false,
                    $"Joystick {this} seems to not have been added but is being removed (joystick list: {string.Join(", ", all)})"); // Put in else to not allocate on normal path.
            }
        }

        private static int s_JoystickCount;
        private static Joystick[] s_Joysticks;
    }
}
