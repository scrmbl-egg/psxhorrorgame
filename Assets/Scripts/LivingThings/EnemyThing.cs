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
        get
        {
            ///enemy damage is not a fixed value
            ///but rather a random value within a range

            return Random.Range(minDamage, maxDamage + 1);
        }
    }
    private float RandomDurationBeforeAttack
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
    [SerializeField] private AIState currentState;
    [SerializeField] private Transform currentTarget;
    [Space(2)]
    [SerializeField] private float attackDistanceThreshold;
    private NavMeshAgent _agent;
    private float _currentAlertTime;
    private static Transform _player;

    [Space(10)]
    [Header("Other")]
    //
    [SerializeField] private bool drawAlertRange = true;
    [SerializeField] private Color gizmoColor;

    #region MonoBehaviour

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = FindObjectOfType<PlayerThing>().transform;
    }

    public virtual void Update()
    {
        ManageAIStates();
    }

    private void OnDrawGizmos()
    {
        if (drawAlertRange) DrawAlertRange();
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

    /// <summary>
    /// Changes the thing's current AI behaviour.
    /// </summary>
    /// <param name="state">AI state.</param>
    public void SetState(AIState state)
    {
        currentState = state;
    }

    /// <summary>
    /// Idle behaviour.
    /// </summary>
    /// 
    /// <remarks>
    /// Base behaviour makes the thing stop mid-path.
    /// If the player enters the alert range, 
    /// the thing will be alert and switch to the chase state.
    /// </remarks>
    public virtual void IdleState()
    {
        _agent.isStopped = true;
        
        if (TargetIsInRange(_player))
        {
            //set target and change state
            currentTarget = _player;
            _currentAlertTime = alertTime;

            SetState(AIState.Chase);
        }
    }

    /// <summary>
    /// Moving to target behaviour.
    /// </summary>
    /// 
    /// <remarks>
    /// Base behaviour makes the thing move towards a destination, but this destination will only be given once.
    /// </remarks>
    public virtual void MoveState()
    {
        _agent.isStopped = false;

        bool thereIsTarget = currentTarget != null;
        bool targetHasntBeenSet = true;
        if (targetHasntBeenSet && thereIsTarget)
        {
            _agent.SetDestination(currentTarget.position);
            targetHasntBeenSet = false;
        }
        else SetState(AIState.Idle);
    }

    /// <summary>
    /// Chasing target behaviour.
    /// </summary>
    /// 
    /// <remarks>
    /// Base behaviour constantly chases target when it isn't NULL.
    /// When the attack distance threshold is reached, the thing switches to its attack state.
    /// </remarks>
    public virtual void ChaseState()
    {
        _agent.isStopped = false;

        bool thereIsNoTarget = currentTarget == null;
        bool alertTimeIsOver = _currentAlertTime <= 0;
        bool isWithinAttackThreshold = _agent.remainingDistance <= _agent.stoppingDistance + attackDistanceThreshold;

        //check if there's no target
        if (thereIsNoTarget) SetState(AIState.Idle);
        else _agent.SetDestination(currentTarget.position);

        //if target is not in range while chasing, execute timer
        if (TargetIsInRange(currentTarget)) _currentAlertTime = alertTime;
        else _currentAlertTime -= Time.deltaTime;

        if (alertTimeIsOver) SetState(AIState.Idle);
        if (isWithinAttackThreshold)
        {
            SetState(AIState.Attack);
            _currentDurationBeforeAttack = RandomDurationBeforeAttack;
        }
    }

    /// <summary>
    /// Attacking a target behaviour.
    /// </summary>
    /// 
    /// <remarks>
    /// Base behaviour stops the thing and executes the attack action. 
    /// Then, it changes back to chasing the target.
    /// </remarks>
    public virtual void AttackState()
    {
        _agent.isStopped = true;

        bool timerHasEnded = _currentTimeBeforeAttack >= _currentDurationBeforeAttack;
        if (timerHasEnded)
        {
            Attack();
            SetState(AIState.Chase);
            _currentTimeBeforeAttack = 0;
        }
        else _currentTimeBeforeAttack += Time.deltaTime;
    }

    #endregion
    #region Actions
    public virtual void Attack()
    {
        Debug.Log($"{ThingName}: I attack with {Damage} damage");

        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRange, attackLayers);
        foreach(Collider collider in colliders)
        {
            bool objectiveIsPlayer = collider.CompareTag("Player");

            if (objectiveIsPlayer)
            {
                PlayerThing player = collider.GetComponentInParent<PlayerThing>();
                player.Health -= Damage;
            }
        }
    }

    #endregion

    public bool TargetIsInRange(Transform target)
    {
        float distanceFromTarget = Vector3.Distance(transform.position, target.position);
        bool targetIsInRange = distanceFromTarget <= alertRange;

        return targetIsInRange;
    }

    #endregion
    #region Private methods

    private void ManageAIStates()
    {
        switch (currentState)
        {
            case AIState.Idle: IdleState();
                break;
            case AIState.Move: MoveState();
                break;
            case AIState.Chase: ChaseState();
                break;
            case AIState.Attack: AttackState();
                break;
            default:
                break;
        }
    }

    private void DrawAlertRange()
    {
        Color gizmoColorWithoutAlpha = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1);
        Vector3 origin = transform.position;
        float radius = alertRange;

        Gizmos.color = gizmoColorWithoutAlpha;
        Gizmos.DrawWireSphere(origin, radius);
    }

    #endregion
}