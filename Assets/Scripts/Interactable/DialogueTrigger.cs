using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : Interactable
{
    public List<string> m_dialogue;

    bool m_isShowingDialogue;

    GameWorld m_world;
    public override void Interact(Event_Player_Interact evt)
    {
        if (!m_world)
            m_world = FindObjectOfType<GameWorld>();
        if (!m_isShowingDialogue)
        {
            m_world.m_dialogueUI.StartDialogue(m_dialogue);
            m_isShowingDialogue = true;
        }
        else 
        {
            m_world.m_dialogueUI.NextLine();
        }
    }
}
