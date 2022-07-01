using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Death_Event{}

public class Health : NetworkBehaviour
{
    public float m_maxHealth;

    public bool m_isPlayer;


    [SyncVar]
    bool m_hasUI;

    [SyncVar]
    float m_curHealth;

    Action m_onDeath;

    public bool m_active;

    GameObject m_healthBarUI;

    GameObject m_player;

    [SyncVar]
    bool m_isProtected;

    private void Awake()
    {
        m_hasUI = false;
        m_curHealth = m_maxHealth;
        m_active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_active && m_curHealth <= 0)
        {
            m_onDeath.Invoke();
            m_active = false;
        }
        if (m_active && m_hasUI)
            UpdateHealthUI();
    }

    public void SetOnDeath(Action a)
    {
        m_onDeath = a;
    }

    public void TakeDamage(float damage)
    {
        if (isServer) 
        {
            m_curHealth -= damage;
        }
    }

    void UpdateHealthUI() 
    {
        Slider s = GetComponentInChildren<Slider>();
        if (s)
            s.value = m_curHealth / m_maxHealth;

        if (!m_isPlayer) 
        {
            // Point the world space health bar toward player
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<PlayerCharacterController>().isLocalPlayer)
                {
                    if (s)
                    {
                        s.transform.LookAt(player.transform);
                    }
                }
            }

            if (m_isProtected)
            {
                s.GetComponentsInChildren<Image>()[1].color = Color.blue;
            }
            else
            {
                s.GetComponentsInChildren<Image>()[1].color = Color.red;
            }
        }

        
    }

    [Command]
    public void CmdSetProtected(bool isProtected)
    {
        m_isProtected = isProtected;
    }

    public void InitializeUI() 
    {
        m_healthBarUI = GetComponentInChildren<Canvas>().gameObject;
        m_hasUI = true;
    }



}
