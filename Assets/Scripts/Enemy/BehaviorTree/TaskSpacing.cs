using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskSpacing : Node
{
    GameObject m_self;
    GameObject m_player;
    NavMeshAgent m_agent;

    float m_spacingDistance;
    float m_spacingSpeed;

    public TaskSpacing(BehaviorTree.Tree tree) : base(tree) {}
    public TaskSpacing(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_player = (GameObject)m_tree.GetData("player");
        m_self = (GameObject)m_tree.GetData("self");
        m_agent = m_self.GetComponent<NavMeshAgent>();

        m_spacingDistance = (float)m_tree.GetData("spacing distance");
        m_spacingSpeed = (float)m_tree.GetData("spacing speed");
    }

    public override NodeState Evaluate()
    {
        Vector2 playerVectorXZ = new Vector2(m_player.transform.position.x - m_self.transform.position.x, m_player.transform.position.z - m_self.transform.position.z);
        float playerDistanceXZ = playerVectorXZ.magnitude;

        if (playerDistanceXZ < m_spacingDistance)
        {
            Vector3 delta = -1 * (m_spacingDistance - playerDistanceXZ) * new Vector3(playerVectorXZ.x, 0, playerVectorXZ.y);
            m_agent.SetDestination(m_self.transform.position + delta);
            m_agent.speed = m_spacingSpeed;
            m_agent.isStopped = false;
        }
        m_self.transform.LookAt(m_player.transform);

        m_state = NodeState.RUNNING;
        return m_state;
    }
}
