using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
 [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
 public class FirstPersonController : MonoBehaviour
 {
  [Header("Player")]
  [SerializeField] private float walkSpeed = 3f;
  [SerializeField] private float sprintSpeed = 6f;
  [SerializeField] private float rotationSpeed = 1f;
  [SerializeField] private float speedChangeRate = 10f;

  private float currentSpeed;
  private float rotationVelocity;
  private float verticalVelocity;
  private float terminalVelocity = 53f;

  [Header("Jump")]
  [SerializeField] private float jumpHeight = 2f;
  [SerializeField] private float gravity = -12f;
  [SerializeField] private float jumpTimeout = 0.1f;
  [SerializeField] private float fallTimeout = 0.15f;

  private float jumpTimeoutDelta;
  private float fallTimeoutDelta;

  [Header("Ground Check")]
  [SerializeField] private bool isGrounded = true;
  [SerializeField] private float groundOffset = -0.14f;
  [SerializeField] private float groundDistance = 0.5f;
  [SerializeField] private LayerMask groundMask;

  [Header("Camera")]
  [SerializeField] private Camera mainCamera;
  [SerializeField] private Transform head;
  [SerializeField] private float topAngle = 90f;
  [SerializeField] private float bottomAngle = -90f;
  [SerializeField] private float deltaTimeMultiplier = 1.0f;
  private float cameraTargetPitch;
  private const float threshold = 0.01f;

  [SerializeField] private CharacterController controller;
  [SerializeField] private PlayerInput playerInput;
  [SerializeField] private FirstPersonInputs firstPersonInputs;

  private void Start()
  {
   jumpTimeoutDelta = jumpTimeout;
   fallTimeoutDelta = fallTimeout;
  }

  private void Update()
  {
   GroundCheck();
  }

  private void LateUpdate()
  {
   CameraRotation();
  }

  private void GroundCheck() =>
      isGrounded = Physics.CheckSphere(transform.position - new Vector3(0f, groundOffset, 0f), groundDistance, groundMask);

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

   head.transform.localRotation = Quaternion.Euler(cameraTargetPitch, 0.0f, 0.0f);

   transform.Rotate(Vector3.up * rotationVelocity);
  }

  private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
  {
   lfAngle %= 360f;
   return Mathf.Clamp(lfAngle, lfMin, lfMax);
  }
 }
}
