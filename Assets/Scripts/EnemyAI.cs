using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chaseRange;
    [SerializeField] private float shootRange;

    [SerializeField] private Cover[] availableCovers;
    [SerializeField] private GameObject enemyBullet;

    private Transform playerTransform;
    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;
    private GameObject[] enemyBullets;

    private Node topNode;

    private float _health;
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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyBullets = new GameObject[10];
        for (int i = 0; i < enemyBullets.Length; i++)
        {
            enemyBullets[i] = Instantiate(enemyBullet, transform.position, Quaternion.identity);
            enemyBullets[i].SetActive(false);
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
        health += Time.deltaTime * healthRestoreRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerBullet")
        {
            health -= 10f;
            other.gameObject.SetActive(false);
        }
    }

    private void ConstructBehaviourTree()
    {
        CanCoverNode canCoverNode = new CanCoverNode(availableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chaseRangeNode = new RangeNode(chaseRange, playerTransform, transform);
        RangeNode shootRangeNode = new RangeNode(shootRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(playerTransform, agent, this, enemyBullets);

        Sequence chaseSequence = new Sequence(new List<Node> { chaseRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { canCoverNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        Selector tryToCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToCoverSelector });

        topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestSpot)
    {
        bestCoverSpot = bestSpot;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
}
