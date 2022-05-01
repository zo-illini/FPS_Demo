using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
public class Sequence : Node
{
    public Sequence(BehaviorTree.Tree tree) : base(tree) {}
    public Sequence(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    public override NodeState Evaluate()
    {
        bool childRunning = false;

        foreach(Node child in m_children)
        {
            switch(child.Evaluate())
            {
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    childRunning = true;
                    continue;
                case NodeState.FAILTURE:
                    m_state = NodeState.FAILTURE;
                    return m_state;
            }
        }

        m_state = childRunning ? NodeState.RUNNING : NodeState.FAILTURE;
        return m_state;
    }
}
}

