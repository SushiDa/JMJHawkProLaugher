using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInputWrapper : MonoBehaviour
{
    [SerializeField]
    private PlayerInputHub inputHub;

    void OnCursorMovement(InputValue value)
    {
        // (x < 0 => left) (x > 0 => right)
        // (y < 0 => down) (y > 0 => up)
        Vector2 movement = value.Get<Vector2>();
        inputHub.CursorMovement = movement;
    }

    void OnMove(InputValue value)
    {
        // (x < 0 => left) (x > 0 => right)
        float movement = value.Get<float>();
        inputHub.Movement = movement;
    }

    void OnRotate(InputValue value)
    {
        // (x < 0 => left) (x > 0 => right)
        float rotation = value.Get<float>();
        inputHub.Rotation = rotation;
    }

    void OnJump()
    {
        inputHub.IsJump = true;
    }

    void OnInteract()
    {
        inputHub.IsInteract = true;
    }

    public void SetActionMap(string map)
    {
        PlayerInput input = GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap(map);
    }
}