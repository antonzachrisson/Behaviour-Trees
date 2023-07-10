using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private EnemyAI ai;
    private GameObject[] bullets;
    private int bulletIndex = 0;
    private float shootCD = 0f;

    public ShootNode(Transform target, NavMeshAgent agent, EnemyAI ai, GameObject[] bullets)
    {
        this.target = target;
        this.agent = agent;
        this.ai = ai;
        this.bullets = bullets;
    }

    public override NodeState Evaluate()
    {
        Vector3 targetDirection = target.position - agent.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, targetDirection, out hit))
        {
            if (hit.collider.tag == "Cover")
            {
                return NodeState.Failure;
            }
        }

        float singleStep = 4f * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(agent.transform.forward, targetDirection, singleStep, 0.0f);
        agent.transform.rotation = Quaternion.LookRotation(newDirection);

        shootCD += Time.deltaTime;
        if (shootCD >= 1f)
        {
            shootCD = 0f;
            if (bulletIndex >= bullets.Length)
                bulletIndex = 0;
            bullets[bulletIndex].transform.position = agent.transform.position;
            bullets[bulletIndex].SetActive(true);
            bullets[bulletIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
            bullets[bulletIndex].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            bullets[bulletIndex].GetComponent<Rigidbody>().AddForce(agent.transform.forward * 500f);
            bulletIndex++;
        }

        agent.isStopped = true;
        ai.SetColor(Color.green);
        return NodeState.Running;
    }
}
