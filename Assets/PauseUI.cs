using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(bool pause) 
    {
        if (pause)
        {
            GetComponent<Text>().text = "游戏暂停";
        }
        else 
        {
            GetComponent<Text>().text = "";
        }
    }
}
