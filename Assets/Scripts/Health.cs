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

    public bool m_hasUI;

    [SyncVar]
    float m_curHealth;

    Action m_onDeath;

    bool m_active;

    GameObject m_healthBarUI;

    GameObject m_player;

    // Start is called before the first frame update
    void Start()
    {
        m_curHealth = m_maxHealth;
        m_active = true;
        if (m_hasUI)
        {
            m_healthBarUI = GetComponentInChildren<Canvas>().gameObject;
            GameWorld world = FindObjectOfType<GameWorld>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_active && m_curHealth <= 0)
        {
            m_onDeath.Invoke();
            m_active = false;
        }

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
        if (m_hasUI)
        {
            Slider s = GetComponentInChildren<Slider>();
            if (s)
                s.value = m_curHealth / m_maxHealth;

            // Point the world space health bar toward player
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (player.GetComponent<PlayerCharacterController>().isLocalPlayer)
                {
                    m_healthBarUI.transform.LookAt(player.transform);
                }
            }
        }
    }

    public void SetProtected(bool isProtected)
    {
        if (isProtected)
        {
            m_healthBarUI.GetComponentsInChildren<Image>()[1].color = Color.blue;
        }
        else
        {
            m_healthBarUI.GetComponentsInChildren<Image>()[1].color = Color.red;
        }
    }



}
