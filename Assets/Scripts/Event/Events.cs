using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events 
{
    public static Event_Enemy_Die EventEnemyDie = new Event_Enemy_Die();

    public static Event_Win EventWin = new Event_Win();

    public static Event_Player_Die EventPlayerDie= new Event_Player_Die();

    public static Event_Player_Fire_Projectile EventPlayerFireProjectile= new Event_Player_Fire_Projectile();

    public static Event_Player_Interact EventPlayerInteract = new Event_Player_Interact();


};

public class Event_Enemy_Die : GameEvent 
{
    public GameObject m_enemy;
};

public class Event_Win : GameEvent {};

public class Event_Player_Die : GameEvent {};

public class Event_Player_Fire_Projectile : GameEvent 
{
    public Transform m_transform;

    public float m_radius;
    
    public bool m_isSphere;
};

public class Event_Player_Interact : GameEvent { };
