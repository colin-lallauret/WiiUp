using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool crouch;
        public bool grenade;  // Pour le clic gauche
        public bool parachute; // Pour le clic droit

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value) => MoveInput(value.Get<Vector2>());
        public void OnLook(InputValue value) => LookInput(value.Get<Vector2>());
        public void OnJump(InputValue value) => JumpInput(value.isPressed);
        public void OnSprint(InputValue value) => SprintInput(value.isPressed);
        public void OnCrouch(InputValue value) => CrouchInput(value.isPressed);
        public void OnGrenade(InputValue value) => GrenadeInput(value.isPressed);
        public void OnParachute(InputValue value) => ParachuteInput(value.isPressed);
#endif

        public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
        public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
        public void JumpInput(bool newJumpState) => jump = newJumpState;
        public void SprintInput(bool newSprintState) => sprint = newSprintState;
        public void CrouchInput(bool newCrouchState) => crouch = newCrouchState;
        public void GrenadeInput(bool newGrenadeState) => grenade = newGrenadeState;
        public void ParachuteInput(bool newParachuteState) => parachute = newParachuteState;

        private void OnApplicationFocus(bool hasFocus) => SetCursorState(cursorLocked);
        private void SetCursorState(bool newState) => Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}