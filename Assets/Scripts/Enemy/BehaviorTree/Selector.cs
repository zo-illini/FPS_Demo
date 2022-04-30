using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
public class Selector : Node
{
    public Selector(BehaviorTree.Tree tree) : base(tree) {}
    public Selector(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    public override NodeState Evaluate()
    {
        foreach(Node child in m_children)
        {
            switch(child.Evaluate())
            {
                case NodeState.SUCCESS:
                    m_state = NodeState.SUCCESS;
                    return m_state;
                case NodeState.RUNNING:
                    m_state = NodeState.RUNNING;
                    return m_state;
                case NodeState.FAILTURE:
                    continue;
            }
        }

        m_state = NodeState.FAILTURE;
        return m_state;
    }
}
}

