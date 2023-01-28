using System;
using SS3D.Core;
using SS3D.Core.Behaviours;
using SS3D.Systems.Health;
using SS3D.Systems.InputHandling;
using SS3D.Systems.Screens;
using UnityEngine;

namespace SS3D.Systems.Entities.Humanoid
{
    /// <summary>
    /// Controls the movement for biped characters that use the same armature
    /// as the human model uses.
    /// </summary>
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(HumanoidAnimatorController))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(StaminaController))]
    public class HumanoidController : NetworkActor
    {
        public event Action<float> OnSpeedChanged;

        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Entity _entity;

        [Header("Movement Settings")]
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _lerpMultiplier;
        [SerializeField] private float _rotationLerpMultiplier;

        [SerializeField] private StaminaController _staminaController;

        [Header("Movement IK Targets")]
        [SerializeField] private Transform _movementTarget;

        [Header("Run/Walk")]
        private bool _isRunning;

        [Header("Debug Info")]
        private Vector3 _absoluteMovement;
        private Vector2 _input;
        private Vector2 _smoothedInput;

        private Vector3 _targetMovement;

        private float _smoothedX;
        private float _smoothedY;
        private Actor _camera;

        private const float WalkAnimatorValue = .3f;
        private const float RunAnimatorValue = 1f;

        protected override void OnStart()
        {
            base.OnStart();

            Setup();
        }

        private void Setup()
        {
            _camera = SystemLocator.Get<CameraSystem>().PlayerCamera;

            _entity.OnMindChanged += HandleControllingSoulChanged;
        }

        private void HandleControllingSoulChanged(Mind mind)
        {
            OnSpeedChanged?.Invoke(0);
        }

        protected override void HandleUpdate(in float deltaTime)
        {
            base.HandleUpdate(in deltaTime);

            if (!IsOwner)
            {
                return;
            }

            ProcessCharacterMovement();
        }

        /// <summary>
        /// Executes the movement code and updates the IK targets
        /// </summary>
        private void ProcessCharacterMovement()
        {
            ProcessToggleRun();
            ProcessPlayerInput();

            _characterController.Move(Physics.gravity);

            if (_input.magnitude != 0)
            {
                MoveMovementTarget(_input);
                RotatePlayerToMovement();
                MovePlayer();
            }
            else
            {
                MovePlayer();
                MoveMovementTarget(Vector2.zero, 5);
            }
        }

        /// <summary>
        /// Moves the movement targets with the given input
        /// </summary>
        /// <param name="movementInput"></param>
        /// <param name="multiplier"></param>
        private void MoveMovementTarget(Vector2 movementInput, float multiplier = 1)
         {
             //makes the movement align to the camera view
             Vector3 newTargetMovement =
                 movementInput.y * Vector3.Cross(_camera.Right, Vector3.up).normalized +
                 movementInput.x * Vector3.Cross(Vector3.up, _camera.Forward).normalized;

             // smoothly changes the target movement
             _targetMovement = Vector3.Lerp(_targetMovement, newTargetMovement, Time.deltaTime * (_lerpMultiplier * multiplier));

             Vector3 resultingMovement = _targetMovement + Position;
            _absoluteMovement = resultingMovement;

             _movementTarget.position = _absoluteMovement;
         }

        /// <summary>
        /// Rotates the player to the target movement
        /// </summary>
        private void RotatePlayerToMovement()
        {
            Quaternion lookRotation = Quaternion.LookRotation(_targetMovement);

            transform.rotation =
                Quaternion.Slerp(Rotation, lookRotation, Time.deltaTime * _rotationLerpMultiplier);
        }

        /// <summary>
        /// Moves the player to the target movement
        /// </summary>
        private void MovePlayer()
        {
            _characterController.Move(_targetMovement * ((_movementSpeed) * Time.deltaTime));
        }

        /// <summary>
        /// Process the player movement input, smoothing it
        /// </summary>
        /// <returns></returns>
        private void ProcessPlayerInput()
        {
            float x = UserInput.GetAxisRaw("Horizontal");
            float y = UserInput.GetAxisRaw("Vertical");

            float inputFilteredSpeed = _isRunning && _staminaController.CanContinueInteraction ? RunAnimatorValue : WalkAnimatorValue;

            x = Mathf.Clamp(x, -inputFilteredSpeed, inputFilteredSpeed);
            y = Mathf.Clamp(y, -inputFilteredSpeed, inputFilteredSpeed);

            _input = new Vector2(x, y);
            _smoothedInput = Vector2.Lerp(_smoothedInput, _input, Time.deltaTime * (_lerpMultiplier / 10));

            OnSpeedChanged?.Invoke(_input.magnitude != 0 ? inputFilteredSpeed : 0);
        }

        /// <summary>
        /// Toggles your movement between run/walk
        /// </summary>
        private void ProcessToggleRun()
        {
            if (UserInput.GetButtonDown("Toggle Run"))
            {
                _isRunning = !_isRunning;
            }
        }
    }

}