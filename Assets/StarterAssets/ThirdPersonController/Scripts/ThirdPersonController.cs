using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))] 
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        // --- ÉVÉNEMENTS ET LOCKS ---
        public static System.Action OnPlayerRespawnEvent; 
        [Header("Locks")]
        public bool canThrowGrenade = true;
        public bool canUseParachute = true;

        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        
        [Header("Crouch Settings")]
        public float CrouchSpeed = 1.5f;
        public float CrouchHeight = 1.0f;
        public float NormalHeight = 2.0f;

        [Header("Gliding Settings")]
        public float GlideGravity = -1.5f; 
        public GameObject UmbrellaObject; 
        
        [Header("Audio Custom (Sons 2D)")]
        public AudioClip umbrellaOpenSound;     
        public AudioClip umbrellaLandingSound; 
        public AudioClip respawnSound;

        [Header("Ice Mechanic")]
        public GameObject iceVisualL; 
        public GameObject iceVisualR; 
        [Tooltip("Vitesse de l'animation sur la glace (0.5 = patinage)")]
        public float IceAnimationSpeed = 0.5f; 
        private bool _isOnIce = false;

        [Header("Space Mechanic")]
        public float SpaceGravity = -2.0f; 
        public float SpaceJumpHeight = 4.0f; 
        private float _normalGravity;
        private float _normalJumpHeight;
        private bool _isInSpace = false;

        [Header("Checkpoint System")]
        private Vector3 _respawnPosition; 

        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;

        [Space(10)]
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;
        public float CameraAngleOverride = 0.0f;
        public bool LockCameraPosition = false;

        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private Vector3 _impactVelocity; 
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDCrouch;
        private int _animIDGliding;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private AudioSource _audioSource; 
        private const float _threshold = 0.01f;
        private bool _hasAnimator;

        public bool IsGliding { get; private set; }
        private bool _hasPlayedOpenSound = false; 
        private bool _wasGlidingBeforeLanding = false; 

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (_mainCamera == null) _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _audioSource = GetComponent<AudioSource>(); 
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#endif
            AssignAnimationIDs();
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            _respawnPosition = transform.position;

            _normalGravity = Gravity;
            _normalJumpHeight = JumpHeight;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            
            HandleCrouch();
            JumpAndGravity();
            GroundedCheck();
            Move();

            // Note: Si tu as un script de Grenade séparé, assure-toi qu'il vérifie 
            // la variable 'canThrowGrenade' de ce controller avant de lancer.
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDCrouch = Animator.StringToHash("Crouch");
            _animIDGliding = Animator.StringToHash("isGliding");
        }

        private void HandleCrouch()
        {
            if (_input.crouch)
            {
                _controller.height = CrouchHeight;
                _controller.center = new Vector3(0, CrouchHeight / 2f, 0);
            }
            else
            {
                _controller.height = NormalHeight;
                _controller.center = new Vector3(0, NormalHeight / 2f, 0); 
            }

            if (_hasAnimator) _animator.SetBool(_animIDCrouch, _input.crouch);
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
            if (_hasAnimator) _animator.SetBool(_animIDGrounded, Grounded);
        }

        private void CameraRotation()
        {
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (_input.crouch) targetSpeed = CrouchSpeed;
            if (IsGliding) targetSpeed *= 1.2f;
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            float currentAcceleration = _isOnIce ? 0.5f : SpeedChangeRate;

            if (currentHorizontalSpeed < targetSpeed - 0.1f || currentHorizontalSpeed > targetSpeed + 0.1f)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * currentAcceleration);
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * currentAcceleration);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float currentRotSmooth = _isOnIce ? 0.4f : RotationSmoothTime;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, currentRotSmooth);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            Vector3 movement = targetDirection.normalized * _speed + _impactVelocity;
            _controller.Move(movement * Time.deltaTime + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            _impactVelocity = Vector3.Lerp(_impactVelocity, Vector3.zero, Time.deltaTime * 5f);

            if (_hasAnimator)
            {
                float animModifier = _isOnIce ? IceAnimationSpeed : 1.0f;
                _animator.SetFloat(_animIDSpeed, _animationBlend * animModifier);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                if (_wasGlidingBeforeLanding)
                {
                    if (_audioSource != null && umbrellaLandingSound != null) _audioSource.PlayOneShot(umbrellaLandingSound);
                    _wasGlidingBeforeLanding = false; 
                }

                _fallTimeoutDelta = FallTimeout;
                IsGliding = false; 
                _hasPlayedOpenSound = false; 

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                    _animator.SetBool(_animIDGliding, false);
                }

                if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;

                if (_input.jump && _jumpTimeoutDelta <= 0.0f && !_input.crouch)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    if (_hasAnimator) _animator.SetBool(_animIDJump, true);
                }

                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
                if(UmbrellaObject != null && UmbrellaObject.activeSelf) UmbrellaObject.SetActive(false);
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;
                if (_fallTimeoutDelta >= 0.0f) _fallTimeoutDelta -= Time.deltaTime;
                else if (_hasAnimator) _animator.SetBool(_animIDFreeFall, true);

                _input.jump = false;
                
                // AJOUT DU LOCK : On ne peut planer que si canUseParachute est vrai
                IsGliding = canUseParachute && _input.parachute && _verticalVelocity < 0;

                if (IsGliding) 
                {
                    _wasGlidingBeforeLanding = true;
                    if (!_hasPlayedOpenSound)
                    {
                        if (_audioSource != null && umbrellaOpenSound != null) _audioSource.PlayOneShot(umbrellaOpenSound);
                        _hasPlayedOpenSound = true;
                    }
                    if (_verticalVelocity < GlideGravity) _verticalVelocity = GlideGravity;
                    _verticalVelocity += GlideGravity * Time.deltaTime;
                    if(UmbrellaObject != null && !UmbrellaObject.activeSelf) UmbrellaObject.SetActive(true);
                }
                else 
                {
                    _hasPlayedOpenSound = false; 
                    if (_verticalVelocity < _terminalVelocity) _verticalVelocity += Gravity * Time.deltaTime;
                    if(UmbrellaObject != null && UmbrellaObject.activeSelf) UmbrellaObject.SetActive(false);
                }

                if (_hasAnimator) _animator.SetBool(_animIDGliding, IsGliding);
            }
        }

        public void ActivateIce() { 
            _isOnIce = true; 
            if (iceVisualL != null) iceVisualL.SetActive(true); 
            if (iceVisualR != null) iceVisualR.SetActive(true); 
        }
        
        public void ClearIce() { 
            _isOnIce = false; 
            if (iceVisualL != null) iceVisualL.SetActive(false); 
            if (iceVisualR != null) iceVisualR.SetActive(false); 
        }

        public void ActivateSpace() { 
            _isInSpace = true; 
            Gravity = SpaceGravity; 
            JumpHeight = SpaceJumpHeight; 
        }

        public void ClearSpace() { 
            _isInSpace = false; 
            Gravity = _normalGravity; 
            JumpHeight = _normalJumpHeight; 
        }

        public void LaunchPlayer(Vector3 force) { _impactVelocity += force; _verticalVelocity = force.y; }
        public void SetCheckpoint(Vector3 newPos) { _respawnPosition = newPos; }

        public void Respawn()
        {
            if (_audioSource != null && respawnSound != null) _audioSource.PlayOneShot(respawnSound);
            _controller.enabled = false;
            transform.position = _respawnPosition;
            _verticalVelocity = 0;
            _impactVelocity = Vector3.zero;
            _controller.enabled = true;

            // --- DECLENCHE L'ALERTE POUR LE LABYRINTHE ET LES LOCKS ---
            OnPlayerRespawnEvent?.Invoke();

            SendMessage("OnPlayerRespawn", SendMessageOptions.DontRequireReceiver);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}