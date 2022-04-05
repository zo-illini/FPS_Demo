using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

enum SquareEnemyState{Idle, Alert, Dodging};


public class SquareEnemyController : BaseEnemyController
{

    SquareEnemyState m_state;

    GameWorld m_world;

    public float m_dodgeSpeed;

    public float m_dodgeDistance;

    public float m_alertTurnVelocity;

    public float m_protectRadius;

    List<GameObject> m_protectingEnemies;


    new void Start()
    {
        base.Start();
        m_state = SquareEnemyState.Idle;

        m_world = FindObjectOfType<GameWorld>();

        EventManager.AddListener<Event_Player_Fire_Projectile>(OnPlayerProjectileFire);
        m_health.SetOnDeath(OnEnemyKilled);

        m_protectingEnemies = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        switch(m_state)
        {
            case SquareEnemyState.Alert:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_player.transform.position - transform.position), Time.deltaTime * m_alertTurnVelocity);
            break;

            case SquareEnemyState.Dodging:
            break;
        }

        UpdateState();
        UpdateProtection();
    }

    new void OnEnemyKilled()
    {
        // Clear the "protected" state of nearby enemies
        foreach (GameObject enemy in m_world.m_enemyList)
        {
            if (enemy != this.gameObject && enemy != null)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) <= m_protectRadius)
                {
                    enemy.GetComponent<BaseEnemyController>().SetProtected(false);
                }
            }
            
        }
        
        EventManager.RemoveListener<Event_Player_Fire_Projectile>(OnPlayerProjectileFire);

        base.OnEnemyKilled();
    }

    void UpdateState()
    { 
        float playerDistanceXZ = GetPlayerVectorXZ().magnitude;

        switch(m_state)
        {  
            case SquareEnemyState.Idle:
                if (playerDistanceXZ < m_alertRadius)
                {
                    ChangeState(SquareEnemyState.Alert);
                }
            break;

            case SquareEnemyState.Alert:

            break;

            case SquareEnemyState.Dodging:
                if (m_agent.remainingDistance < 0.1f)
                {
                    ChangeState(SquareEnemyState.Alert);
                }
            break;
        }
    }

    void ChangeState(SquareEnemyState newState)
    {
        //Debug.Log(gameObject.name +  " SM: " + m_state + " to " + newState);
        m_state = newState;
    }

    // Dodge player projectile
    void OnPlayerProjectileFire(Event_Player_Fire_Projectile evt)
    {
        if (m_state == SquareEnemyState.Alert)
        {
            RaycastHit hit;
            Physics.SphereCast(evt.m_transform.position, evt.m_radius, evt.m_transform.forward, out hit, m_alertRadius);
            if (hit.collider == GetComponent<Collider>())
            {
                ChangeState(SquareEnemyState.Dodging);
                m_agent.speed = m_dodgeSpeed;
                Vector3 dir = Random.Range(0, 2) == 0 ? transform.right : -1 * transform.right;
                m_agent.SetDestination(transform.position + dir * m_dodgeDistance);
            }
        }
    }

    void UpdateProtection()
    {
        // try to add all protecting enemies
        foreach (GameObject enemy in m_world.m_enemyList)
        {
            if (enemy && enemy != this.gameObject)
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) <= m_protectRadius && !m_protectingEnemies.Contains(enemy))
                {
                    enemy.GetComponent<BaseEnemyController>().SetProtected(true);
                    m_protectingEnemies.Add(enemy);
                }
            }
        }

        // remove protecting enemies that are far
        for (int i = 0; i < m_protectingEnemies.Count; i ++)
        {
            GameObject e = m_protectingEnemies[i];
            if (!e)
            {
                m_protectingEnemies.Remove(e);

            }
            else if (Vector3.Distance(e.transform.position, transform.position) > m_protectRadius)
            {
                m_protectingEnemies.Remove(e);
                e.GetComponent<BaseEnemyController>().SetProtected(false);
            }
        }
    }

}
