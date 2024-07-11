using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
[CreateAssetMenu()]
public class InputReader : ScriptableObject, GameInput.IOnFootActions ,GameInput.ICameraActions {

    //OnFoot
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<Vector2> LookEvent = delegate { };
    public event UnityAction AimEventPerformed = delegate { };
    public event UnityAction AimEventCanceled = delegate { };
    public event UnityAction InteractEvent = delegate { };
    public event UnityAction RunEventPerformed = delegate { };
    public event UnityAction RunEventCanceled = delegate { };
    public event UnityAction CrouchEvent = delegate { };
    public event UnityAction ShowInventoryEvent = delegate { };
    public event UnityAction HideInventoryEvent = delegate { };
    public event UnityAction Equip1stWeapon = delegate { };
    public event UnityAction Equip2ndWeapon = delegate { };
    public event UnityAction RotateCamEvent = delegate { };
    public event UnityAction RotateCamCanceledEvent = delegate { };


    //Camera
    public event UnityAction<float> ZoomEvent = delegate { };
    private GameInput gameInput;

    private void OnEnable() {

        if(gameInput == null) {
            gameInput = new GameInput();
        }
        
        gameInput.OnFoot.SetCallbacks(this);
        gameInput.Camera.SetCallbacks(this);

        gameInput.Enable();
    }

    #region OnFoot
    public void OnLook(InputAction.CallbackContext context) {
        //value return mouse position
        LookEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context) {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAim(InputAction.CallbackContext context) {
        //must hold to performed

        switch(context.phase) {
            case InputActionPhase.Performed:
                AimEventPerformed.Invoke();
                break;
            case InputActionPhase.Canceled:
                AimEventCanceled.Invoke();
                break;
        }

    }

    public void OnInteract(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            InteractEvent.Invoke();
        }
    }
    public void OnRun(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            RunEventPerformed.Invoke();
        }

        if(context.phase == InputActionPhase.Canceled) {
            RunEventCanceled.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            CrouchEvent.Invoke();
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            ShowInventoryEvent.Invoke();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            HideInventoryEvent.Invoke();
        }
    }

    public void OnEquip1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Equip1stWeapon.Invoke();
        }
    }

    public void OnEquip2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Equip2ndWeapon.Invoke();
        }
    }


    public void OnRotateCam(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RotateCamEvent.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            RotateCamCanceledEvent.Invoke();
        }
    }

    #endregion

    #region Camera
    public void OnZoom(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            ZoomEvent.Invoke(context.ReadValue<float>());
        }
    }
    #endregion

    public void DisableOnFootInputs()
    {
        gameInput.OnFoot.Disable();
    }
}
