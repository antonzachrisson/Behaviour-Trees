using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTankNode : Node
{
    private TankAI ai;

    public HealthTankNode(TankAI ai)
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
