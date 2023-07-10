using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    [SerializeField] private GameObject playerBullet;

    private Material material;
    private Vector3 bestCoverSpot;
    private NavMeshAgent agent;
    private GameObject[] playerBullets;

    private Node topNode;

    [SerializeField] private float _health;
    public float health
    {
        get { return _health; }
        set { _health = Mathf.Clamp(value, 0, maxHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        health = maxHealth;

        playerBullets = new GameObject[10];
        for (int i = 0; i < playerBullets.Length; i++)
        {
            playerBullets[i] = Instantiate(playerBullet, transform.position, Quaternion.identity);
            playerBullets[i].SetActive(false);
        }

        ConstructBehaviourTree();
    }

    private void Update()
    {
        topNode.Evaluate();
        if (topNode.nodeState == NodeState.Failure)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            health -= 10f;
            other.gameObject.SetActive(false);
        }
    }

    private void ConstructBehaviourTree()
    {
        FindCoverPlayerNode findCoverPlayerNode = new FindCoverPlayerNode(this);
        CoverPlayerNode coverPlayerNode = new CoverPlayerNode(agent, this);
        ShootPlayerNode shootPlayerNode = new ShootPlayerNode(agent, this, playerBullets);
        HealthPlayerNode healthPlayerNode = new HealthPlayerNode(this);

        Sequence hideBehindTankSequence = new Sequence(new List<Node> { findCoverPlayerNode, coverPlayerNode, shootPlayerNode });
        Selector mainSelector = new Selector(new List<Node> { healthPlayerNode, hideBehindTankSequence });

        topNode = new Selector(new List<Node> { mainSelector });
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Vector3 bestSpot)
    {
        bestCoverSpot = bestSpot;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public Vector3 GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
}

