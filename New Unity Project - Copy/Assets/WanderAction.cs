using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wander", story: "[Agent] wanders between [points] [randomly]", category: "Action", id: "a3c251af0514cad238f6f9ec7dedf408")]
public partial class WanderAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Points;
    [SerializeReference] public BlackboardVariable<bool> Randomly;
    [SerializeReference] public BlackboardVariable<Transform> AgentTransform;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;

    private Transform CurrentTarget;
    private int currentPatrolPoint = 0;
    public float DistanceThreshold = 2f;
    public float speed = 2f;
    private bool flipped;

    protected override Status OnStart()
    {
        if(Agent.Value == null)
        {
            LogFailure("no agent assigned");
            return Status.Failure;
        }

        if(Waypoints.Value == null || Waypoints.Value.Count == 0)
        {
            LogFailure("No waypoints assigned");
            return Status.Failure;
        }

        Initialize();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || CurrentTarget == null)
        {
            return Status.Failure;
        }
        MoveTowardsTarget(Waypoints.Value[currentPatrolPoint].transform.position);

        if(Waypoints.Value[currentPatrolPoint].transform.position.x - Agent.Value.transform.position.x > 0 && flipped == false)
        {
            flipped = true;
            Agent.Value.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (Waypoints.Value[currentPatrolPoint].transform.position.x - Agent.Value.transform.position.x < 0 && flipped == true)
        {
            flipped = false;
            Agent.Value.transform.localScale = new Vector3(1f, 1f, 1f);

        }


        if (reachedWaypoint())
        {
            if (Randomly)
            {
                pickRandomWaypoint();
            }
            else
            {
                
                if (currentPatrolPoint < Waypoints.Value.Capacity-1)
                {
                    currentPatrolPoint += 1;
                }
                else
                {
                    currentPatrolPoint = 0;
                }
            }
        }

        return Status.Success;
    }

    private Status Initialize()
    {
        CurrentTarget = Waypoints.Value[currentPatrolPoint].transform;
        return Status.Running;
    }

    private bool reachedWaypoint()
    {
        return Vector2.Distance(AgentTransform.Value.position, Waypoints.Value[currentPatrolPoint].transform.position) < DistanceThreshold;
    }

    private void MoveTowardsTarget(Vector2 targetPos)
    {
        Vector2 dir = targetPos - (Vector2)Agent.Value.transform.position;
        if (dir.sqrMagnitude < 0.0001f)
        {
            return;
        }
        dir.Normalize();
        Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position, targetPos, speed * Time.deltaTime);
    }

    private void pickRandomWaypoint()
    {
        if (Waypoints.Value != null && Waypoints.Value.Count > 0)
        {
            int target = Random.Range(0, Waypoints.Value.Count);
            CurrentTarget = Waypoints.Value[target].transform;
            currentPatrolPoint = target;
        }
    }
}

