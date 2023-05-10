using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
    ///<summary>
    ///A script that handles the Movement and camera control of a first person character.
    ///</summary>
    [RequireComponent(typeof(PlayerInput))]
    public class FirstPersonController : MonoBehaviour
    {
        #region Components
        [SerializeField, Tooltip("The rigidbody component attached to the player.")]
        private Rigidbody rb;
        [SerializeField, Tooltip("The player input script attached to the player.")]
        private PlayerInput playerInput;
        [SerializeField, Tooltip("The first person inputs script attached to the player.")]
        private FirstPersonInputs firstPersonInputs;
        #endregion

        #region Ground Check
        [SerializeField, Tooltip("The boolean that determines if the player is grounded or not.")]
        private bool isGrounded;
        [SerializeField, Tooltip("The layer mask that determines what is considered ground.")]
        private LayerMask groundMask;

        private const float groundCheckDistance = 0.75f;

        // Sets isGrounded based on a raycast sent straight down from the player object
        private void CheckGround()
        {
            Vector3 origin = transform.position - Vector3.up * (transform.localScale.y / 2);
            Vector3 direction = Vector3.down;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, groundCheckDistance, groundMask))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
        #endregion

        #region Jump
        [Header("Jump")]
        [SerializeField, Tooltip("The boolean that determines if the player can jump or not.")]
        private bool enableJump = true;
        [SerializeField, Tooltip("The force applied to the player when jumping.")]
        private float jumpForce = 5f;

        /// <summary>
        /// Makes the player jump if the player is grounded.
        /// </summary>
        private void Jump()
        {
            rb.AddForce(0f, jumpForce, 0f, ForceMode.Impulse);
            isGrounded = false;
        }
        #endregion

        #region Movement
        [Header("Movement")]
        [SerializeField, Tooltip("The boolean that determines if the player can move or not.")]
        private bool enableMovement = true;
        [SerializeField, Tooltip("The boolean that determines if the player can sprint or not.")]
        private bool enableSprint = true;
        [SerializeField, Tooltip("The walking speed of the player.")]
        private float walkSpeed = 3f;
        [SerializeField, Tooltip("The sprinting speed of the player.")]
        private float sprintSpeed = 6f;
        [SerializeField, Tooltip("The maximum velocity change of the player.")]
        private float maxVelocityChange = 10f;
        private Vector3 movementDirection;

        /// <summary>
        /// Moves the player based on the player's input.
        /// </summary>
        private void MovePlayer()
        {
            Vector2 move = firstPersonInputs.Move;
            Vector3 targetVelocity = new Vector3(move.x, 0f, move.y);

            if (enableSprint && firstPersonInputs.Sprint)
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;
            else
                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        #endregion

        #region Player Camera
        [Header("Camera")]
        [SerializeField, Tooltip("The main camera attached to the player.")]
        private Camera mainCamera;
        [SerializeField, Tooltip("The transform of the player's head.")]
        private Transform head;
        [SerializeField, Tooltip("The maximum angle the camera can look up.")]
        private float topAngle = 90f;
        [SerializeField, Tooltip("The maximum angle the camera can look down.")]
        private float bottomAngle = -90f;
        [SerializeField, Tooltip("The speed at which the camera rotates.")]
        private float rotationSpeed = 1f;
        [SerializeField, Tooltip("The multiplier for the deltaTime value used in camera rotation.")]
        private float deltaTimeMultiplier = 1f;
        private float cameraTargetPitch;
        private float rotationVelocity;
        private const float threshold = 0.01f;

        /// <summary>
        /// Rotates the camera based on the player's input.
        /// </summary>
        private void CameraRotation()
        {
            Vector2 look = firstPersonInputs.Look;
            float lookMagnitude = look.sqrMagnitude;

            if (lookMagnitude < threshold)
                return;

            float lookSpeed = rotationSpeed * deltaTimeMultiplier;

            cameraTargetPitch += look.y * lookSpeed;
            rotationVelocity = look.x * lookSpeed;

            cameraTargetPitch = ClampAngle(cameraTargetPitch, bottomAngle, topAngle);

            head.transform.localRotation = Quaternion.AngleAxis(cameraTargetPitch, Vector3.right);
            transform.Rotate(Vector3.up * rotationVelocity);
        }

        /// <summary>
        /// Clamps an angle between a minimum and maximum value.
        /// </summary>
        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            lfAngle %= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        #endregion

        private void FixedUpdate()
        {
            CheckGround();
            if (!enableMovement)
                return;

            MovePlayer();

            if (enableJump && firstPersonInputs.Jump && isGrounded)
                Jump();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }
    }
}
