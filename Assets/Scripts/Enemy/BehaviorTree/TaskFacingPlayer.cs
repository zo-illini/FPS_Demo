using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskFacingPlayer : Node
{
    GameObject m_self;
    GameObject m_player;

    float m_alertTurnVelocity;

    public TaskFacingPlayer(BehaviorTree.Tree tree) : base(tree) {}
    public TaskFacingPlayer(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_player = (GameObject)m_tree.GetData("player");
        m_self = (GameObject)m_tree.GetData("self");
        m_alertTurnVelocity = (float)m_tree.GetData("alert turn speed");
    }

    public override NodeState Evaluate()
    {
        m_self.transform.rotation = Quaternion.RotateTowards(m_self.transform.rotation, Quaternion.LookRotation(m_player.transform.position - m_self.transform.position), Time.deltaTime * m_alertTurnVelocity);
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
