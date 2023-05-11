using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSleepyKoala.Essentials.FirstPersonController
{
    [CreateAssetMenu(fileName = "FirstPersonControllerSettings", menuName = "TheSleepyKoala/Essentials/FirstPersonControllerSettings")]
    public class FirstPersonControllerSettings : ScriptableObject
    {
        #region Ground Check Variables
        [Header("Ground Check Settings")]
        [Tooltip("The layer mask that determines what is considered ground.")]
        public LayerMask groundMask;
        #endregion

        #region Jump Variables
        [Header("Jump Settings")]
        [Tooltip("The boolean that determines if the player can jump or not.")]
        public bool enableJump = true;
        [Tooltip("The force applied to the player when jumping.")]
        public float jumpForce = 5f;
        #endregion

        #region Movement Variables
        [Header("Movement Settings")]
        [Tooltip("The boolean that determines if the player can move or not.")]
        public bool enableMovement = true;
        [Tooltip("The walking speed of the player.")]
        public float walkSpeed = 5f;
        //make space
        [Space (10)]
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
        public float crouchHeight = 0.5f;
        [Tooltip("The speed modifier of the player when crouching.")]
        public float crouchSpeedModifier = 0.5f;
        #endregion

        #region Player Camera Variables
        [Header("Player Camera Settings")]
        [Tooltip("The maximum angle the camera can look up.")]
        public float topAngle = 90f;
        [Tooltip("The maximum angle the camera can look down.")]
        public float bottomAngle = -90f;
        [Tooltip("The speed at which the camera rotates.")]
        public float rotationSpeed = 1f;
        [Tooltip("The multiplier for the deltaTime value used in camera rotation.")]
        public float deltaTimeMultiplier = 1f;
        #endregion
    }
}
