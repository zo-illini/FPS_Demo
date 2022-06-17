using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestGameWorld : NetworkBehaviour
{
    public GameObject TestAIPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            GameObject obj = Instantiate(TestAIPrefab);
            NetworkServer.Spawn(obj);
        }
    }
}
