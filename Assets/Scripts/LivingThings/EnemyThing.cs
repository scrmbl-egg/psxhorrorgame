using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyThing : LivingThing
{
    [Header("Dependencies")]
    //
    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    [Space(10)]
    [Header("Enemy Properties")]
    //
    [SerializeField, Min(0)] private int minDamage;
    [SerializeField, Min(1)] private int maxDamage;
    [Space(2)]
    [SerializeField, Min(0)] private float minStateChangeDelayBeforeAttack;
    [SerializeField, Min(float.Epsilon)] private float maxStateChangeDelayBeforeAttack;
    [SerializeField, Min(0)] private float minStateChangeDelayAfterAttack;
    [SerializeField, Min(float.Epsilon)] private float maxStateChangeDelayAfterAttack;
    [Space(2)]
    [SerializeField] private LayerMask attackLayers;

    public int Damage
    {
        get => Random.Range(minDamage, maxDamage + 1);
    }
    public float RandomStateDelayBeforeAttack
    {
        get => Random.Range(minStateChangeDelayBeforeAttack, maxStateChangeDelayBeforeAttack);
    }
    public float RandomStateDelayAfterAttack
    {
        get => Random.Range(minStateChangeDelayAfterAttack, maxStateChangeDelayAfterAttack);
    }
    public LayerMask AttackLayers => attackLayers;


    [Space(10)]
    [Header("Artificial Intelligence Settings")]
    //
    [SerializeField] private float alertRange;
    [SerializeField] private float attentionSpan;
    [Space(2)]
    [SerializeField] private Transform currentTarget;
    [Space(2)]
    [SerializeField] private float attackDistanceThreshold;
    private EnemyBaseState _currentState;
    private static Transform _player;
    //states
    private EnemyIdleState _idleState = new EnemyIdleState();
    private EnemyChaseState _chaseState = new EnemyChaseState();
    private EnemyAttackState _attackState = new EnemyAttackState();

    public float AlertRange => alertRange;
    public float AttentionSpan => attentionSpan;
    public Transform Target => currentTarget;
    public float AttackDistanceThreshold => attackDistanceThreshold;
    public Transform Player => _player;
    public EnemyIdleState IdleState => _idleState;
    public EnemyChaseState ChaseState => _chaseState;
    public EnemyAttackState AttackState => _attackState;

    [Space(10)]
    [Header("Other")]
    //
    [SerializeField] private bool drawAlertRange;
    [SerializeField] private bool drawAttackRange;

    #region MonoBehaviour

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _player = FindObjectOfType<PlayerThing>().transform;

        SetState(IdleState);
    }

    public virtual void Update()
    {
        _currentState.UpdateState(this);
    }

    private void OnDrawGizmos()
    {
        if (drawAlertRange) DrawAlertRange();
        if (drawAttackRange) DrawAttackRange();
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
        //TODO: do damage sound
    }

    #endregion
    #region AI

    public void SetState(EnemyBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    #endregion

    #endregion
    #region Private methods

    private void DrawAlertRange()
    {
        Vector3 origin = transform.position;
        float radius = alertRange;

        Gizmos.DrawWireSphere(origin, radius);
    }

    private void DrawAttackRange()
    {
        Vector3 origin = transform.position;
        float radius1 = agent.stoppingDistance;
        float radius2 = agent.stoppingDistance + attackDistanceThreshold;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, radius1);
        Gizmos.DrawWireSphere(origin, radius2);
    }

    #endregion
}