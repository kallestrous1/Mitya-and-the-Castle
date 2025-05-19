using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
        Wandering,
        Following
    }

public class mosquito : Enemy
{
    [SerializeField] private Transform player;
    public float waypointDistanceThreshold = 2f;

    private Transform currentWaypointTarget;
    public Transform[] waypoints;

    private float circleDuration = 5f;
    private float attackDuration = 3f;

    public float followDistance;

    public State currentState;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (waypoints == null || waypoints.Length == 0) return;

        currentState = State.Wandering;
        Wander();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (DistanceToPlayer() < followDistance && target != null)
        {
            currentState = State.Following;
            MoveTowardsTarget(target.transform.position);
        }

        if (reachedWaypoint() && currentState == State.Wandering)
        {
            pickRandomWaypoint();
            Wander();
        }
    }

    private float DistanceToPlayer()
    {
        if (!player) return float.MaxValue;
        return Vector2.Distance(transform.position, target.transform.position);
    }

    private void MoveTowardsTarget(Vector2 targetPos)
    {
        Vector2 dir = targetPos - (Vector2)transform.position;
        if(dir.sqrMagnitude < 0.0001f)
        {
            return;
        }
        dir.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private bool reachedWaypoint()
    {
        if (!currentWaypointTarget) return false;

        return Vector2.Distance(transform.position, currentWaypointTarget.position) < waypointDistanceThreshold;
    }

    private void pickRandomWaypoint()
    {
        if(waypoints!= null && waypoints.Length > 0)
        {
            currentWaypointTarget = waypoints[Random.Range(0, waypoints.Length)];
        }
    }

    private void Wander()
    {
        pickRandomWaypoint();

        if(currentState == State.Wandering)
        {
            if (currentWaypointTarget)
            {
                MoveTowardsTarget(currentWaypointTarget.position);
            }
        }
    }

    private void Attack(float duration)
    {
        ani.SetTrigger("Attack");
        Vector2 targetPos = (Vector2)target.transform.position;
        Vector2 dir = targetPos - (Vector2)transform.position;
        dir.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, targetPos, 10 * Time.deltaTime);
    }

}
