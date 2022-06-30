using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestSpawnEnemy : NetworkBehaviour
{
    public GameObject m_TestEnemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (isServer) 
        {
            CmdSpawn();
        }
    }


    [Command]
    public void CmdSpawn()
    {
        GameObject obj = Instantiate(m_TestEnemyPrefab, new Vector3(0, 0, 17), Quaternion.identity);
        obj.GetComponent<CircleEnemyBT>().m_patrolSpeed = 0;
        NetworkServer.Spawn(obj);
    }
}
