using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MissionUI : MonoBehaviour
{
    Text m_missionText;
    
    int m_remainingEnemyCount;

    public void InitializeMainUI(int EnemyCount) 
    {
        m_remainingEnemyCount = EnemyCount;
        m_missionText = GetComponent<Text>();
        m_missionText.text = "任务目标: 再击杀" + m_remainingEnemyCount.ToString() + "个敌人";

        EventManager.AddListener<Event_Enemy_Die>(OnEnemyKilled);
    }

    void OnEnemyKilled(Event_Enemy_Die evt) 
    {
        m_remainingEnemyCount -= 1;
        if (m_remainingEnemyCount != 0)
        {
            m_missionText.text = "任务目标: 再击杀" + m_remainingEnemyCount.ToString() + "个敌人";
        }
        else 
        {
            m_missionText.text = "任务完成";
        }
    }

    // For local client update
    private void Update()
    {
        /*
        if (m_missionText) 
        {
            if (m_remainingEnemyCount != 0)
            {
                m_missionText.text = "任务目标: 再击杀" + m_remainingEnemyCount.ToString() + "个敌人";
            }
            else
            {
                m_missionText.text = "任务完成";
            }
        }
        */
    }




}
