using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.Networking;

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

        m_world = FindObjectOfType<GameWorld>();

        EventManager.AddListener<Event_Player_Fire_Projectile>(OnPlayerProjectileFire);
        m_health.SetOnDeath(OnEnemyKilled);

        m_protectingEnemies = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateProtection();
    }


    new void OnEnemyKilled()
    {
        if (!isServer)
            return;

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

    // Dodge player projectile
    void OnPlayerProjectileFire(Event_Player_Fire_Projectile evt)
    {
        GetComponent<SquareEnemyBT>().SetData("dodge event", evt);
    }
    void UpdateProtection()
    {
        if (!isServer)
            return;

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
