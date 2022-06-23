using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponUI : MonoBehaviour
{
    Text m_UIText;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<Event_Player_Switch_Weapon>(OnWeaponSwitch);
        m_UIText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnWeaponSwitch(Event_Player_Switch_Weapon evt) 
    {
        if (evt.m_newWeaponID == 0)
        {
            m_UIText.text = "当前使用的武器：远程";
        }
        else 
        {
            m_UIText.text = "当前使用的武器：近程";
        }
    }
}
