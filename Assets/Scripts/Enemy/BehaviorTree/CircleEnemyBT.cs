using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CircleEnemyBT : BehaviorTree.Tree
{

    public float m_patrolRadius;
    public float m_patrolSpeed;
    public float m_alertRadius;
    public float m_spacingDistance;
    public float m_spacingSpeed;

    protected override Node InitializeTree()
    {
        GameWorld world = FindObjectOfType<GameWorld>();

        //SetData("player", world.m_player);
        SetData("self", this.gameObject);
        SetData("patrol speed", m_patrolSpeed);
        SetData("patrol center", this.transform.position);
        SetData("patrol radius", m_patrolRadius);
        SetData("alert radius", m_alertRadius);
        SetData("spacing distance", m_spacingDistance);
        SetData("spacing speed", m_spacingSpeed);

        Node root = new Selector(this, new List<Node>
        {
            new Sequence(this, new List<Node>
            {
                new CheckPlayerInFOV(this),
                new TaskSpacing(this),
                new TaskCircleAttack(this),
            }),
            new TaskPatrol(this)
        });

        return root;
    }
}
