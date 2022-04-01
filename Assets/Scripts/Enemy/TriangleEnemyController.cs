using UnityEngine;
using UnityEngine.AI;

enum TriangleEnemyState{Patrolling, Approaching, Attacking};


public class TriangleEnemyController : BaseEnemyController
{
    public float m_approachSpeed;

    public float m_dashSpeed;

    public float m_dashOverDistance;

    TriangleEnemyState m_state;


    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_patrolSpeed;

        m_state = TriangleEnemyState.Patrolling;
        m_randomWalkDestination = transform.position;
        m_randomWalkCenter = transform.position;

        m_player = FindObjectOfType<GameWorld>().m_player;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistanceXZ = Vector2.Distance(new Vector2(m_player.transform.position.x, m_player.transform.position.z), new Vector2(transform.position.x, transform.position.z));

        switch(m_state)
        {
            case TriangleEnemyState.Patrolling:
                Patrol();
            break;
            case TriangleEnemyState.Approaching:
                m_agent.SetDestination(m_player.transform.position);
            break;
            case TriangleEnemyState.Attacking:
                
            break;
        }

        UpdateState();
    }

    void UpdateState()
    { 
        float playerDistanceXZ = GetPlayerVectorXZ().magnitude;

        switch(m_state)
        {  
            case TriangleEnemyState.Patrolling:
                if (playerDistanceXZ < m_alertRadius)
                {
                    ChangeState(TriangleEnemyState.Approaching);
                    m_agent.speed = m_approachSpeed;
                }
            break;

            case TriangleEnemyState.Approaching:
                if (playerDistanceXZ < m_attackRadius)
                {
                    ChangeState(TriangleEnemyState.Attacking);

                    Vector2 playerLookAtXZ = GetPlayerVectorXZ().normalized;
                    Vector3 dashDestination = m_player.transform.position + new Vector3(playerLookAtXZ.x, 0, playerLookAtXZ.y) * m_dashOverDistance;
                    m_agent.SetDestination(dashDestination);
                    m_agent.speed = m_dashSpeed;
                }
            break;

            case TriangleEnemyState.Attacking:
                if (m_agent.remainingDistance < 1f)
                {
                    ChangeState(TriangleEnemyState.Approaching);
                    m_agent.speed = m_approachSpeed;
                }
            break;
        }
    }

    void ChangeState(TriangleEnemyState newState)
    {
        //Debug.Log(gameObject.name +  " SM: " + m_state + " to " + newState);
        m_state = newState;
    }

}
