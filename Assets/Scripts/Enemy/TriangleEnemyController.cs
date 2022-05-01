using UnityEngine;
using UnityEngine.AI;

enum TriangleEnemyState{Patrolling, Approaching, Attacking};


public class TriangleEnemyController : BaseEnemyController
{
    public float m_approachSpeed;

    public float m_dashSpeed;

    public float m_dashOverDistance;

    TriangleEnemyState m_state;


    new void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        // Damage player if touched
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerCharacterController>().TakeDamage(10f);
        }
    }

}
