using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private float cursorSpeed = 1000f;

        RectTransform cursorTransform;
        Canvas canvas;
        RectTransform canvas_Transform;
        Mouse virtualMouse;
        private static Vector2 MousePosition { get; set; }
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
                var pos = cursorTransform.anchoredPosition;
                InputState.Change(virtualMouse.position, pos);
            }

            //set our real mouse to be invisible + unusable.
            //using confined: cause if we use locked it disables ui interaction with virtual mouse
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            //then subscribe to input system On after update event
            UnityEngine.InputSystem.InputSystem.onAfterUpdate += UpdateMotion;


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

        }

        private void UsingGamepad()
        {
            var currentPos = MousePosition;
            var deltaValue = GamepadDelta;
            deltaValue *= cursorSpeed * Time.unscaledDeltaTime;
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

            Vector2 moveDelta = new Vector2(mouseX, mouseY) * cursorSpeed * Time.deltaTime;
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


    }
}