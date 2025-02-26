using UnityEngine;
using UnityEngine.InputSystem;

public class S_PlayerInputsReciever : MonoBehaviour
{
    [Header(" External references :")]
    [SerializeField] Camera _playerCamera;

    void Start()
    {
        S_VariablesChecker.AreVariablesCorrectlySetted(gameObject.name, null, 
            (_playerCamera, nameof(_playerCamera))
        );
    }

    public void OnMovementInput(InputAction.CallbackContext p_callbackContext)
    {
        if (p_callbackContext.performed)
        {
            S_PlayerController._OnPlayerMoveInputEvent?.Invoke(p_callbackContext.ReadValue<Vector2>());
        }

        if (p_callbackContext.canceled)
        {
            S_PlayerController._OnPlayerMoveInputEvent?.Invoke(p_callbackContext.ReadValue<Vector2>());
        }
    }

    public void OnRotateInput(InputAction.CallbackContext p_callbackContext)
    {
        if (!p_callbackContext.performed) 
            return;

        Vector2 rotationInput = p_callbackContext.ReadValue<Vector2>();

        // The recieved Vector2 orientation is incorrect, but ONLY when given by the mouse, not the controller.
        // The reason behind this is because the mouse has his (0, 0) position is not at the center of the screen, but at the left down of the screen.
        // Our objective is to convert that data into the correct one (the orientation around the player).

        // If the input comes from the mouse
        if (Mouse.current != null && p_callbackContext.control.device is Pointer)
        {
            // Converting the mouse screen position into mouse world position
            Vector3 mouseWorldPosition = _playerCamera.ScreenToWorldPoint(new Vector3(rotationInput.x, rotationInput.y, _playerCamera.nearClipPlane));

            // Compute the normalized direction beetween the mouse world position and the player world position
            Vector2 direction = (mouseWorldPosition - transform.parent.transform.position).normalized;

            S_PlayerController._OnPlayerRotateEvent?.Invoke(direction);
        }
        else
        {
            // The value sent by the controller is already correct, so there is no need to change it
            S_PlayerController._OnPlayerRotateEvent?.Invoke(rotationInput);
        }
    }

    public void OnActiveCapacityUseInput(InputAction.CallbackContext p_callbackContext)
    {
        if (p_callbackContext.performed)
        {
            S_PlayerController._OnActiveCapacityUseEvent?.Invoke();
        }
    }

    public void OnPauseUnPauseInput(InputAction.CallbackContext p_callbackContext)
    {
        if (p_callbackContext.performed)
        {
            // TODO: Invoke S_Player event nammed OnPlayerPauseEvent
        }
    }
}