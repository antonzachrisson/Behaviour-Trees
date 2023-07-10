using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoverPlayerNode : Node
{
    private NavMeshAgent agent;
    private PlayerAI ai;

    public CoverPlayerNode(NavMeshAgent agent, PlayerAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Vector3 coverSpot = ai.GetBestCoverSpot();
        if (coverSpot == null)
            return NodeState.Failure;
        //ai.SetColor(Color.yellow);
        float dist = Vector3.Distance(coverSpot, agent.transform.position);
        if (dist > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(coverSpot);
            return NodeState.Running;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.Success;
        }
    }
}

