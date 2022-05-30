using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    protected bool m_isInteracting = false;

    protected PlayerCharacterController m_playerController;
    // Start is called before the first frame update

    void OnTriggerStay(Collider other)
    {
        if (!m_isInteracting && other.gameObject.tag == "Player") 
        {
            if (!m_playerController)
            {
                m_playerController = other.gameObject.GetComponent<PlayerCharacterController>();
            }
            m_playerController.InteractableCallBack(this);
            
        }
    }

    virtual public bool Interact() 
    {
        Debug.Log("Interact");
        return !m_isInteracting;
    }
}
