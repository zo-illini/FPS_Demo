using UnityEngine;

public class GameWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_playerPrefab;

    public GameObject m_player{ get; private set; }

    void Start()
    {
        InitializePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializePlayer()
    {
        m_player = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
    }
}

