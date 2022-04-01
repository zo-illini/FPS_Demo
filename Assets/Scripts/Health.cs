using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Death_Event{}

public class Health : MonoBehaviour
{
    public float m_maxHealth;

    public bool m_hasUI;

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
            m_player = FindObjectOfType<GameWorld>().m_player;
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

        // Point the world space health bar toward player
        if (m_hasUI)
        {
            m_healthBarUI.transform.LookAt(m_player.transform);
        }
    }

    public void SetOnDeath(Action a)
    {
        m_onDeath = a;
    }

    public void TakeDamage(float damage)
    {
        m_curHealth -= damage;
        if (m_hasUI)
        {
            m_healthBarUI.GetComponentInChildren<Slider>().value = m_curHealth / m_maxHealth;
        }
    }



}
