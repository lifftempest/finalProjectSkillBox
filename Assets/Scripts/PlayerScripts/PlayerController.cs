using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MovementHandler _movementHandler;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private AnimatorHandler _animatorHandler;
    [Space(5)]
    [SerializeField] private Transform _playerTransform;

    private float _moveInput;
    private bool _jumpInput;
    private float _horizontalMoveSpeed;
    private float _absHorizontalMoveSpeed;
    private float _verticalMoveSpeed;
    private bool _isGrounded;
    private float _distanceToGround;

    private void Update()
    {
        _moveInput = _inputHandler.HorizontalInput;
        _jumpInput = _inputHandler.JumpPressed;

        _horizontalMoveSpeed = _movementHandler.HorizontalMoveSpeed;
        _absHorizontalMoveSpeed = _movementHandler.AbsHorizontalMS;
        _verticalMoveSpeed = _movementHandler.VerticalMoveSpeed;
        _distanceToGround = _movementHandler.DistanceToGround;
        _isGrounded = _movementHandler.IsGrounded;

        Jump();
        UpdatePlayerDirection();

        _animatorHandler.UpdateAnimatorClip(_absHorizontalMoveSpeed, _isGrounded, _verticalMoveSpeed, _distanceToGround);
    }

    private void FixedUpdate()
    {
        _movementHandler.HandleMovement(_moveInput);
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
}
