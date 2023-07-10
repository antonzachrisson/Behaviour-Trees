using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRangeNode : Node
{
    private float range;
    private Transform origin;

    public TankRangeNode(float range, Transform origin)
    {
        this.range = range;
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

        float dist = Vector3.Distance(closestEnemy.position, origin.position);
        return dist <= range ? NodeState.Success : NodeState.Failure;
    }
}
