using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredTankNode : Node
{
    private Transform origin;

    public IsCoveredTankNode(Transform origin)
    {
        this.origin = origin;
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
            if (Vector3.Distance(enemyTransforms[i].position, origin.position) < Vector3.Distance(closestEnemy.position, origin.position))
                closestEnemy = enemyTransforms[i];
        }

        RaycastHit hit;
        if (Physics.Raycast(origin.position, closestEnemy.position - origin.position, out hit))
        {
            if (hit.collider.transform != closestEnemy)
            {
                return NodeState.Success;
            }
        }
        return NodeState.Failure;
    }
}
