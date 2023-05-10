using UnityEngine;
using UnityEngine.InputSystem;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
    public class FirstPersonInputs : MonoBehaviour
    {
        [field: Header("Input Values")]
        [field: SerializeField] public Vector2 Move { get; private set; }
        [field: SerializeField] public Vector2 Look { get; private set; }
        [field: SerializeField] public bool Jump { get; set; }
        [field: SerializeField] public bool Sprint { get; private set; }

        [Header ("Mouse Settings")]
        [SerializeField] private bool lockCursor = true;
        [SerializeField] private bool mouseLook = true;

        public void OnMove(InputValue value) => Move = value.Get<Vector2>();
        public void OnLook(InputValue value)
        {
            if (mouseLook)
                Look = value.Get<Vector2>();
        }
        public void OnJump(InputValue value) => Jump = value.isPressed;
        public void OnSprint(InputValue value) => Sprint = value.isPressed;

        private void OnApplicationFocus(bool hasFocus) => Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
