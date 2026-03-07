using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementHandler _movementHandler;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private AnimatorHandler _animatorHandler;
    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private AttackerHandler _attackerHandler;
    [Space(5)]
    [SerializeField] private Transform _playerTransform;

    private float _moveInput;
    private bool _jumpInput;
    private float _horizontalMoveSpeed;
    private float _absHorizontalMoveSpeed;
    private float _verticalMoveSpeed;
    private bool _isGrounded;
    private float _distanceToGround;

    private void Awake()
    {
        _healthComponent.OnDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        _healthComponent.OnDeath -= PlayerDeath;
    }

    private void Update()
    {
        if (_healthComponent.IsAlive)
        {
            SetVariablesValue();

            Jump();
            UpdatePlayerDirection();

            OnlyGroundedActions();

            _animatorHandler.UpdateAnimatorClip(_absHorizontalMoveSpeed, _isGrounded, _verticalMoveSpeed, _distanceToGround);
        }
    }

    private void FixedUpdate()
    {
        if (_healthComponent.IsAlive)
        {
            _movementHandler.HandleMovement(_moveInput);
        }
    }

    private void Jump()
    {
        _movementHandler.HandleJump(_jumpInput);
        _movementHandler.CheckJumpBufferTime(_jumpInput);
    }

    private void UpdatePlayerDirection()
    {
        if (_horizontalMoveSpeed > 0.1)
        {
            _playerTransform.localScale = new Vector2(1, _playerTransform.localScale.y);
        }
        else if (_horizontalMoveSpeed < -0.1)
        {
            _playerTransform.localScale = new Vector2(-1, _playerTransform.localScale.y);
        }
    }

    private void OnlyGroundedActions()
    {
        if (_isGrounded)
        {
            _attackerHandler.Shoot(_inputHandler.AttackButtonPressed);
            _interactionHandler.TryInteract(_inputHandler.InteractionPressed);
        }
    }

    private void SetVariablesValue()
    {
        _moveInput = _inputHandler.HorizontalInput;
        _jumpInput = _inputHandler.JumpPressed;

        _horizontalMoveSpeed = _movementHandler.HorizontalMoveSpeed;
        _absHorizontalMoveSpeed = _movementHandler.AbsHorizontalMS;
        _verticalMoveSpeed = _movementHandler.VerticalMoveSpeed;
        _distanceToGround = _movementHandler.DistanceToGround;
        _isGrounded = _movementHandler.IsGrounded;
    }

    private void PlayerDeath()
    {
        print("DEAD");
        _movementHandler.enabled = false;
        _inputHandler.enabled = false;
        _interactionHandler.enabled = false;
        _healthComponent.enabled = false;
        _animatorHandler.SetDeathTrigger();
        EventManager.InvokePlayerDeathEvents();
    }
}
