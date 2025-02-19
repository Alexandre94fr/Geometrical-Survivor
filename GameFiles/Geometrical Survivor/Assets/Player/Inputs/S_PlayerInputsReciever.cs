using UnityEngine;
using UnityEngine.InputSystem;

public class S_PlayerInputsReciever : MonoBehaviour
{
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

    public void OnActiveCapacityUseInput(InputAction.CallbackContext p_callbackContext)
    {
        if (p_callbackContext.performed)
        {
            // TODO: Invoke S_Player event nammed OnActiveCapacityUseEvent
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