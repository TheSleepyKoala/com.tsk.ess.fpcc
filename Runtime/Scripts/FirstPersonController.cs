using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
    ///<summary>
    ///A script that handles the Movement and camera control of a first person character.
    ///</summary>
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody), typeof(FirstPersonInputs))]
    public class FirstPersonController : MonoBehaviour
    {
        #region Components
        [Header("Components")]
        [SerializeField, Tooltip("The rigidbody component attached to the player.")]
        private Rigidbody rb;
        [SerializeField, Tooltip("The player input script attached to the player.")]
        private PlayerInput playerInput;
        [SerializeField, Tooltip("The first person inputs script attached to the player.")]
        private FirstPersonInputs firstPersonInputs;
        [SerializeField, Tooltip("The first person controller settings scriptable object.")]
        private FirstPersonControllerSettings settings;
        #endregion

        #region Ground Check
        private bool isGrounded;
        private const float groundCheckDistance = 0.75f;

        private void CheckGround()
        {
            Vector3 origin = transform.position - Vector3.up * (transform.localScale.y / 2);
            Vector3 direction = Vector3.down;
    
            if (Physics.Raycast(origin, direction, out _, groundCheckDistance, settings.groundMask))
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
        /// <summary>
        /// Makes the player jump if the player is grounded.
        /// </summary>
        private void Jump()
        {
            if (firstPersonInputs.Jump && isGrounded)
            {
                rb.AddForce(0f, settings.jumpForce, 0f, ForceMode.Impulse);
                isGrounded = false;
            }
        }
        #endregion

        #region Movement
        private float originalWalkSpeed;

        /// <summary>
        /// Moves the player based on the player's input.
        /// </summary>
        private void MovePlayer()
        {
            Vector2 move = firstPersonInputs.Move;
            Vector3 targetVelocity = new Vector3(move.x, 0f, move.y);

            if (settings.enableSprint && firstPersonInputs.Sprint && isGrounded && !isCrouching)
                targetVelocity = transform.TransformDirection(targetVelocity) * settings.sprintSpeed;
            else
                targetVelocity = transform.TransformDirection(targetVelocity) * settings.walkSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -settings.maxVelocityChange, settings.maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -settings.maxVelocityChange, settings.maxVelocityChange);
            velocityChange.y = 0f;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        #endregion

        #region Crouch
        private bool isCrouching;
        private Vector3 originalScale;

        /// <summary>
        /// Makes the player crouch if the player is not crouching.
        /// </summary>
        private void Crouch()
        {
            if(firstPersonInputs.Crouch && !isCrouching && isGrounded)
            {   
                isCrouching = true;
                transform.localScale = new Vector3(transform.localScale.x, settings.crouchHeight, transform.localScale.z);
                settings.walkSpeed = originalWalkSpeed * settings.crouchSpeedModifier;
            }
            else if(!firstPersonInputs.Crouch && isCrouching)
            {
                isCrouching = false;
                transform.localScale = originalScale;
                settings.walkSpeed = originalWalkSpeed;
            }
        }
        #endregion

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

            cameraTargetPitch = ClampAngle(cameraTargetPitch, settings.bottomAngle, settings.topAngle);

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
