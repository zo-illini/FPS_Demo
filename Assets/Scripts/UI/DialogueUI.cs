using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public List<string> m_dialogue;
    public int m_dialoguePtr;

    Text m_dialogueText;
    Image m_panel;

    public void InitializeDialogueUI() 
    {
        m_panel = GetComponent<Image>();
        m_dialogueText = GetComponentInChildren<Text>();
        
        // Default hide the panel
        Color c = m_panel.color;
        m_panel.color = new Color(c.r, c.g, c.b, 0);
    }

    public void StartDialogue() 
    {
        Color c = m_panel.color;
        m_panel.color = new Color(c.r, c.g, c.b, 0.5f);
        ShowDialogue();
    }

    private void ShowDialogue() 
    {
        if (m_dialoguePtr >= 0 && m_dialoguePtr < m_dialogue.Count) 
        {
            m_dialogueText.text = m_dialogue [m_dialoguePtr];
        }
    }



}
