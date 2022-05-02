using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyThing : LivingThing
{
    [Header("Enemy Properties")]
    //
    [SerializeField, Min(0)] private int minDamage;
    [SerializeField, Min(1)] private int maxDamage;
    [Space(2)]
    [SerializeField, Min(0)] private float minDurationBeforeAttack;
    [SerializeField, Min(1)] private float maxDurationBeforeAttack;
    [Space(2)]
    [SerializeField] private LayerMask attackLayers;
    private float _currentTimeBeforeAttack;
    private float _currentDurationBeforeAttack;

    public int Damage
    {
        get => Random.Range(minDamage, maxDamage + 1);
    }
    public float RandomDurationBeforeAttack
    {
        get => Random.Range(minDurationBeforeAttack, maxDurationBeforeAttack);
    }

    [Space(10)]
    [Header("Artificial Intelligence Settings")]
    //
    [SerializeField] private bool thingIsAlert;
    [SerializeField] private float alertRange;
    [SerializeField] private float alertTime;
    [Space(2)]
    [SerializeField] private Transform currentTarget;
    [Space(2)]
    [SerializeField] private float attackDistanceThreshold;
    private NavMeshAgent _agent;
    private EnemyBaseState _currentState;
    private EnemyIdleState _idleState = new EnemyIdleState();
    private EnemyChaseState _chaseState = new EnemyChaseState();
    private EnemyAttackState _attackState = new EnemyAttackState();

    public bool ThingIsAlert => thingIsAlert;
    public float AlertRange => alertRange;
    public float AlertTime => alertTime;
    public Transform Target
    {
        get => currentTarget;
        set => currentTarget = value;
    }
    public float AttackDistanceThreshold => attackDistanceThreshold;
    public NavMeshAgent Agent => _agent;
    public EnemyIdleState IdleState => _idleState;
    public EnemyChaseState ChaseState => _chaseState;
    public EnemyAttackState AttackState => _attackState;

    #region MonoBehaviour

    private void Awake()
    {
        SwitchState(IdleState);
    }

    public virtual void Update()
    {

    }

    private void OnDrawGizmos()
    {
    }

    #endregion

    #region Public methods

    #region Health and death

    public override void DeathEffect()
    {
        base.DeathEffect();

        //TODO: trigger ragdoll physics
    }

    public override void DamageEffect()
    {
        Debug.Log($"{ThingName} enemy has been hit!");
        //TODO: do damage sound
    }

    #endregion
    #region AI

    public void SwitchState(EnemyBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }

    #endregion

    #endregion
}