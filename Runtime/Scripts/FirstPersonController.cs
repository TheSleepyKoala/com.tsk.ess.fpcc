using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
    ///<summary>
    ///A script that handles the Movement and camera control of a first person character.
    ///</summary>
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    [RequireComponent(typeof(PlayerInput), typeof(FirstPersonInputs))]
    public class FirstPersonController : MonoBehaviour
    {
        /*
            This section contains the declaration of various components that are required for the script to work.
            These include the Rigidbody, PlayerInput, FirstPersonInputs, and FirstPersonControllerSettings.
        */
        #region Components
        [Header("Components")]
        [SerializeField, Tooltip("The rigidbody component attached to the player.")]
        private Rigidbody rb;

        [SerializeField, Tooltip("The capsule collider component attached to the player.")]
        private CapsuleCollider capsuleCollider;

        [SerializeField, Tooltip("The player input script attached to the player.")]
        private PlayerInput playerInput;

        [SerializeField, Tooltip("The first person inputs script attached to the player.")]
        private FirstPersonInputs firstPersonInputs;

        [SerializeField, Tooltip("The first person controller settings scriptable object.")]
        private FirstPersonControllerSettings settings;
        #endregion

        /*
            This section handles the ground check for the player.
            It uses a Raycast to check if the player is standing on the ground.
            The CheckGround() function is called every FixedUpdate().
        */
        #region Ground Check
        private bool isGrounded,
            onSlope,
            exitingSlope;
        private float delayTimer;
        private RaycastHit slopeHit;

        private void CheckGround()
        {
            Vector3 origin = transform.position - Vector3.up * (transform.localScale.y / 2);
            Vector3 direction = Vector3.down;

            if (
                Physics.Raycast(
                    origin,
                    direction,
                    out _,
                    settings.groundCheckDistance,
                    settings.groundMask
                )
            )
            {
                isGrounded = true;
                onSlope = CheckIfSlope();
                delayTimer = settings.timeBeforeNotGrounded; // reset the delay timer
            }
            else
            {
                delayTimer -= Time.deltaTime;
                if (delayTimer > 0)
                {
                    isGrounded = false;
                }
            }
        }

        private bool CheckIfSlope()
        {
            float playerHeight = capsuleCollider.height / 2 + 0.3f; // 0.3f is the offset

            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight))
            {
                // The rotation of the slopeHit normal is the angle of the slope.
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < settings.maxSlopeAngle && angle != 0;
            }

            return false;
        }
        #endregion

        /*
            This section handles the player's jump.
            If the player presses the jump button and is currently on the ground,
            the player will jump using the Rigidbody.AddForce() function.
        */
        #region Jump
        private bool hasJumped = false;

        /// <summary>
        /// Makes the player jump if the player is grounded.
        /// </summary>
        private void Jump()
        {
            if (firstPersonInputs.Jump && !hasJumped && isGrounded)
            {
                rb.AddForce(0f, settings.jumpForce, 0f, ForceMode.Impulse);
                exitingSlope = true;

                firstPersonInputs.Jump = false; // reset jump input to stop auto jumping
                hasJumped = true;

                Invoke(nameof(ResetJump), settings.jumpCooldown); // reset jump after jump delay
            }
        }

        private void ResetJump()
        {
            hasJumped = false;
            exitingSlope = false;
        }

        #endregion

        /*
            This section handles the player's movement.
            It first gets the player's input using the FirstPersonInputs script.
            If sprinting is enabled, and the player is sprinting, the player will move at the sprint speed.
            Otherwise, the player will move at the walk speed.
            It then applies a force to the player's Rigidbody to move them.
        */
        #region Movement
        private float originalWalkSpeed;

        /// <summary>
        /// Moves the player based on the player's input.
        /// </summary>
        private void MovePlayer()
        {
            Vector3 velocityChange = SpeedControl();

            // If player is on a slope, apply slope multiplier to velocityChange.
            if (onSlope && !exitingSlope)
            {
                rb.AddForce(
                    GetSlopeMoveDirection(velocityChange) * settings.slopeMultiplier,
                    ForceMode.VelocityChange
                );

                if (rb.velocity.y > 0)
                    rb.AddForce(Vector3.down * 180f, ForceMode.Force);
            }
            else if (isGrounded)
            {
                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }

            rb.useGravity = !onSlope;
        }

        /// <summary>
        /// The SpeedControl() function calculates the target velocity based on the player's input.
        /// </summary>
        private Vector3 SpeedControl()
        {
            Vector2 move = firstPersonInputs.Move;
            Vector3 targetVelocity = new(move.x, 0f, move.y);

            if (settings.enableSprint && firstPersonInputs.Sprint && isGrounded && !isCrouching)
                targetVelocity =
                    transform.TransformDirection(targetVelocity) * settings.sprintSpeed;
            else
                targetVelocity = transform.TransformDirection(targetVelocity) * settings.walkSpeed;

            // Apply a force that attempts to reach target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(
                velocityChange.x,
                -settings.maxVelocityChange,
                settings.maxVelocityChange
            );
            velocityChange.z = Mathf.Clamp(
                velocityChange.z,
                -settings.maxVelocityChange,
                settings.maxVelocityChange
            );
            velocityChange.y = 0f;

            return velocityChange;
        }

        /// <summary>
        /// Calculates the slope factor based on the player's current height.
        /// Note:
        /// If creating a flat surface, the GameObject's rotation must be set to (0, 0, 0).
        /// </summary>

        private Vector3 GetSlopeMoveDirection(Vector3 velocityChange) =>
            Vector3.ProjectOnPlane(velocityChange, slopeHit.normal);
        #endregion

        /*
            This section handles the player's crouch.
            If the player presses the crouch button and is currently on the ground, the player will crouch.
            The player's scale will be reduced, and their walk speed will be decreased based on the crouchSpeedModifier.
        */
        #region Crouch
        private bool isCrouching;
        private Vector3 originalScale;

        /// <summary>
        /// Makes the player crouch if the player is not crouching.
        /// </summary>
        private void Crouch()
        {
            if (firstPersonInputs.Crouch && !isCrouching && isGrounded)
            {
                isCrouching = true;
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    settings.crouchHeight,
                    transform.localScale.z
                );
                settings.walkSpeed = originalWalkSpeed * settings.crouchSpeedModifier;
            }
            else if (!firstPersonInputs.Crouch && isCrouching)
            {
                isCrouching = false;
                transform.localScale = originalScale;
                settings.walkSpeed = originalWalkSpeed;
            }
        }
        #endregion

        /*
            This section handles the player's camera.
            It first gets the player's input using the FirstPersonInputs script.
            It then rotates the camera based on the input.
            The camera's pitch is modified by the look.y input, while the camera's yaw is modified by the look.x input.
            The camera's pitch is clamped between the bottomAngle and topAngle.
        */
        #region Player Camera
        [Header("Camera")]
        [SerializeField, Tooltip("The main camera attached to the player.")]
        private Camera mainCamera;

        [SerializeField, Tooltip("The transform of the player's head.")]
        private Transform head;
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

            float lookSpeed = settings.rotationSpeed * settings.deltaTimeMultiplier;

            cameraTargetPitch += look.y * lookSpeed;
            rotationVelocity = look.x * lookSpeed;

            cameraTargetPitch = ClampAngle(
                cameraTargetPitch,
                settings.bottomAngle,
                settings.topAngle
            );

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

        private void Start()
        {
            originalWalkSpeed = settings.walkSpeed;
            originalScale = transform.localScale;
        }

        private void FixedUpdate()
        {
            CheckGround();

            if (!settings.enableMovement)
                return;

            MovePlayer();

            if (settings.enableJump)
                Jump();

            if (settings.enableCrouch)
                Crouch();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }
    }
}
