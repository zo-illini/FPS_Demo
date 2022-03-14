using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events 
{
    public static Event_Enemy_Die EventEnemyDie = new Event_Enemy_Die();


};

public class Event_Enemy_Die : GameEvent {};
