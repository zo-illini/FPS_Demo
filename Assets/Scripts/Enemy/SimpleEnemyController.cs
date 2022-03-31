using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SimpleEnemyState{Patrolling};

public class SimpleEnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    GameWorld m_world;

    NavMeshAgent m_agent;

    SimpleEnemyState m_state;

    Vector3 m_randomWalkDestination;

    Collider m_collider;

    public float m_moveSpeed;

    public float m_randomWalkRadius;

    public Vector3 m_randomWalkCenter;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        m_state = SimpleEnemyState.Patrolling;
        m_randomWalkDestination = transform.position;
        m_randomWalkCenter = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_state)
        {
            case SimpleEnemyState.Patrolling:
                if (IsReachedRandomWalkDestinationXZ())
                {
                    SetRandomWalkDestinationXZ();
                }
                else
                {
                    m_agent.SetDestination(m_randomWalkDestination);
                }
            break;

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        OnEnemyHit();
        Destroy(this.gameObject);
    }

    void SetRandomWalkDestinationXZ()
    {
        //float unitX = Random.Range(0, 1f);
        //Vector2 unitVector = new Vector2(unitX, Mathf.Sqrt(1 - unitX * unitX));
        Vector2 newPosXZ = new Vector2(m_randomWalkCenter.x, m_randomWalkCenter.z) + Random.insideUnitCircle * m_randomWalkRadius;
        Vector3 newPos = new Vector3(newPosXZ.x, m_randomWalkCenter.y, newPosXZ.y);

        // Validate walkpoint here

        m_randomWalkDestination = newPos;
    }

    bool IsReachedRandomWalkDestinationXZ()
    {
        Vector3 pos = transform.position;
        return new Vector2(pos.x - m_randomWalkDestination.x, pos.z - m_randomWalkDestination.z).magnitude < 1f;
    }

    void OnEnemyHit()
    {
        EventManager.Broadcast(Events.EventEnemyDie);
    }
}
