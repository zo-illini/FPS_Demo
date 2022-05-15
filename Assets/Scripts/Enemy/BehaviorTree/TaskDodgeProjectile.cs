using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskDodgeProjectile : Node
{
    GameObject m_self;
    GameObject m_player;
    NavMeshAgent m_agent;
    float m_dodgeSpeed;
    float m_dodgeDistance;
    private bool m_isDodging;

    public TaskDodgeProjectile(BehaviorTree.Tree tree) : base(tree) {}
    public TaskDodgeProjectile(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_player = (GameObject)m_tree.GetData("player");
        m_self = (GameObject)m_tree.GetData("self");
        m_agent = m_self.GetComponent<NavMeshAgent>();
        m_dodgeSpeed = (float)m_tree.GetData("dodge speed");
        m_dodgeDistance = (float)m_tree.GetData("dodge distance");

        m_isDodging = false;
    }

    public override NodeState Evaluate()
    {
        if (!m_isDodging)
        {
            RaycastHit hit;
            Event_Player_Fire_Projectile evt = (Event_Player_Fire_Projectile)m_tree.GetData("dodge event");
            if (evt != null && evt.m_transform != null)
            {
                Physics.SphereCast(evt.m_transform.position, evt.m_radius, evt.m_transform.forward, out hit, (float)m_tree.GetData("alert radius"));
                if (hit.collider == m_self.GetComponent<Collider>())
                {
                    Vector3 dir = Random.Range(0, 2) == 0 ? m_self.transform.right : -1 * m_self.transform.right;
                    m_agent.SetDestination(m_self.transform.position + dir * m_dodgeDistance);
                    m_agent.speed = m_dodgeSpeed;
                    m_agent.isStopped = false;
                    m_isDodging = true;
                }
                m_tree.SetData("dodge event", null);
            }
        }
        else
        {
            if (m_agent.remainingDistance < 0.1f)
            {
                m_agent.isStopped = true;
                m_state = NodeState.SUCCESS;
                m_isDodging = false;
                return m_state;
            }
        }
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
