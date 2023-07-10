using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankAI : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float lowShieldThreshold;
    [SerializeField] private float shieldRestoreRate;
    [SerializeField] private float maxShield;

    [SerializeField] private float senseRange;

    [SerializeField] private Cover[] availableCovers;

    [SerializeField] private Transform[] enemyTransforms;
    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    [SerializeField] private float _health;
    [SerializeField] private float _shield;
    public float health
    {
        get { return _health; }
        set { _health = Mathf.Clamp(value, 0, maxHealth); }
    }
    public float shield
    {
        get { return _shield; }
        set { _shield = Mathf.Clamp(value, 0, maxShield); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        health = maxHealth;
        shield = maxShield;
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
        shield += Time.deltaTime * shieldRestoreRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyBullet")
        {
            if (shield > 0)
            {
                if (shield < 10f)
                {
                    float healthDmg = 10f - shield;
                    shield = 0;
                    health -= healthDmg;
                }
                else
                    shield -= 10f;
            }
            else
                health -= 10f;
            other.gameObject.SetActive(false);
        }
    }

    private void ConstructBehaviourTree()
    {
        CanCoverTankNode canCoverTankNode = new CanCoverTankNode(availableCovers, this);
        GoToCoverTankNode goToCoverTankNode = new GoToCoverTankNode(agent, this);
        ShieldTankNode shieldTankNode = new ShieldTankNode(this, lowShieldThreshold);
        IsCoveredTankNode isCoveredTankNode = new IsCoveredTankNode(transform);
        TankRangeNode keepDistRangeNode = new TankRangeNode(senseRange, transform);
        CreateDistanceNode createDistanceNode = new CreateDistanceNode(agent, this);
        HealthTankNode healthTankNode = new HealthTankNode(this);

        Sequence keepDistSequence = new Sequence(new List<Node> { keepDistRangeNode, createDistanceNode });
       
        Sequence goToCoverSequence = new Sequence(new List<Node> { canCoverTankNode, goToCoverTankNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, keepDistSequence });
        Selector tryToCoverSelector = new Selector(new List<Node> { isCoveredTankNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { shieldTankNode, tryToCoverSelector });

        topNode = new Selector(new List<Node> { healthTankNode, mainCoverSequence, keepDistSequence });
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestSpot)
    {
        bestCoverSpot = bestSpot;
    }

    public void SetEnemyTransforms(Transform[] enemyTransforms)
    {
        this.enemyTransforms = enemyTransforms;
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
