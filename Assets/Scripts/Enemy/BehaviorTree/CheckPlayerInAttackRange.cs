using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using BehaviorTree;

public class CheckPlayerInAttackRange: Node
{
    GameObject m_self;
    GameObject m_player;
    SyncListString m_playerNameList;
    float m_range;

    public CheckPlayerInAttackRange(BehaviorTree.Tree tree) : base(tree) {}
    public CheckPlayerInAttackRange(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_player = (GameObject)m_tree.GetData("player");
        m_playerNameList = (SyncListString)m_tree.GetData("playerNameList");
        m_self = (GameObject)m_tree.GetData("self");
        m_range = (float)m_tree.GetData("attack radius");
    }

    public override NodeState Evaluate()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) 
        {
            
            if (Vector3.Distance(m_self.transform.position, player.transform.position) < m_range)
            {
                m_tree.SetData("attackTarget", player);
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        /*
        
        if (m_player != null)
        {
            if (Vector3.Distance(m_self.transform.position, m_player.transform.position) < m_range)
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
            else
            {
                m_state = NodeState.FAILTURE;
                return m_state;
            }
        }
        */

        m_state = NodeState.FAILTURE;
        return m_state;
    }
}
