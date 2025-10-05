using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;


namespace Assests.Inputs
{

    [RequireComponent(typeof(PlayerInput))]
    public abstract class PlayerInputDispatcher : MonoBehaviour
    {
        protected PlayerInput _playerInput;
        public PlayerInput playerInput { get => _playerInput; }

        protected virtual void OnEnable() {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.onActionTriggered += BroadcastAction;
        }

        protected virtual void OnDisable()
        {
             _playerInput.onActionTriggered -= BroadcastAction;
        }

        private void BroadcastAction(InputAction.CallbackContext context)
        {
            if (!_playerInput.isActiveAndEnabled) return;
            SendMessage($"On{context.action.name}", context, SendMessageOptions.RequireReceiver);
        }
    }


    class CustomVirtualCursor : PlayerInputDispatcher, NewControls.IUtilsActions
    {
        //the cursor speed that the player can set in the settings tab
        [SerializeField] public static int cursorSpeed_playerSet { get; private set; } = 2500;

        //cursor speed used for actually moving the mouse (dynamically changes to status effects)
        public static int cursorSpeed_current;

        //used in keeping the real mouse cursor locked onto the virtual mouse
        public static bool restrain { get; private set; } = true;


        //if i ever need to reference the depth level we need the game to work on 
        //idk why i'll ever need to push anything around the depth but this is used to keep everything within the same z
        //else collisions won't happen, unless because its a 2d game that doesn't matter
        public static Vector3 targetZ = new Vector3(0, 0, 0);

        RectTransform cursorTransform;
        Canvas canvas;
        RectTransform canvas_Transform;
        Mouse virtualMouse;
        public static Vector2 MousePosition { get; private set; }
        private static Vector2 GamepadDelta { get; set; }
        public static Vector2 Position { get; private set; }



        private void Start()
        {
            InputSystemManager.Subscribe(this);
        }
        private void OnDestroy()
        {
            InputSystemManager.UnSubscribe(this);
        }


        private new void OnEnable()
        {
            base.OnEnable();

            cursorTransform = GetComponent<RectTransform>();
            canvas = cursorTransform.GetComponentInParent<Canvas>();
            canvas_Transform = canvas.GetComponentInParent<RectTransform>();


            //create a virtual mouse instance
            if (virtualMouse == null)
            {

                virtualMouse = (Mouse)UnityEngine.InputSystem.InputSystem.AddDevice("VirtualMouse");
            }
            else if (!virtualMouse.added)
            {
                UnityEngine.InputSystem.InputSystem.AddDevice(virtualMouse);
            }

            InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

            //now set virtual mouse initial position to the recttransform
            if (cursorTransform != null)
            {
                InputState.Change(virtualMouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
            }

            //set our real mouse to be invisible + unusable.
            //using confined: cause if we use locked it disables ui interaction with virtual mouse
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            //then subscribe to input system On after update event
            UnityEngine.InputSystem.InputSystem.onAfterUpdate += UpdateMotion;

            //set mouse speed to what player want it
            cursorSpeed_current = cursorSpeed_playerSet;

        }

        private new void OnDisable()
        {
            UnityEngine.InputSystem.InputSystem.onAfterUpdate -= UpdateMotion;

            //also clear the virtual mouse
            if (virtualMouse != null && virtualMouse.added)
            {
                UnityEngine.InputSystem.InputSystem.RemoveDevice(virtualMouse);
            }

            base.OnDisable();
        }

        private void UpdateMotion()
        {
            //for exception handling
            if (virtualMouse == null)
            {
                return;
            }

            //if we're using a controller
            if (Gamepad.current != null)
            {
                UsingGamepad();
            }
            //else we are using a mouse
            else
            {
                UsingMouse();
            }

            InputState.Change(virtualMouse.position, Position);
            AnchorCursor(Position);
            //cursorTransform.position = Position;
            //Debug.Log("New Pos: \n" + cursorTransform.position);


            //raycast here
            //we are making our real mouse be following our virtual mouse to make it easier to click
            //only do it if we are in a tank level and game is not paused

            //if we are null then we should be in main menu
            if (restrain)
            {
                Mouse.current.WarpCursorPosition(MousePosition);
            }

        }

        private void UsingGamepad()
        {
            var currentPos = MousePosition;
            var deltaValue = GamepadDelta;
            deltaValue *= cursorSpeed_current * Time.unscaledDeltaTime;
            var newPos = currentPos + deltaValue;
            Position = newPos;
            newPos.x = Mathf.Clamp(newPos.x, 0f, Screen.width);
            newPos.y = Mathf.Clamp(newPos.y, 0f, Screen.height);
            Position = newPos;
            InputState.Change(virtualMouse.delta, deltaValue);
        }

        private void UsingMouse()
        {
            //Position = new Vector2(1920, 1080); //testing

            //get the change in mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector2 moveDelta = new Vector2(mouseX, mouseY) * cursorSpeed_current * Time.unscaledDeltaTime;
            MousePosition += moveDelta;

            //clamp new virtual mouse position 
            var pos = MousePosition;
            pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
            pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
            MousePosition = pos;

            // update new position to be current position
            Position = MousePosition;
        }

        private void AnchorCursor(Vector2 newPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas_Transform,
            newPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out var anchoredPosition);

            cursorTransform.anchoredPosition = anchoredPosition;
        }




        public void OnCursorPosition(InputAction.CallbackContext context)
        {
            Debug.Log("Oncursor_Position:");
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnCursorMove(InputAction.CallbackContext context)
        {
            Debug.Log("OnCursor_Move");
            GamepadDelta = context.ReadValue<Vector2>();
        }





        /// <summary>
        /// Returns the current virtual mouse position in terms of physical tank position. (vector2)
        /// To get UI virtual mouse position call the variable MousePosition
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMousePosition_V2()
        {

            //get our TargetZ object to determine on what z we should be setting our mouse at
            float z = Vector3.Dot(Camera.main.transform.forward, targetZ - Camera.main.transform.position);
            Vector3 newMousePosition = new Vector3(MousePosition.x, MousePosition.y, z);

            return Camera.main.ScreenToWorldPoint(newMousePosition); //convert to world pos

        }


        public static Vector3 GetMousePosition_V3()
        {

            //get our TargetZ object to determine on what z we should be setting our mouse at
            float z = Vector3.Dot(Camera.main.transform.forward, targetZ - Camera.main.transform.position);
            Vector3 newMousePosition = new Vector3(MousePosition.x, MousePosition.y, z);

            return Camera.main.ScreenToWorldPoint(newMousePosition); //convert to world pos

        }


        //used in setting our restrain variable
        //if given true. we run like normal
        public static void SetRestrain(bool newRestrain)
        {
            //how this should be:
            ///if we are restrained (set to true)
            ///     we are settting our restrain to true
            ///     we are setting our cursur to not visable
            ///     we are setting our cursor to be confined
            /// 
            /// else we are unconstrained
            /// so the opposite of the upper in everything

            restrain = newRestrain;
            Cursor.visible = !newRestrain;
            Cursor.lockState = newRestrain ? CursorLockMode.Confined : CursorLockMode.None;

        }


    }
}