using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class GetClosestEnemyNode : Node
{
    private TankAI ai;
    private Transform[] enemyTransforms;

    public GetClosestEnemyNode(Transform[] enemyTransforms, TankAI ai)
    {
        this.ai = ai;
        this.enemyTransforms = enemyTransforms;
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
        ai.SetEnemyTransforms(enemyTransforms);
        ai.SetClosestEnemy(closestEnemy);
        return closestEnemy != null ? NodeState.Success : NodeState.Failure;
    }
}*/
