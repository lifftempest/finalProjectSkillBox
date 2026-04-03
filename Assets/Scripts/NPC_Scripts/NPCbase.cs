using System;
using UnityEngine;

public abstract class NPCbase : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected Animator _Animator;
    [Space(10), Header("Parameters")]
    [SerializeField] private float _distanceToSpottedState;
    [SerializeField] private float _distanceToTriggeredState;
    [SerializeField] private float _distanceToInteractionState;

    private Vector2 _directionToPlayer;
    private float _angleToPlayer;
    private float _distanceToPlayer;
    protected Transform _Player;
    protected bool _IsAutoStateDetection = true;
    protected bool _IsRunning = false;

    protected Action<NPC_States> OnStateChanged;

    protected NPC_States LastState { get; private set; }
    protected NPC_States CurrentState { get; private set; }

    protected virtual void Awake()
    {
        OnStateChanged += DetermineStateBehaviour;
        _Player = FindAnyObjectByType<PlayerController>().GetComponent<Transform>();
    }

    protected virtual void OnDestroy()
    {
        OnStateChanged -= DetermineStateBehaviour;
    }

    protected virtual void Update()
    {
        Debug.DrawRay(transform.position, transform.right * _distanceToSpottedState, Color.blue);
        Debug.DrawRay(transform.position, transform.right * _distanceToTriggeredState, Color.yellow);
        Debug.DrawRay(transform.position, transform.right * _distanceToInteractionState, Color.red);

        SetDestinationVars();

        if (_IsAutoStateDetection)
        {
            CheckState();
        }
    }

    protected void ChangeBehaviourState(NPC_States newState)
    {
        if (CurrentState != newState)
        {
            LastState = CurrentState;
            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
        }
    }

    protected void CheckState()
    {
        if (_angleToPlayer > -15 && _angleToPlayer < 15)
        {
            if (!_IsRunning)
            {
                if (_distanceToPlayer > _distanceToSpottedState)
                {
                    ChangeBehaviourState(NPC_States.Idle);
                }
                if (_distanceToPlayer < _distanceToSpottedState && _distanceToPlayer > _distanceToTriggeredState)
                {
                    ChangeBehaviourState(NPC_States.PlayerSpotted);
                }
                if (_distanceToPlayer < _distanceToTriggeredState && _distanceToPlayer > _distanceToInteractionState)
                {
                    ChangeBehaviourState(NPC_States.Triggered);
                }
            }
            if (_distanceToPlayer < _distanceToInteractionState)
            {
                ChangeBehaviourState(NPC_States.Interaction);
            }
        }
        else
        {
            ChangeBehaviourState(NPC_States.Idle);
        }
    }

    private void SetDestinationVars()
    {
        _directionToPlayer = (_Player.position - transform.position).normalized;
        _angleToPlayer = Mathf.Round(Vector2.SignedAngle(new Vector2(transform.localScale.x, 0), _directionToPlayer));
        _distanceToPlayer = Vector2.Distance(transform.position, _Player.position);
    }

    private void DetermineStateBehaviour(NPC_States state)
    {
        switch (state)
        {
            case NPC_States.Idle:
                ExecuteIdleStateBehaviour();
                break;
            case NPC_States.PlayerSpotted:
                ExecutePlayerSpottedBehaviour();
                break;
            case NPC_States.Triggered:
                ExecuteTriggeredBehaviour();
                break;
            case NPC_States.Interaction:
                ExecuteInteractionBehaviour();
                break;
        }
    }

    protected abstract void ExecuteIdleStateBehaviour();
    protected abstract void ExecutePlayerSpottedBehaviour();
    protected abstract void ExecuteTriggeredBehaviour();
    protected abstract void ExecuteInteractionBehaviour();
}
