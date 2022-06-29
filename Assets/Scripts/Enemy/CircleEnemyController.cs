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

    void UpdateState()
    { 
        /*
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
        */
    }

    void ChangeState(CircleEnemyState newState)
    {
        //Debug.Log(gameObject.name +  " SM: " + m_state + " to " + newState);

        m_state = newState;
    }

}
