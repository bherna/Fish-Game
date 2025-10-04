using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class AutoHideVirtualMouse : MonoBehaviour
{
    [SerializeField] private InputActionReference _stickAction;
    [SerializeField] private InputActionReference _mousePointAction;
    [SerializeField] private InputActionReference _clickAction;
    [SerializeField] private Graphic _image;
    [SerializeField] private RectTransform _canvasTransform;
    [SerializeField] private float _moveSpeed = 500;

    private Mouse _virtualMouse;
    private RectTransform _cursorTransform;

    private Vector2 _delta;
    private void Awake()
    {
        _cursorTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        if (_virtualMouse == null)
        {
            _virtualMouse = InputSystem.AddDevice<Mouse>("VirtualMouse");
        }

        SetCursorEnabled(false);

        _stickAction.action.Enable();
        _mousePointAction.action.Enable();
        _clickAction.action.Enable();

        _stickAction.action.performed += OnStickMove;
        _mousePointAction.action.performed += OnMouseMove;
        _clickAction.action.performed += OnClick;

        InputSystem.onAfterUpdate += OnAfterUpdate;
    }

    private void OnDisable()
    {
        SetCursorEnabled(false);

        InputSystem.onAfterUpdate -= OnAfterUpdate;

        _stickAction.action.performed -= OnStickMove;
        _mousePointAction.action.performed -= OnMouseMove;
        _clickAction.action.performed -= OnClick;
    }
    private void SetCursorEnabled(bool enabled)
    {
        Cursor.visible = !enabled;
        _image.enabled = enabled;

        if (_virtualMouse == null) return;
        if (enabled)
        {
            if (!_virtualMouse.added) InputSystem.AddDevice(_virtualMouse);

            //move virtual mouse to screen center instead of having it start at (0,0)
            InputState.Change(_virtualMouse.position, new Vector2(Screen.width / 2, Screen.height / 2));
        }
        else
        {
            InputSystem.RemoveDevice(_virtualMouse);
        }
    }
    private void OnAfterUpdate()
    {
        if (!_virtualMouse.added) return;

        var newPosition = (_virtualMouse.position.ReadValue() + _delta * _moveSpeed * Time.deltaTime);
        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        //move virtual mouse screen position
        InputState.Change(_virtualMouse.position, newPosition);
        InputState.Change(_virtualMouse.delta, _delta * _moveSpeed * Time.deltaTime);

        //move cursor transform accordingly
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasTransform, newPosition, null, out Vector3 localPoint);
        _cursorTransform.position = localPoint;
    }
    private void OnStickMove(InputAction.CallbackContext ctx)
    {
        if (!_image.enabled)
        {
            SetCursorEnabled(true);
        }
        if (!_virtualMouse.added) return;
        _delta = ctx.ReadValue<Vector2>();
    }
    private void OnClick(InputAction.CallbackContext ctx)
    {
        if (!_virtualMouse.added) return;
        _virtualMouse.CopyState(out MouseState mouseState);
        mouseState.WithButton(MouseButton.Left, ctx.ReadValueAsButton());
        InputState.Change(_virtualMouse, mouseState);
    }
    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        if (_image.enabled && ctx.control.device.name != "VirtualMouse")
        {
            StartCoroutine(Co_EndOfFrame());
        }
        //using local coroutine because removing the device before end of frame causes errors
        IEnumerator Co_EndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            SetCursorEnabled(false);
        }
    }

}