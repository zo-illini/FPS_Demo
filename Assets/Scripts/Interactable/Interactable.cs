using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool m_closeToPlayer = false;
    // Start is called before the first frame update

    private void Start()
    {
        EventManager.AddListener<Event_Player_Interact>(OnInteract);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_closeToPlayer = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") 
            m_closeToPlayer = true;   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_closeToPlayer = false;
    }

    void OnInteract(Event_Player_Interact evt) 
    {
        if (m_closeToPlayer)
            Interact(evt);

    }

    virtual public void Interact(Event_Player_Interact evt) 
    {
        Debug.Log("Interact");
    }
}
