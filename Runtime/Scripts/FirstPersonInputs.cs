using UnityEngine;
using UnityEngine.InputSystem;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
    public class FirstPersonInputs : MonoBehaviour
    {
        [field: Header("Input Values")]
        [field: SerializeField]
        public Vector2 Move { get; private set; }

        [field: SerializeField]
        public Vector2 Look { get; private set; }

        [field: SerializeField]
        public bool Jump { get; set; } // This is settable so that the jump input can be reset after being used.

        [field: SerializeField]
        public bool Sprint { get; private set; }

        [field: SerializeField]
        public bool Crouch { get; private set; }

        [Header("Mouse Settings")]
        [SerializeField]
        private bool lockCursor = true;

        [SerializeField]
        private bool mouseLook = true;

        public void MoveInput(InputAction.CallbackContext context) =>
            Move = context.ReadValue<Vector2>();

        public void LookInput(InputAction.CallbackContext context)
        {
            if (mouseLook)
                Look = context.ReadValue<Vector2>();
        }

        public void JumpInput(InputAction.CallbackContext context)
        {
            if (context.performed)
                Jump = true;
        }

        public void SprintInput(InputAction.CallbackContext context)
        {
            if (context.started)
                Sprint = true;
            else if (context.canceled)
                Sprint = false;
        }

        public void CrouchInput(InputAction.CallbackContext context)
        {
            if (context.started)
                Crouch = true;
            else if (context.canceled)
                Crouch = false;
        }

        private void OnApplicationFocus(bool hasFocus) =>
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
