using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskMoveToPlayer : Node
{
    GameObject m_self;
    GameObject m_player;

    float m_desiredDistance;

    public TaskMoveToPlayer(BehaviorTree.Tree tree) : base(tree) {}
    public TaskMoveToPlayer(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_player = (GameObject)m_tree.GetData("player");
        m_self = (GameObject)m_tree.GetData("self");
        m_desiredDistance = (float)m_tree.GetData("attack distance");
    }

    public override NodeState Evaluate()
    {
        NavMeshAgent agent = m_self.GetComponent<NavMeshAgent>();
        
        if (Vector3.Distance(m_self.transform.position, m_player.transform.position) < m_desiredDistance)
        {
            agent.isStopped = true;
            m_state = NodeState.SUCCESS;
            return m_state;
        }
        else
        {
            agent.SetDestination(m_player.transform.position);
            agent.isStopped = false;
            m_state = NodeState.RUNNING;
            return m_state;
        }
    }
}
