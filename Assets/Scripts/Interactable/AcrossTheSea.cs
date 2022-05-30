using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrossTheSea : Interactable
{
    bool m_hasInteracted = false;
    public override bool Interact()
    {
        if (m_playerController && !m_hasInteracted) 
        {
            m_playerController.gameObject.transform.position += new Vector3(8, 0, 0);
        }

        m_hasInteracted = true;
        return true;
    }
}
