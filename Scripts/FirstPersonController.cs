using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
 ///<summary>
 ///A script that handles the movement and camera control of a first person character.
 ///</summary>
 [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
 public class FirstPersonController : MonoBehaviour
 {
  #region Player Movement
  [Header("Player")]
  [SerializeField, Tooltip("The walking speed of the player.")]
  private float walkSpeed = 3f;
  [SerializeField, Tooltip("The sprinting speed of the player.")]
  private float sprintSpeed = 6f;
  [SerializeField, Tooltip("The rate at which the player's speed changes.")]
  private float speedChangeRate = 10f;
  private float currentSpeed;
  
  /// <summary>
  /// Moves the player based on the player's input.
  /// </summary>
  private void MovePlayer()
  {

  }
  #endregion

  #region Gravity
  [SerializeField, Tooltip("The gravity affecting the player.")]
  private float gravity = -12f;

  /// <summary>
  /// Applies gravity to the character controller's vertical velocity.
  /// </summary>
  private void ApplyGravity()
  {
   if (controller.isGrounded)
   {
    // If the controller is on the ground, reset the vertical velocity
    // to prevent accumulating gravity over multiple frames.
    Vector3 verticalVelocity = Vector3.down * controller.velocity.y;
    controller.Move(verticalVelocity * Time.deltaTime);
   }
   else
   {
    // Apply gravity to the controller's vertical velocity.
    Vector3 gravityVector = Vector3.up * gravity;
    controller.Move(gravityVector * Time.deltaTime);
   }
  }
  #endregion

  #region Player Actions
  [Header("Jump")]
  [SerializeField, Tooltip("The height the player can jump.")]
  private float jumpHeight = 2f;
  [SerializeField, Tooltip("The time the player has to jump after leaving the ground.")]
  private float jumpTimeout = 0.1f;
  [SerializeField, Tooltip("The time the player has to land after jumping.")]
  private float fallTimeout = 0.15f;
  private float rotationVelocity;
  private float jumpTimeoutDelta;
  private float fallTimeoutDelta;
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

  #region Components
  [SerializeField, Tooltip("The character controller component attached to the player.")]
  private CharacterController controller;
  [SerializeField, Tooltip("The player input script attached to the player.")]
  private PlayerInput playerInput;
  [SerializeField, Tooltip("The first person inputs script attached to the player.")]
  private FirstPersonInputs firstPersonInputs;
  #endregion

  private void Start()
  {
   jumpTimeoutDelta = jumpTimeout;
   fallTimeoutDelta = fallTimeout;
  }

  private void Update()
  {
   ApplyGravity();
  }

  private void LateUpdate()
  {
   CameraRotation();
  }
 }
}
