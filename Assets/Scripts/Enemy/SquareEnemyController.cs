using UnityEngine;
using UnityEngine.AI;

enum SquareEnemyState{Idle, Alert, Dodging};


public class SquareEnemyController : BaseEnemyController
{

    SquareEnemyState m_state;

    public float m_dodgeSpeed;

    public float m_dodgeDistance;

    public float m_alertTurnVelocity;


    new void Start()
    {
        base.Start();
        m_state = SquareEnemyState.Idle;

        EventManager.AddListener<Event_Player_Fire_Projectile>(OnPlayerProjectileFire);
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
    }

    new void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player Projectile")
        {
            m_health.TakeDamage(50);
            Destroy(collider.gameObject);
        }
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
        Debug.Log(gameObject.name +  " SM: " + m_state + " to " + newState);
        m_state = newState;
    }

    void OnPlayerProjectileFire(Event_Player_Fire_Projectile evt)
    {
        if (m_state == SquareEnemyState.Alert && evt.m_isSphere)
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

}
