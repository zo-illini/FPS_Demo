using System.Collections;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeState
    {
        SUCCESS,
        RUNNING,
        FAILTURE
    }
    public class Node
    {
        protected NodeState m_state;

        public Node m_parent;

        public List<Node> m_children = new List<Node>();

        protected Tree m_tree;

        //private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node(Tree tree)
        {
            m_parent = null;
            m_tree = tree;
            InitializeNode();
        }

        public Node(Tree tree, List<Node> children)
        {
            foreach (Node n in children)
            {
                _Attach(n);
            }
            m_tree = tree;
            InitializeNode();
        }

        private void _Attach(Node node)
        {
            node.m_parent = this;
            m_children.Add(node);
        }

        protected virtual void InitializeNode()
        {

        }

        public virtual NodeState Evaluate()
        {
            return NodeState.FAILTURE;
        }

        /*
        public void SetData(string key, object data)
        {
            _dataContext[key] = data;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
              return value;
            
            Node node = m_parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }
                else
                {
                    node = node.m_parent;
                }
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            
            Node node = m_parent;
            while (node != null)
            {
                if (node.ClearData(key))
                {
                    return true;
                }
                else
                {
                    node = node.m_parent;
                }
            }

            return false;
        }
        */

    }
}


