using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    List<string> m_dialogue;
    int m_dialoguePtr;

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

    public void StartDialogue(List<string> dialogue) 
    {
        Color c = m_panel.color;
        m_panel.color = new Color(c.r, c.g, c.b, 0.5f);
        m_dialogue = dialogue;
        ShowDialogue();
    }

    public void NextLine() 
    {
        m_dialoguePtr = Mathf.Clamp(m_dialoguePtr + 1, 0, m_dialogue.Count);
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
