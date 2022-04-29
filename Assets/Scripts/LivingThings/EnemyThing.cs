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
    [SerializeField, Min(0)] private float minTimeBeforeAttack;
    [SerializeField, Min(1)] private float maxTimeBeforeAttack;

    public int Damage
    {
        get
        {
            ///enemy damage is not a fixed value
            ///but rather a random value within a range

            return Random.Range(minDamage, maxDamage + 1);
        }
    }
    public float TimeBeforeAttack
    {
        get => Random.Range(minTimeBeforeAttack, maxTimeBeforeAttack);
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
        //TODO: CHANGE ALERT BEHAVIOUR WITH A COLLIDER INSTEAD OF DISTANCE CHECK

        float distanceFromPlayer = Vector3.Distance(transform.position, _player.position);
        bool playerIsInAlertRange = distanceFromPlayer <= alertRange;

        if (!thingIsAlert && playerIsInAlertRange)
        {
            //set target and change state
            currentTarget = _player;
            _currentAlertTime = alertTime;
            thingIsAlert = true;

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
        bool thereIsNoTarget = currentTarget == null;
        bool isWithinAttackThreshold = 
            _agent.remainingDistance <= _agent.stoppingDistance + attackDistanceThreshold;


        if (thereIsNoTarget) SetState(AIState.Idle);
        else _agent.SetDestination(currentTarget.position);


        if (isWithinAttackThreshold) SetState(AIState.Attack);
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
        
    }

    #endregion
    #region Actions
    public virtual void Attack()
    {
        Debug.Log($"{ThingName}: I attack with {Damage} damage");
    }

    #endregion

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
        Gizmos.color = gizmoColor;

        Vector3 origin = transform.position;
        float radius = alertRange;
        Gizmos.DrawWireSphere(origin, radius);
    }

    #endregion
}