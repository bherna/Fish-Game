using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemManager : MonoBehaviour, NewControls.IEscMenuActions, NewControls.IUtilsActions
{

    private NewControls inputActions;




    //single ton this class
    public static InputSystemManager instance { get; private set; }

    private void Awake()
    {

        //delete duplicate of this instance
        if (instance == null)
        {

            instance = this;
            inputActions = new NewControls();
            inputActions.Enable();
            inputActions.EscMenu.SetCallbacks(this);
            inputActions.Utils.SetCallbacks(this);

        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }








    public void OnExit(InputAction.CallbackContext context)
    {

        Debug.LogWarning("OnExit in Manager");
        //proxy to make sure that actions are called at the right times
        //filter events by event type
        if (context.phase == InputActionPhase.Performed)
        {
            foreach (var action in esc_Actions)
            {
                action.OnExit(context);
            }
        }
        
        
    }


    public void OnCursorPosition(InputAction.CallbackContext context)
    {
        Debug.LogWarning("OnCursorPosition  has been changed");
        //proxy to make sure that actions are called at the right times
        foreach (var action in utils_Actions)
        {
            action.OnCursorPosition(context);
        }
        
    }

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        Debug.LogWarning("OnCursorMove  has been changed");
        //proxy to make sure that actions are called at the right times
        foreach (var action in utils_Actions)
        {
            action.OnCursorMove(context);
        }
    }






    private static List<NewControls.IEscMenuActions> esc_Actions = new List<NewControls.IEscMenuActions>();
    private static List<NewControls.IUtilsActions> utils_Actions = new List<NewControls.IUtilsActions>();


    public static void Subscribe(NewControls.IEscMenuActions newAction)
    {
        esc_Actions.Add(newAction);
    }
    public static void Subscribe(NewControls.IUtilsActions newAction)
    {
        utils_Actions.Add(newAction);
    }


    public static void UnSubscribe(NewControls.IEscMenuActions newAction)
    {
        esc_Actions.Remove(newAction);
    }
    public static void UnSubscribe(NewControls.IUtilsActions newAction)
    {
        utils_Actions.Remove(newAction);
    }



    
}
