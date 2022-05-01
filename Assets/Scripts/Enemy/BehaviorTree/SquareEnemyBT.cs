using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class SquareEnemyBT : BehaviorTree.Tree
{

    public float m_alertRadius;
    public float m_alertTurnSpeed;
    public float m_dodgeSpeed;
    public float m_dodgeDistance;

    protected override Node InitializeTree()
    {
        GameWorld world = FindObjectOfType<GameWorld>();

        SetData("player", world.m_player);
        SetData("self", this.gameObject);
        SetData("alert radius", m_alertRadius);
        SetData("dodge speed", m_dodgeSpeed);
        SetData("dodge distance", m_dodgeDistance);
        SetData("alert turn speed", m_alertTurnSpeed);
        SetData("dodge event", null);

        Node root = new Sequence(this, new List<Node>
            {
                new CheckPlayerInFOV(this),
                new TaskFacingPlayer(this),
                new TaskDodgeProjectile(this),
            });

        EventManager.AddListener<Event_Player_Fire_Projectile>(OnPlayerProjectileFire);
        return root;
    }

    void OnPlayerProjectileFire(Event_Player_Fire_Projectile evt)
    {
        SetData("dodge event", evt);
    }
}
