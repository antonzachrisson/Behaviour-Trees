using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCoverPlayerNode : Node
{
    private PlayerAI ai;

    public FindCoverPlayerNode(PlayerAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Transform[] enemyTransforms = new Transform[GameObject.FindGameObjectsWithTag("Enemy").Length];
        for (int i = 0; i < enemyTransforms.Length; i++)
        {
            enemyTransforms[i] = GameObject.FindGameObjectsWithTag("Enemy")[i].transform;
        }

        Transform closestEnemy = enemyTransforms[0];
        for (int i = 1; i < enemyTransforms.Length; i++)
        {
            if (Vector3.Distance(enemyTransforms[i].position, ai.transform.position) < Vector3.Distance(closestEnemy.position, ai.transform.position))
                closestEnemy = enemyTransforms[i];
        }

        Transform tankAI = GameObject.FindGameObjectWithTag("Tank").transform;

        Vector3 targetDirection = tankAI.position - closestEnemy.position;
        targetDirection.y = 0;

        Vector3 targetPos = targetDirection * 1.5f;

        ai.SetBestCoverSpot(targetPos);

        return targetPos != null ? NodeState.Success : NodeState.Failure;
    }
}
