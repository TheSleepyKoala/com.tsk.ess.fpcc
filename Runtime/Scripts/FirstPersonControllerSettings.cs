using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
    [CreateAssetMenu(
        fileName = "FirstPersonControllerSettings",
        menuName = "TheSleepyKoala/Essentials/FirstPersonControllerSettings"
    )]
    public class FirstPersonControllerSettings : ScriptableObject
    {
        #region Ground Check Variables
        [Header("Ground Check Settings")]
        [Tooltip("The layer mask that determines what is considered ground.")]
        public LayerMask groundMask;

        [Tooltip("The distance to check for ground.")]
        public float groundCheckDistance = 0.75f;

        [Tooltip("The time before the player is considered not grounded.")]
        public float timeBeforeNotGrounded = 0.3f;

        [Space(10)]
        [Tooltip("The maximum slope angle the player can walk on.")]
        public float maxSlopeAngle = 45f;

        [Tooltip(
            "The multiplier that determines how much the player's speed is reduced when walking on a slope."
        )]
        public float slopeMultiplier = 0.8f;
        #endregion

        #region Jump Variables
        [Header("Jump Settings")]
        [Tooltip("The boolean that determines if the player can jump or not.")]
        public bool enableJump = true;

        [Tooltip("The force applied to the player when jumping.")]
        public float jumpForce = 5f;

        [Tooltip("The cooldown between jumps.")]
        public float jumpCooldown = 0.3f;
        #endregion

        #region Movement Variables
        [Header("Movement Settings")]
        [Tooltip("The boolean that determines if the player can move or not.")]
        public bool enableMovement = true;

        [Tooltip("The walking speed of the player.")]
        public float walkSpeed = 5f;

        [Space(10)]
        [Tooltip("The boolean that determines if the player can sprint or not.")]
        public bool enableSprint = true;

        [Tooltip("The sprinting speed of the player.")]
        public float sprintSpeed = 10f;

        [Tooltip("The maximum velocity change of the player.")]
        public float maxVelocityChange = 10f;
        #endregion

        #region Crouch Variables
        [Header("Crouch Settings")]
        [Tooltip("The boolean that determines if the player can crouch or not.")]
        public bool enableCrouch = true;

        [Tooltip("The height of the player when crouching.")]
        public float crouchScaleHeight = 0.5f;

        [Tooltip("The speed modifier of the player when crouching.")]
        public float crouchSpeedModifier = 0.5f;
        #endregion

        #region Player Camera Variables
        [Header("Player Camera Settings")]
        [Tooltip("The maximum angle the camera can look up.")]
        public float topAngle = 90f;

        [Tooltip("The maximum angle the camera can look down.")]
        public float bottomAngle = -90f;

        [Tooltip("The multiplier for the deltaTime value used in camera rotation.")]
        public float mouseSensitivity = 2f;
        #endregion
    }
}
