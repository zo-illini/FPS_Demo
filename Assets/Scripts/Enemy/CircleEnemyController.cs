using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum CircleEnemyState{Patrolling, Spcaing};


public class CircleEnemyController : BaseEnemyController
{
    CircleEnemyState m_state;

    public float m_spacingDistance;
    public float m_spacingSpeed;

    new void Start()
    {
        base.Start();
        m_state = CircleEnemyState.Patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerVectorXZ = new Vector2(m_player.transform.position.x - transform.position.x, m_player.transform.position.z - transform.position.z);
        float playerDistanceXZ = playerVectorXZ.magnitude;

        switch(m_state)
        {
            case CircleEnemyState.Patrolling:
                Patrol();
            break;
            case CircleEnemyState.Spcaing:
                if (playerDistanceXZ < m_spacingDistance)
                {
                    Vector3 delta = -1 * (m_spacingDistance - playerDistanceXZ) * new Vector3(playerVectorXZ.x, 0, playerVectorXZ.y);
                    m_agent.SetDestination(transform.position + delta);
                }
                transform.LookAt(m_player.transform);

                // Shoot at Player
                GetComponent<WeaponController>().Shoot(m_player.transform.position - transform.position);
            break;
        }

        UpdateState();
    }

    void UpdateState()
    { 
        float playerDistanceXZ = GetPlayerVectorXZ().magnitude;

        switch(m_state)
        {  
            case CircleEnemyState.Patrolling:
                if (playerDistanceXZ < m_alertRadius)
                {
                    ChangeState(CircleEnemyState.Spcaing);
                    m_agent.speed = m_spacingSpeed;
                }
            break;

            case CircleEnemyState.Spcaing:
                
            break;
        }
    }

    void ChangeState(CircleEnemyState newState)
    {
        //Debug.Log(gameObject.name +  " SM: " + m_state + " to " + newState);

        m_state = newState;
    }

}
