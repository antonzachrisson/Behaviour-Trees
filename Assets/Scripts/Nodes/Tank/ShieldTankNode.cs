using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTankNode : Node
{
    private TankAI ai;
    private float threshold;

    public ShieldTankNode(TankAI ai, float threshold)
    {
        this.ai = ai;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        return ai.shield <= threshold ? NodeState.Success : NodeState.Failure;
    }
}
