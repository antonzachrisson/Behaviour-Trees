using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlayerNode : Node
{
    private PlayerAI ai;

    public HealthPlayerNode(PlayerAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        if (ai.health > 0)
        {
            return NodeState.Failure;
        }
        ai.Die();
        return NodeState.Success;
    }
}
