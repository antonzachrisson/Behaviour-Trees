using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateDistanceNode : Node
{
    private NavMeshAgent agent;
    private TankAI ai;

    public CreateDistanceNode(NavMeshAgent agent, TankAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.yellow);

        Transform[] enemyTransforms = new Transform[GameObject.FindGameObjectsWithTag("Enemy").Length];
        for (int i = 0; i < enemyTransforms.Length; i++)
        {
            enemyTransforms[i] = GameObject.FindGameObjectsWithTag("Enemy")[i].transform;
        }

        Transform closestEnemy = enemyTransforms[0];
        for (int i = 1; i < enemyTransforms.Length; i++)
        {
            if (Vector3.Distance(enemyTransforms[i].position, agent.transform.position) < Vector3.Distance(closestEnemy.position, agent.transform.position))
                closestEnemy = enemyTransforms[i];
        }

        agent.isStopped = false;
        Vector3 targetDirection = agent.transform.position - closestEnemy.position;
        targetDirection.y = 0;

        Vector3 newPos = targetDirection * 1.1f;
        float singleStep = 4f * Time.deltaTime;
        agent.SetDestination(newPos);
        Vector3 newDirection = Vector3.RotateTowards(agent.transform.forward, -targetDirection, singleStep, 0.0f);
        agent.transform.rotation = Quaternion.LookRotation(newDirection);
        return NodeState.Running;
    }
}
