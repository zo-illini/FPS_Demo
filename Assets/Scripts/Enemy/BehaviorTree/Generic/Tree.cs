using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
public abstract class Tree : MonoBehaviour
{
    private Node _root;

    protected Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    protected void Start()
    {
        _root = InitializeTree();
    }

    private void Update()
    {
        if (_root != null)
            _root.Evaluate();
    }

    protected abstract Node InitializeTree();

    public void SetData(string key, object data)
    {
        _dataContext[key] = data;
    }

    public object GetData(string key)
    {
        object ret = null;
        _dataContext.TryGetValue(key, out ret);
        return ret;
    }

    public void ClearData(string key)
    {
        if (_dataContext.ContainsKey(key))
            _dataContext.Remove(key);
    }

}
}

