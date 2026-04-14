using System.Collections;
using UnityEngine;

public class Enemy_Bum : NPCbase
{
    [Space(10), Header("BumFields")]
    [Header("AnimatorStringVars")]
    [SerializeField] private string _isPlayerSpottedTrigger;
    [SerializeField] private string _isTrigerredTrigger;
    [SerializeField] private string _isInteractionRadiusTrigger;
    [SerializeField] private string _isIdleTrigger;
    [SerializeField] private string _isStopRunTrigger;
    [Space(5), Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private ScoreComponent _scoreComponent;
    [SerializeField] private Bum_Audio _bumAudio;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Vector2 _attackBox;
    [Space(5), Header("Parameters")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _dealtDamage;

    private Vector3 _startPosition;
    private Vector3 _startScale;
    private Vector3 _targetPosition;

    private int _spottedTriggerHash;
    private int _triggeredTriggerHash;
    private int _interactionRadiusHash;
    private int _idleTriggerHash;
    private int _stopRunHash;
    private bool _isMoving = false;
    

    protected override void Awake()
    {
        base.Awake();
        _startPosition = transform.position;
        _startScale = transform.localScale;

        _healthComponent.OnDeath += DeathBehaviour;
        _healthComponent.OnHealthChanged += TakeDamage;

        SetHashValues();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _healthComponent.OnDeath -= DeathBehaviour;
        _healthComponent.OnHealthChanged -= TakeDamage;
    }

    protected override void Update()
    {
        base.Update();
        print(CurrentState);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_attackPoint.position, _attackBox);
    }

    protected override void ExecuteIdleStateBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            if (Vector2.Distance(transform.position, _startPosition) > 0.1f)
            {
                _targetPosition = _startPosition;
                StartCoroutine(IdleBehaviour());

            }
            if (!_Animator.GetCurrentAnimatorStateInfo(0).IsName("Bum_Idle"))
                _Animator.SetTrigger(_isIdleTrigger);
        }
    }
    protected override void ExecutePlayerSpottedBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            _Animator.SetTrigger(_spottedTriggerHash);
        }
    }
    protected override void ExecuteTriggeredBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            _targetPosition = _Player.position;

            StartCoroutine(MoveBumToTarget(_targetPosition));
        }
    }
    protected override void ExecuteInteractionBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            StartCoroutine(AttackFunc()); 
        }
    }

    private IEnumerator IdleBehaviour()
    {
        StartCoroutine(MoveBumToTarget(_targetPosition));
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        yield return new WaitUntil(() => _IsRunning == false);
        _Animator.SetTrigger(_idleTriggerHash);

        transform.position = _startPosition;
        transform.localScale = _startScale;
    }

    private IEnumerator MoveBumToTarget(Vector3 targetPosition)
    {
        _Animator.ResetTrigger(_spottedTriggerHash);
        _isMoving = true;
        _Animator.SetTrigger(_triggeredTriggerHash);
        _IsRunning = true;
        yield return new WaitUntil(() => _Animator.GetCurrentAnimatorStateInfo(0).IsName("Bum_Run"));
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f && _isMoving)
        {
            
            Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPosition);
            yield return null;
        }
        _isMoving = false;
        _Animator.SetTrigger(_stopRunHash);
        _IsRunning = false;
    }

    private IEnumerator AttackFunc()
    {
        StopMoving();
        yield return new WaitUntil(() => _IsRunning == false);
        //в аниматоре метод DealDamage
        _IsAutoStateDetection = false;
        _Animator.SetTrigger(_interactionRadiusHash);
    }

    private void TakeDamage(float damage)
    {
        StartCoroutine(SpriteDamagedColorChanger.FlashSprite(_spriteRenderer));
        _bumAudio.PlayHurtClip();
        if (CurrentState == NPC_States.PlayerSpotted || CurrentState == NPC_States.Idle)
        {
            _Animator.SetTrigger(_triggeredTriggerHash);
            ChangeBehaviourState(NPC_States.Triggered);
        }
        
        print($"Enemy_Bum health: {_healthComponent.CurrentHealth}/{_healthComponent.MaxHealth}");
    }

    private void DeathBehaviour()
    {
        StopAllCoroutines();
        _Animator.SetTrigger("isDead");
        _healthComponent.enabled = false;
        _collider.excludeLayers += LayerMask.GetMask("Bullet", "Player");
        ScoreHandler.Instance.AddScore(_scoreComponent.ScoreValue);
        Destroy(gameObject, 2f);
    }

    protected void DealDamage()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_attackPoint.position, _attackBox, 0, transform.right, _attackBox.x);
        if (hit != false)
        {
            print(hit.transform.gameObject.name);
            AudioHandler.Instance.PlaySfx(_bumAudio.HitClip);
            if (hit.transform.TryGetComponent<HealthComponent>(out var component))
            {
                if (component.CompareTag("Player"))
                {
                    component.TakeDamage(_dealtDamage);
                }
            }
        }
        else { AudioHandler.Instance.PlaySfx(_bumAudio.AttackMissClip); }
        if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Bum_Attack3"))
        {
            CheckState();
            if (CurrentState == NPC_States.Interaction)
            {
                _Animator.SetTrigger(_interactionRadiusHash);
            }
            else
            {
                _Animator.SetTrigger("isPlayerLost");
                _IsAutoStateDetection = true;
            }
        }
    }

    private void StopMoving()
    {
        _isMoving = false;
    }

    private void SetHashValues()
    {
        _spottedTriggerHash = Animator.StringToHash(_isPlayerSpottedTrigger);
        _triggeredTriggerHash = Animator.StringToHash(_isTrigerredTrigger);
        _interactionRadiusHash = Animator.StringToHash(_isInteractionRadiusTrigger);
        _idleTriggerHash = Animator.StringToHash(_isIdleTrigger);
        _stopRunHash = Animator.StringToHash(_isStopRunTrigger);
    }
}
