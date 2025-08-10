using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Run to player", story: "run to [player] set [animator] [run] to [true]", category: "Action", id: "2afcae9ab44cd2b9c16cdbc715fadcca")]
public partial class RunToPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<string> Run;
    [SerializeReference] public BlackboardVariable<bool> True;

    private Rigidbody2D rb;
    private Enemy enemy;
    public float runSpeed = 100f;

    protected override Status OnStart()
    {
        enemy = Agent.Value.GetComponent<Enemy>();
        rb = Agent.Value.GetComponent<Rigidbody2D>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float distanceToPlayer = Vector2.Distance(Agent.Value.transform.position, Player.Value.transform.position);
        if (distanceToPlayer < 4f)
        {
            Animator.Value.SetBool(Run.Value, false);
            return Status.Success;
        }
        float directionMultiplier = 1;
        if (enemy.facingRight)
        {
            directionMultiplier = 1;
        }
        else
        {
            directionMultiplier = -1;
        }
        Animator.Value.SetBool(Run.Value, true);
        rb.AddForce(new Vector2(runSpeed * directionMultiplier, 0));

        return Status.Running;
    }
}

