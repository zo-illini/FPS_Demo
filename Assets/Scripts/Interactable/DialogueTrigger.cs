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
            m_world.m_dialogueUI.StartDialogue(this);
            m_isShowingDialogue = true;
        }
        else 
        {
            m_world.m_dialogueUI.NextLine();
        }
    }

    public void OnDialogueFinished() 
    {
        m_isShowingDialogue = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            m_closeToPlayer = false;
            m_world.m_dialogueUI.FinishDialogue();
        }


    }
}
