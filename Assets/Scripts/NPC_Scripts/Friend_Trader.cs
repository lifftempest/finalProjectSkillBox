using UnityEngine;

public class Friend_Trader : NPCbase, IInteractable
{
    [Space(10), Header("TraderFields")]
    [Header("AnimatorStringVars")]
    [SerializeField] private string _idleTriggerName;
    [SerializeField] private string _spottedTriggerName;
    [SerializeField] private string _triggerTriggerName;
    [SerializeField] private string _interactionTriggerName;
    [Space(5), Header("Components")]
    [Space(5), Header("Parameters")]
    [SerializeField] private string _interactionPrompt;
    [SerializeField] private int _scoreCost;

    private int _idleTriggerHash;
    private int _spottedTriggerHash;
    private int _triggerTriggerHash;
    private int _interactionTriggerHash;

    private bool _isInteractionRadius;

    public string InteractionPrompt => _interactionPrompt;

    protected override void Awake()
    {
        base.Awake();

        SetHashValues();
    }

    protected override void ExecuteIdleStateBehaviour()
    {
        _Animator.SetTrigger(_idleTriggerHash);
    }
    protected override void ExecutePlayerSpottedBehaviour()
    {
        _Animator.SetTrigger(_spottedTriggerHash);
    }
    protected override void ExecuteTriggeredBehaviour()
    {
        if (!_Animator.GetCurrentAnimatorStateInfo(0).IsName("Trader_UseReady"))
        {
            _Animator.SetTrigger(_triggerTriggerHash);
        }
        _isInteractionRadius = false;
    }
    protected override void ExecuteInteractionBehaviour()
    {
        _isInteractionRadius = true;
    }

    public void Interact()
    {
        if (_isInteractionRadius)
        {
            var hp = _Player.GetComponentInParent<HealthComponent>();
            var ammo = _Player.GetComponentInParent<AttackerHandler>();

            if (hp.CurrentHealth < hp.MaxHealth || ammo.CurrentMagazine != ammo.MaxMagazine)
            {
                hp.Heal();
                ammo.ReloadMagazine();
                ScoreHandler.Instance.AddScore(-1 * _scoreCost);
                _Animator.SetTrigger(_interactionTriggerHash);
            }
        }
    }

    private void SetHashValues()
    {
        _idleTriggerHash = Animator.StringToHash(_idleTriggerName);
        _spottedTriggerHash = Animator.StringToHash(_spottedTriggerName);
        _triggerTriggerHash = Animator.StringToHash(_triggerTriggerName);
        _interactionTriggerHash = Animator.StringToHash(_interactionTriggerName);
    }
}
