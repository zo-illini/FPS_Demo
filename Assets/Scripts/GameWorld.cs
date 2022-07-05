using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Networking;

public enum EnemyType {Triangle, Circle, Square };

public class GameWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_playerStart;

    public GameObject m_playerPrefab;

    public GameObject m_simpleEnemyPrefab;

    public GameObject m_triangleEnemyPrefab;

    public GameObject m_circleEnemyPrefab;

    public GameObject m_squareEnemyPrefab;

    public GameObject m_player{ get; private set; }

    public GameObject m_startMenuPrefab;

    public GameObject m_gameHUDPrefab;

    public GameObject m_mainUI;
    
    GameObject m_startMenu;

    GameObject m_gameHUD;

    NetworkManager manager;

    MissionUI m_missionUI;

    public DialogueUI m_dialogueUI;

    PauseUI m_pauseUI;

    Canvas m_canvas;

    Button m_startButtonServer;

    Button m_startButtonClient;

    GameObject m_tempCamera;

    int m_totalEnemyCount;

    int m_killCount;

    bool m_isGamePaused;

    public List<GameObject> m_enemyList {get; private set;}

    void Awake()
    {
        // Create Start Game Menu
        m_startMenu = Instantiate(m_startMenuPrefab);
        m_canvas = m_startMenu.GetComponentInChildren<Canvas>();
        m_startButtonServer = m_startMenu.GetComponentsInChildren<Button>()[0];
        m_startButtonClient = m_startMenu.GetComponentsInChildren<Button>()[1];


        manager = GetComponent<NetworkManager>();

        m_canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        m_startButtonServer.onClick.AddListener(StartGameServer);
        m_startButtonClient.onClick.AddListener(StartGameClient);


        // Create a temporary camera
        m_tempCamera = new GameObject();
        m_tempCamera.AddComponent<Camera>();

        // Create Mission UI
        //m_missionUI = m_mainUI.GetComponentInChildren<Text>().gameObject.AddComponent<MissionUI>();
        m_missionUI = m_mainUI.GetComponentInChildren<MissionUI>();

        // Creaate Dialogue UI
        //m_dialogueUI = GetComponentInChildren<Image>().gameObject.AddComponent<DialogueUI>();
        m_dialogueUI = m_mainUI.GetComponentInChildren<DialogueUI>();
        m_pauseUI = m_mainUI.GetComponentInChildren<PauseUI>();

        m_killCount = 0;

        m_isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGameServer() 
    {
        manager.StartHost();
        StartCoroutine(StartGame(true));
    }

    void StartGameClient()
    {
        manager.StartClient();
        StartCoroutine(StartGame(false));
    }

    IEnumerator StartGame(bool isHost)
    {
        //暂时使用延时避开NetworkManager中创建Player的时序无法插手的问题
        yield return new WaitForSeconds(0.2f);
        // Exit Start Menu
        Destroy(m_startMenu);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Destroy(m_tempCamera);

        // GameHUD
        m_gameHUD = Instantiate(m_gameHUDPrefab);

        // More Initialization
        LocalInitializePlayer();
        if (isHost)
        {
            InitializeEnemies();
        }
        else 
        {
            LocalInitializeEnemies();
        }
        m_missionUI.InitializeMainUI(m_totalEnemyCount);
        m_dialogueUI.InitializeDialogueUI();
            
        // Deal with events
        EventManager.AddListener<Event_Enemy_Die>(OnEnemyKilled);
        EventManager.AddListener<Event_Win>(OnWin);
        EventManager.AddListener<Event_Player_Die>(OnPlyaerDie);
        

    }

    void LocalInitializePlayer() 
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetComponent<PlayerCharacterController>().isLocalPlayer)
            {
                m_player = player;
                if (m_playerStart)
                    player.GetComponent<PlayerCharacterController>().CmdTeleportPlayer(m_playerStart.transform.position, m_playerStart.transform.rotation);
                // Show player health bar
                m_player.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                m_player.GetComponentInChildren<Health>().InitializeUI();

            }
        }
    }

    void InitializeEnemies() 
    {
        m_enemyList = new List<GameObject>();
        foreach (EnemySpawn obj in FindObjectsOfType<EnemySpawn>()) 
        {
            CreateEnemy(obj.type, obj.transform);
        }
    }

    public GameObject CreateEnemy(EnemyType type, Transform transform) 
    {
        GameObject ret;
        switch (type) 
        {
            case EnemyType.Triangle:
                ret = Instantiate(m_triangleEnemyPrefab, transform.position, transform.rotation);
                break;
            case EnemyType.Circle:
                ret = Instantiate(m_circleEnemyPrefab, transform.position, transform.rotation);
                break;
            case EnemyType.Square:
                ret = Instantiate(m_squareEnemyPrefab, transform.position, transform.rotation);
                break;
            default:
                ret = null;
                break;
        }

        ret.GetComponent<Health>().InitializeUI();

        m_enemyList.Add(ret);
        m_totalEnemyCount += 1;
        NetworkServer.Spawn(ret);
        return ret;
    }

    void LocalInitializeEnemies() 
    {
        m_enemyList = new List<GameObject>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) 
        {
            m_enemyList.Add(enemy);
            enemy.GetComponent<BaseEnemyController>().InitializeOnDeath();
            m_totalEnemyCount += 1;
        }
    }

    void OnEnemyKilled(Event_Enemy_Die evt)
    {
        m_gameHUD.GetComponentInChildren<Text>().text = "Enemy Killed: " + (++m_killCount).ToString();
        if (m_killCount == m_totalEnemyCount)
        {
            EventManager.Broadcast(Events.EventWin);
        }
        
        for (int i = 0; i < m_enemyList.Count; i ++)
        {
            if (m_enemyList[i] == evt.m_enemy)
            {
                m_enemyList[i] = null;
                break;
            }
        }

    }

    void OnWin(Event_Win evt)
    {
        m_gameHUD.GetComponentsInChildren<Text>()[1].color = Color.black;
        Time.timeScale = 0;
        StartCoroutine(WaitToEndGame());
    }

    void OnPlyaerDie(Event_Player_Die evt)
    {
        m_gameHUD.GetComponentsInChildren<Text>()[2].color = Color.black;
        Time.timeScale = 0;
        StartCoroutine(WaitToEndGame());
    }

    IEnumerator WaitToEndGame()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        if (Application.isEditor)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }

    public void PauseGame() 
    {
        if (NetworkServer.connections.Count != 1)
            return;

        m_isGamePaused = !m_isGamePaused;

        Time.timeScale = m_isGamePaused ? 0 : 1;
        m_pauseUI.UpdateUI(m_isGamePaused);

    }
}

