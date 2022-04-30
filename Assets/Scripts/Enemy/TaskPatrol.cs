using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


using BehaviorTree;
public class TaskPatrol : Node
{
    GameObject m_object;

    NavMeshAgent m_agent;
    private Vector3 m_patrolCenter;
    private float m_patrolRadius;
    private Vector3 m_patrolDestination;

    private float m_patrolSpeed;
    
    public TaskPatrol(BehaviorTree.Tree tree) : base(tree) {}
    public TaskPatrol(BehaviorTree.Tree tree, List<Node> children) : base(tree, children) {}

    protected override void InitializeNode()
    {
        m_object = (GameObject)m_tree.GetData("self");
        m_patrolCenter = (Vector3)m_tree.GetData("patrol center");
        m_patrolRadius = (float)m_tree.GetData("patrol radius");
        m_patrolSpeed = (float)m_tree.GetData("patrol speed");
        m_patrolDestination = GetRandomDestination();

        m_agent = m_object.GetComponent<NavMeshAgent>();
        
    }

    public override NodeState Evaluate()
    {

        Vector2 posXZ = new Vector2(m_object.transform.position.x, m_object.transform.position.z);
        if ((posXZ - new Vector2(m_patrolDestination.x, m_patrolDestination.z)).magnitude < 0.5f)
        {
            m_patrolDestination = GetRandomDestination();
        }
        else
        {
            m_agent.SetDestination(m_patrolDestination);
            m_agent.speed = m_patrolSpeed;
            m_agent.isStopped = false;
        }

        m_state = NodeState.RUNNING;
        return m_state;
    }

    private Vector3 GetRandomDestination()
    {
        Vector2 newPosXZ = new Vector2(m_patrolCenter.x, m_patrolCenter.z) + Random.insideUnitCircle * m_patrolRadius;
        Vector3 newPos = new Vector3(newPosXZ.x, m_patrolCenter.y, newPosXZ.y);

        return newPos;
    }

}
