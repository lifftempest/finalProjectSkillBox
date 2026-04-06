using System.Collections;
using UnityEngine;

public class Enemy_Robot : NPCbase
{
    [Space(10), Header("RobotFields")]
    [Header("AnimatorStringVars")]
    [SerializeField] private string _idleTriggerName;
    [SerializeField] private string _spottedTriggerName;
    [SerializeField] private string _trigerredTriggerName;
    [SerializeField] private string _interactionTriggerName;
    [Space(5), Header("Components")]
    [SerializeField] private ScoreComponent _scoreComponent;
    [SerializeField] private Collider2D _robotCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] private Transform _firePoint;
    [Space(5), Header("Parameters")]
    [SerializeField] private float _fireRate;

    private int _idleTriggerHash;
    private int _spottedTriggerHash;
    private int _trigerredTriggerHash;
    private int _interactionTriggerHash;

    private float _currentFireRate;

    protected override void Awake()
    {
        base.Awake();

        SetHashVarsValue();

        _healthComponent.OnDeath += DeathBehaviour;
        _healthComponent.OnHealthChanged += TakeDamage;
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

        if (_currentFireRate > 0)
        {
            _currentFireRate -= Time.deltaTime;
        }
    }

    protected override void ExecuteIdleStateBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            if (!_Animator.GetCurrentAnimatorStateInfo(0).IsName("Robot_Idle"))
            {
                _Animator.SetTrigger(_idleTriggerHash);
            }
        }
        
    }
    protected override void ExecutePlayerSpottedBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Robot_WaitShoot"))
            {
                _Animator.SetTrigger(_trigerredTriggerHash);
            }
            if (!_Animator.GetCurrentAnimatorStateInfo(0).IsName("Robot_WaitState") && LastState == NPC_States.Idle)
            {
                _Animator.SetTrigger(_spottedTriggerHash);
            }
        }
        
    }
    protected override void ExecuteTriggeredBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            if (!_Animator.GetCurrentAnimatorStateInfo(0).IsName("Robot_WaitShoot"))
            {
                _Animator.SetTrigger(_trigerredTriggerHash);
            }
        }
        
    }
    protected override void ExecuteInteractionBehaviour()
    {
        if (_healthComponent.IsAlive)
        {
            StartCoroutine(Fire());
        }
    }

    private void TakeDamage(float damage)
    {
        StartCoroutine(SpriteDamagedColorChanger.FlashSprite(_spriteRenderer));
    }

    private IEnumerator Fire()
    {
        while (CurrentState == NPC_States.Interaction)
        {
            if (_currentFireRate <= 0)
            {
                _Animator.SetTrigger(_interactionTriggerHash);
                var bullet = _bulletPool.GetBullet();
                bullet.gameObject.transform.position = _firePoint.position;
                bullet.transform.parent = null;
                bullet.SetActive(true);
                bullet.GetComponent<Robot_Bullet>().FireBullet(_firePoint);
                _currentFireRate = _fireRate;
                yield return null;
            }
            else
            {
                yield return null;
            }
        }
        yield return null;
    }

    private void DeathBehaviour()
    {
        StopAllCoroutines();
        _Animator.SetTrigger("isDead");
        _healthComponent.enabled = false;
        _robotCollider.excludeLayers += LayerMask.GetMask("Bullet", "Player");
        ScoreHandler.Instance.AddScore(_scoreComponent.ScoreValue);
        Destroy(gameObject, 2f);
    }

    private void SetHashVarsValue()
    {
        _idleTriggerHash = Animator.StringToHash(_idleTriggerName);
        _spottedTriggerHash = Animator.StringToHash(_spottedTriggerName);
        _trigerredTriggerHash = Animator.StringToHash(_trigerredTriggerName);
        _interactionTriggerHash = Animator.StringToHash(_interactionTriggerName);
    }
}
