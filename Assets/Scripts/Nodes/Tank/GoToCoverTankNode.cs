using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToCoverTankNode : Node
{
    private NavMeshAgent agent;
    private TankAI ai;

    public GoToCoverTankNode(NavMeshAgent agent, TankAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform coverSpot = ai.GetBestCoverSpot();
        if (coverSpot == null)
            return NodeState.Failure;
        ai.SetColor(Color.yellow);
        float dist = Vector3.Distance(coverSpot.position, agent.transform.position);
        if (dist > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(coverSpot.position);
            return NodeState.Running;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.Success;
        }
    }
}
