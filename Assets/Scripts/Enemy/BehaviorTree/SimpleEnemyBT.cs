using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class SimpleEnemyBT : BehaviorTree.Tree
{

    public float m_patrolRadius;
    public float m_alertRadius;
    public float m_attackRadius;

    public float m_patrolSpeed;

    protected override Node InitializeTree()
    {
        GameWorld world = FindObjectOfType<GameWorld>();

        SetData("player", world.m_player);
        SetData("self", this.gameObject);
        SetData("patrol speed", m_patrolSpeed);
        SetData("patrol center", this.transform.position);
        SetData("patrol radius", m_patrolRadius);
        SetData("alert radius", m_alertRadius);
        SetData("attack radius", m_attackRadius);

        Node root = new Selector(this, new List<Node>
        {
            new Sequence(this, new List<Node>
            {
                new CheckPlayerInFOV(this),
                new TaskMoveToPlayer(this),
            }),
            new TaskPatrol(this)
        });

        return root;
    }
}
