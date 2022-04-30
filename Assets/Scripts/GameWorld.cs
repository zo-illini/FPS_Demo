using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_playerPrefab;

    public GameObject m_simpleEnemyPrefab;

    public GameObject m_triangleEnemyPrefab;

    public GameObject m_circleEnemyPrefab;

    public GameObject m_squareEnemyPrefab;

    public GameObject m_player{ get; private set; }

    public GameObject m_startMenuPrefab;

    public GameObject m_gameHUDPrefab;
    GameObject m_startMenu;

    GameObject m_gameHUD;

    Canvas m_canvas;

    Button m_startButton;

    GameObject m_tempCamera;

    int m_totalEnemyCount;

    int m_killCount;
    public List<GameObject> m_enemyList {get; private set;}

    void Start()
    {
        // Create Start Game Menu
        m_startMenu = Instantiate(m_startMenuPrefab);
        m_canvas = m_startMenu.GetComponentInChildren<Canvas>();
        m_startButton = m_startMenu.GetComponentInChildren<Button>();

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        m_canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        m_startButton.onClick.AddListener(StartGame);

        // Create a temporary camera
        m_tempCamera = new GameObject();
        m_tempCamera.AddComponent<Camera>();

        m_killCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        // Exit Start Menu
        Destroy(m_startMenu);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Destroy(m_tempCamera);

        // GameHUD
        m_gameHUD = Instantiate(m_gameHUDPrefab);

        // More Initialization
        InitializePlayer();
        InitializeScene();
    }

    void InitializePlayer()
    {
        m_player = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.Euler(0, 210, 0));
    }

    void InitializeScene()
    {
        m_enemyList = new List<GameObject>();

        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-15, 2, -10), Quaternion.identity));
        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-20, 2, -10), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_circleEnemyPrefab, new Vector3(-35, 2, -20), Quaternion.identity));
        m_enemyList.Add(Instantiate(m_circleEnemyPrefab, new Vector3(-50, 2, -20), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-60, 2, -40), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-50, 2, -40), Quaternion.identity));
        m_enemyList.Add(Instantiate(m_circleEnemyPrefab, new Vector3(-40, 2, -40), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_squareEnemyPrefab, new Vector3(-10, 2, -50), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-10, 2, -40), Quaternion.identity));
        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-10, 2, -60), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_squareEnemyPrefab, new Vector3(-35, 2, -68), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_circleEnemyPrefab, new Vector3(-40, 2, -63), Quaternion.identity));
        m_enemyList.Add(Instantiate(m_circleEnemyPrefab, new Vector3(-40, 2, -57), Quaternion.identity));

        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-35, 2, -63), Quaternion.identity));
        m_enemyList.Add(Instantiate(m_triangleEnemyPrefab, new Vector3(-35, 2, -57), Quaternion.identity));

        m_totalEnemyCount = 15;


        // Deal with events
        EventManager.AddListener<Event_Enemy_Die>(OnEnemyKilled);
        EventManager.AddListener<Event_Win>(OnWin);
        EventManager.AddListener<Event_Player_Die>(OnPlyaerDie);

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
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}

