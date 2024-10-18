using UnityEngine;
using System;

namespace Menu
{
    [AddComponentMenu("GameDev/Menu/Input Handler")]
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;
        public enum InputMode { Controller, Keyboard, Touch }
        private InputMode _currentInputMode;
        private InputMode _inputModeLastFrame;
        public static Action<InputMode> OnInputModeChanged;
        public bool onController, onKeyboard, onTouch;

        public bool forceController;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null && instance != this)
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            _currentInputMode = InputMode.Keyboard;
        }

        private void Update()
        {
            _currentInputMode = ProcessInputMode();

            if (_currentInputMode != _inputModeLastFrame)
            {
                OnInputModeChanged?.Invoke(_currentInputMode);
            }

            _inputModeLastFrame = ProcessInputMode();


            if (_currentInputMode == InputMode.Controller)
            {
                onController = true;
                onKeyboard = false;
                onTouch = false;
            }
            else if (_currentInputMode == InputMode.Keyboard)
            {
                onKeyboard = true;
                onController = false;
                onTouch = false;
            }
            else if (_currentInputMode == InputMode.Touch)
            {
                onTouch = true;
                onController = false;
                onKeyboard = false;
            }
            else
            {
                Debug.LogWarning("THIS SHOULD NEVER SHOW!!!");
            }
        }

        private InputMode ProcessInputMode()
        {
            if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return InputMode.Touch;
            }

            if (Input.GetJoystickNames().Length == 0)
            {
                // If there are no controllers plugged in return keyboard input mode
                return InputMode.Keyboard;
            }

            if (Input.anyKeyDown)
            {
                // This detects if regular input buttons are pressed on a controller
                if (Input.GetKeyDown(KeyCode.JoystickButton0)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton1)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton2)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton3)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton4)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton5)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton6)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton7)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton8)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton9)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton10)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton11)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton12)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton13)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton14)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton15)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton16)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton17)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton18)) return InputMode.Controller;
                else if (Input.GetKeyDown(KeyCode.JoystickButton19)) return InputMode.Controller;
                else return InputMode.Keyboard; // If a key is pressed but it's not a joystick button it has to come from mouse/keyboard
            }

            if (Input.anyKey)
            {
                // Unity will only recognize Input.anyKey for keyboard presses
                // Only need to put code in here if the axis is used for both mouse and keyboard
                if (Input.GetAxisRaw("Horizontal") != 0) return InputMode.Keyboard;
                if (Input.GetAxisRaw("Vertical") != 0) return InputMode.Keyboard;
            }

            // Left joystick axis buttons
            if (Input.GetAxisRaw("Horizontal") != 0) return InputMode.Controller;
            if (Input.GetAxisRaw("Vertical") != 0) return InputMode.Controller;

            // ***NOTE***
            // By default in a Unity project only "Horizontal" and "Vertical" are setup in the Project Settings
            // The axis configurations below MUST be added manually in order for them to work
            // The mappings can be found here: https://bit.ly/4gjr5cJ

            // Right joystick axis buttons
            if (Input.GetAxisRaw("Mouse X - RightStick") != 0) return InputMode.Controller;
            if (Input.GetAxisRaw("Mouse Y - RightStick") != 0) return InputMode.Controller;

            // D-Pad axis buttons
            // if (Input.GetAxisRaw("Horizontal") != 0) return InputMode.Controller;
            // if (Input.GetAxisRaw("Vertical") != 0) return InputMode.Controller;

            // Left and right trigger axis buttons
            // if (Input.GetAxisRaw("LeftTrigger") < 0) return InputMode.Controller;
            // if (Input.GetAxisRaw("RightTrigger") < 0) return InputMode.Controller;
            return _currentInputMode;
        }
    }
}