using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using BehaviorTree;

public class TaskCircleAttack : Node
{
    GameObject m_self;

    GameObject m_player;

    public TaskCircleAttack(BehaviorTree.Tree tree) : base(tree) {}
    public TaskCircleAttack(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_self = (GameObject)m_tree.GetData("self");
        m_player = (GameObject)m_tree.GetData("player");
    }


    public override NodeState Evaluate()
    {
        GameObject target = (GameObject)m_tree.GetData("moveTarget");
        if (target) 
        {
            m_self.GetComponent<WeaponController>().Shoot(target.transform.position - m_self.transform.position, 0);
        }

        m_state = NodeState.SUCCESS;
        return m_state;


    }
}
