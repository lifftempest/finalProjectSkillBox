using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidBody;
    [Header("MoveSettings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _groundDeceleration;
    [SerializeField] private float _airDeceleration;
    [Header("JumpSettings")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _coyoteTime;
    [SerializeField] private float _jumpBufferTime;
    [Space(3)]
    [SerializeField] private float _gravityScale;
    [SerializeField] private float _fallGravityMultiplier;
    [Space(5)]
    [SerializeField] private Transform _groundPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckDistance;


    private bool _isGrounded;
    private float _coyoteTimeCounter;
    private float _jumpBufferTimeCounter;

    public float HorizontalMoveSpeed => _rigidBody.linearVelocityX;
    public float VerticalMoveSpeed => _rigidBody.linearVelocityY;
    public float AbsHorizontalMS => Mathf.Abs(HorizontalMoveSpeed);
    public float DistanceToGround { get; private set; }
    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _rigidBody.gravityScale = _gravityScale;
    }

    private void Update()
    {
        CheckGround();
        CheckCoyoteTime();
        CheckDistanceToGround();
    }

    private void FixedUpdate()
    {
        HandleFallGravity();
    }

    public void HandleMovement(float inputValue)
    {
        float targetSpeed = inputValue * _moveSpeed;
        float speedDifference = targetSpeed - _rigidBody.linearVelocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration :
            (_isGrounded ? _groundDeceleration : _airDeceleration);
        float movement = speedDifference * accelerationRate;

        _rigidBody.AddForce(Vector2.right * movement, ForceMode2D.Force);

        if (Mathf.Abs(_rigidBody.linearVelocity.x) > _maxSpeed)
        {
            _rigidBody.linearVelocity = new Vector2(Mathf.Sign(
                _rigidBody.linearVelocity.x) * _maxSpeed, 
                _rigidBody.linearVelocity.y);
        }
    }

    public void HandleJump(bool inputJump)
    {
        if (inputJump && _isGrounded)
        {
            ExecuteJump();
        }
        else if (inputJump && !_isGrounded && _coyoteTimeCounter > 0)
        {
            ExecuteJump();
            _coyoteTimeCounter = 0;
        }
        else if (_isGrounded && _jumpBufferTimeCounter > 0)
        {
            ExecuteJump();
            _jumpBufferTimeCounter = 0;
        }
    }

    private void ExecuteJump()
    {
        _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, 0f);
        _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }
    /// <summary>
    /// Для получения урона от огня
    /// </summary>
    /// <param name="firePoisiton"></param>
    public void PushPlayer(Transform firePoisiton, float lauchAngle, float launchPower)
    {
        Vector2 horizontalDirection = (transform.position - firePoisiton.position).normalized;
        var angle = lauchAngle * Mathf.Deg2Rad;
        Vector2 launchVelocity = horizontalDirection * launchPower * Mathf.Cos(angle);
        launchVelocity.y = launchPower * Mathf.Sin(angle);
        _rigidBody.linearVelocity = Vector2.zero;
        _rigidBody.linearVelocity = launchVelocity;
    }

    private void HandleFallGravity()
    {
        if (!_isGrounded && _rigidBody.linearVelocity.y < 0)
        {
            _rigidBody.gravityScale = _gravityScale * _fallGravityMultiplier;
        }
        else
        {
            _rigidBody.gravityScale = _gravityScale;
        }
    }

    private void CheckDistanceToGround()
    {
        if (!_isGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(
            _groundPoint.position,
            Vector2.down,
            1000f,
            _groundLayer);

            DistanceToGround = hit.distance;
        }
        else
        {
            DistanceToGround = 0;
        }
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            _groundPoint.position,
            _groundCheckSize,
            0f,
            Vector2.down,
            _groundCheckDistance,
            _groundLayer);

        _isGrounded = hit.collider != null;
    }

    private void CheckCoyoteTime()
    {
        if (_isGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            if (_coyoteTimeCounter > 0)
            {
                _coyoteTimeCounter -= Time.deltaTime;
            }
        }
    }

    public void CheckJumpBufferTime(bool inputJump)
    {
        if (!_isGrounded && inputJump)
        {
            _jumpBufferTimeCounter = _jumpBufferTime;
        }
        else
        {
            if (_jumpBufferTimeCounter > 0)
            {
                _jumpBufferTimeCounter -= Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundPoint != null)
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(_groundPoint.position, _groundCheckSize);
        }
    }
}
