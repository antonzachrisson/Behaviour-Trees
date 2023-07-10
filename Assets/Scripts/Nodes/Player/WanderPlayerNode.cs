using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderPlayerNode : Node
{
    private NavMeshAgent agent;
    private bool hasPos;
    private Vector3 finalPos;

    public WanderPlayerNode(NavMeshAgent agent)
    {
        this.agent = agent;
        hasPos = false;
    }

    public override NodeState Evaluate()
    {        
        if(!hasPos)
        {
            Vector3 randDir = Random.insideUnitSphere * 1000f;
            randDir.y = 0;
            randDir += agent.transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randDir, out hit, 1000f, 1);
            finalPos = hit.position;
            agent.isStopped = false;
            agent.SetDestination(finalPos);
            hasPos = true;
            return NodeState.Failure;
        }
        else
        {
            float dist = Vector3.Distance(finalPos, agent.transform.position);
            if (dist <= 0.2f)
            {
                hasPos = false;
                agent.isStopped = true;
            }
            return NodeState.Running;
        }
    }
}
