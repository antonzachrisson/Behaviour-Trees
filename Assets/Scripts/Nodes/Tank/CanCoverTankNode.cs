using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanCoverTankNode : Node
{
    private Cover[] availableCovers;
    private Transform[] enemyTransforms;
    private TankAI ai;
    private Transform target;

    public CanCoverTankNode(Cover[] availableCovers, TankAI ai)
    {
        this.availableCovers = availableCovers;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        enemyTransforms = new Transform[GameObject.FindGameObjectsWithTag("Enemy").Length];
        for (int i = 0; i < enemyTransforms.Length; i++)
        {
            enemyTransforms[i] = GameObject.FindGameObjectsWithTag("Enemy")[i].transform;
        }

        target = enemyTransforms[0];
        for (int i = 1; i < enemyTransforms.Length; i++)
        {
            if (Vector3.Distance(enemyTransforms[i].position, ai.transform.position) < Vector3.Distance(target.position, ai.transform.position))
                target = enemyTransforms[i];
        }

        Transform bestSpot = FindBestCoverSpot();
        ai.SetBestCoverSpot(bestSpot);
        return bestSpot != null ? NodeState.Success : NodeState.Failure;
    }

    private Transform FindBestCoverSpot()
    {
        if (ai.GetBestCoverSpot() != null)
        {
            if (CheckIfSpotIsValid(ai.GetBestCoverSpot()))
            {
                return ai.GetBestCoverSpot();
            }
        }
        float minAngle = 90;
        Transform bestSpot = null;
        for (int i = 0; i < availableCovers.Length; i++)
        {
            Transform bestSpotInCover = FindBestSpotInCover(availableCovers[i], ref minAngle);
            if (bestSpotInCover != null)
            {
                bestSpot = bestSpotInCover;
            }
        }
        return bestSpot;
    }

    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {
        Transform[] availableSpots = cover.GetCoverSpots();
        Transform bestSpot = null;
        for (int i = 0; i < availableSpots.Length; i++)
        {
            Vector3 dir = target.position - availableSpots[i].position;
            if (CheckIfSpotIsValid(availableSpots[i]))
            {
                float angle = Vector3.Angle(availableSpots[i].forward, dir);
                if (angle < minAngle)
                {
                    minAngle = angle;
                    bestSpot = availableSpots[i];
                }
            }
        }
        return bestSpot;
    }

    private bool CheckIfSpotIsValid(Transform spot)
    {
        RaycastHit hit;
        Vector3 dir = target.position - spot.position;
        if (Physics.Raycast(spot.position, dir, out hit))
        {
            if (hit.collider.gameObject.tag == "Cover")
            {
                return true;
            }
        }
        return false;
    }
}
