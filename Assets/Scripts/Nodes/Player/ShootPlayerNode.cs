using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootPlayerNode : Node
{
    private NavMeshAgent agent;
    private PlayerAI ai;
    private GameObject[] bullets;
    private int bulletIndex = 0;
    private float shootCD = 0f;

    public ShootPlayerNode(NavMeshAgent agent, PlayerAI ai, GameObject[] bullets)
    {
        this.agent = agent;
        this.ai = ai;
        this.bullets = bullets;
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

        Vector3 targetDirection = closestEnemy.position - agent.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, targetDirection, out hit))
        {
            if (hit.collider.tag == "Cover")
            {
                return NodeState.Success;
            }
        }

        Vector3 shootDir = Vector3.Normalize(targetDirection);
        shootDir.y = 0;
        
        shootCD += Time.deltaTime;
        if (shootCD >= 0.5f)
        {
            shootCD = 0f;
            if (bulletIndex >= bullets.Length)
                bulletIndex = 0;
            bullets[bulletIndex].transform.position = agent.transform.position;
            bullets[bulletIndex].SetActive(true);
            bullets[bulletIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullets[bulletIndex].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bullets[bulletIndex].GetComponent<Rigidbody>().AddForce(shootDir * 500f);
            bulletIndex++;
        }

        ai.SetColor(Color.blue);
        return NodeState.Running;
    }
}

