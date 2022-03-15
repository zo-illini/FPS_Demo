using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events 
{
    public static Event_Enemy_Die EventEnemyDie = new Event_Enemy_Die();

    public static Event_Win EventWin = new Event_Win();



};

public class Event_Enemy_Die : GameEvent {};

public class Event_Win : GameEvent {};
