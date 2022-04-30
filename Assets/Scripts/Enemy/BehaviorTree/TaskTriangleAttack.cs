using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskTriangleAttack : Node
{
    GameObject m_self;

    GameObject m_player;

    float m_dashSpeed;

    NavMeshAgent m_agent;
    bool m_isDashing;

    public TaskTriangleAttack(BehaviorTree.Tree tree) : base(tree) {}
    public TaskTriangleAttack(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_self = (GameObject)m_tree.GetData("self");
        m_player = (GameObject)m_tree.GetData("player");
        m_dashSpeed = (float)m_tree.GetData("dash attack speed");

        m_agent = m_self.GetComponent<NavMeshAgent>();
        m_isDashing = false;

    }


    public override NodeState Evaluate()
    {
        if (!m_isDashing)
        {
            Vector3 playerLookAt = m_player.transform.position - m_self.transform.position;
            Vector3 dashDestination = m_player.transform.position + new Vector3(playerLookAt.x, 0, playerLookAt.y).normalized;
            m_agent.SetDestination(dashDestination);
            m_agent.speed = m_dashSpeed;
            m_isDashing = true;
        }
        else
        {
            if (m_agent.remainingDistance < 0.1f)
            {
                m_isDashing = false;
            }
        }
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
