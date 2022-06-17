using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestSyncAI : MonoBehaviour
{
    NavMeshAgent m_agent;
    public float m_patrolSpeed;
    Vector3 m_patrolDestination;
    Vector3 m_patrolCenter;
    public float m_patrolRadius;
    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_patrolCenter = transform.position;
        m_patrolDestination = m_patrolCenter;

    }

    // Update is called once per frame
    void Update()
    {
        m_agent.speed = m_patrolSpeed;
        m_agent.isStopped = false;
        Vector2 posXZ = new Vector2(transform.position.x, transform.position.z);
        if ((posXZ - new Vector2(m_patrolDestination.x, m_patrolDestination.z)).magnitude < 0.5f)
        {
            m_patrolDestination = GetRandomDestination();
        }
        else
        {
            m_agent.SetDestination(m_patrolDestination);
        }
    }

    private Vector3 GetRandomDestination()
    {
        Vector2 newPosXZ = new Vector2(m_patrolCenter.x, m_patrolCenter.z) + Random.insideUnitCircle * m_patrolRadius;
        Vector3 newPos = new Vector3(newPosXZ.x, m_patrolCenter.y, newPosXZ.y);

        return newPos;
    }
}
