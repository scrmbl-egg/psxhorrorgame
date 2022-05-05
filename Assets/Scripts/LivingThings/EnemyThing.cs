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

    [Space(10)]
    [Header("Artificial Intelligence Settings")]
    //
    [SerializeField] private float alertRange;
    [SerializeField] private float attentionSpan;
    [Space(2)]
    [SerializeField] private Transform currentTarget;
    [Space(2)]
    [SerializeField] private float attackDistanceThreshold;

    public float AlertRange => alertRange;
    public float AttentionSpan => attentionSpan;
    public Transform Target => currentTarget;
    public static Transform PlayerTarget { get; private set; }
    public float AttackDistanceThreshold => attackDistanceThreshold;

    [Space(10)]
    [Header("Other")]
    //
    [SerializeField] private bool drawAlertRange;
    [SerializeField] private bool drawAttackRange;

    #region MonoBehaviour

    public virtual void Awake()
    {
        if (PlayerTarget == null) PlayerTarget = FindObjectOfType<PlayerThing>().transform;
    }

    private void OnDrawGizmos()
    {
        if (drawAlertRange) DrawAlertRange();
        if (drawAttackRange) DrawAttackRange();
    }

    #endregion

    #region Public methods

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    public virtual void Attack()
    {
        Debug.Log("attack");

        Vector3 origin = transform.position;
        float radius = 3;

        Collider[] colliders = Physics.OverlapSphere(origin, radius, attackLayers);
        foreach (Collider collider in colliders)
        {
            bool colliderIsNotPlayer = !collider.transform.CompareTag("Player");
            if (colliderIsNotPlayer) continue;
            //else...

            LivingThing target = collider.GetComponentInParent<LivingThing>();

            target.Health -= Damage;
        }
    }

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