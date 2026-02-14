using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Animator _animator;
    [Header("Settings")]
    [SerializeField] private string _speedParam = "horizontalVelocity";
    [SerializeField] private string _airSpeedParam = "verticalVelocity";
    [SerializeField] private string _distanceToGroundParam = "distanceToGround";
    [SerializeField] private string _groundedParam = "isGrounded";

    private int _speedParamHash;
    private int _airSpeedParamHash;
    private int _distanceToGroundParamHash;
    private int _groundParamHash;


    private void Awake()
    {
        _speedParamHash = Animator.StringToHash(_speedParam);
        _airSpeedParamHash = Animator.StringToHash(_airSpeedParam);
        _distanceToGroundParamHash = Animator.StringToHash(_distanceToGroundParam);
        _groundParamHash = Animator.StringToHash(_groundedParam);
    }

    public void UpdateAnimatorClip(float groundSpeed, bool isGrounded, float airSpeed, float distanceToGround)
    {
        _animator.SetFloat(_speedParamHash, groundSpeed);
        _animator.SetFloat(_airSpeedParamHash, airSpeed);
        _animator.SetFloat(_distanceToGroundParamHash, distanceToGround);
        _animator.SetBool(_groundParamHash, isGrounded);
    }
}
