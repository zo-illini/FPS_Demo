using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BaseEnemyController : MonoBehaviour
{
    public float m_patrolSpeed;

    public float m_randomWalkRadius;

    public float m_alertRadius;

    public float m_attackRadius;

    public Vector3 m_randomWalkCenter;

    protected GameObject m_player;
    protected NavMeshAgent m_agent;

    protected Vector3 m_randomWalkDestination;

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_patrolSpeed;

        m_randomWalkDestination = transform.position;
        m_randomWalkCenter = transform.position;

        m_player = FindObjectOfType<GameWorld>().m_player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void Patrol()
    {
        if (m_agent.remainingDistance < 1.0f)
        {
            m_agent.SetDestination(GetRandomWalkDestination());
        }
    }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player Projectile")
        {
            OnEnemyHit();
            Destroy(this.gameObject);
            Destroy(collider.gameObject);
        }
        
    }

    protected Vector3 GetRandomWalkDestination()
    {
        //float unitX = Random.Range(0, 1f);
        //Vector2 unitVector = new Vector2(unitX, Mathf.Sqrt(1 - unitX * unitX));
        Vector2 newPosXZ = new Vector2(m_randomWalkCenter.x, m_randomWalkCenter.z) + Random.insideUnitCircle * m_randomWalkRadius;
        Vector3 newPos = new Vector3(newPosXZ.x, m_randomWalkCenter.y, newPosXZ.y);

        // Validate walkpoint here

        return newPos;
    }

    protected Vector2 GetPlayerVectorXZ()
    {
        return new Vector2(m_player.transform.position.x, m_player.transform.position.z) - new Vector2(transform.position.x, transform.position.z);
    }

    protected void OnEnemyHit()
    {
        EventManager.Broadcast(Events.EventEnemyDie);
    }
}
