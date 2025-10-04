using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Assests.InputSystem
{

    class Exit : MonoBehaviour, NewControls.IEscMenuActions
    {

        public void OnExit(InputAction.CallbackContext context)
        {
            Debug.LogWarning("OnExit called in Exit");
        }

        private void Start()
        {
            InputSystemManager.Subscribe(this);
        }


        private void OnDestroy()
        {
            InputSystemManager.UnSubscribe(this);
        }
    }
    
}