using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrossTheSea : Interactable
{
    bool m_hasInteracted = false;
    GameObject Player;
    public override void Interact(Event_Player_Interact evt)
    {
        if (!m_hasInteracted) 
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) 
            {
                if (player.GetComponent<PlayerCharacterController>().isLocalPlayer) 
                {
                    player.GetComponent<PlayerCharacterController>().CmdTeleportPlayer(player.transform.position + new Vector3(8, 0, 0), player.transform.rotation);
                }
            }
        }

        m_hasInteracted = true;
    }
}
