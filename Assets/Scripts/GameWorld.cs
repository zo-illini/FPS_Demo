using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

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
    GameObject m_startMenu;

    GameObject m_gameHUD;

    MissionUI m_missionUI;

    public DialogueUI m_dialogueUI;

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

        // Create Mission UI
        m_missionUI = GetComponentInChildren<Text>().gameObject.AddComponent<MissionUI>();

        // Creaate Dialogue UI
        //m_dialogueUI = GetComponentInChildren<Image>().gameObject.AddComponent<DialogueUI>();
        m_dialogueUI = GetComponentInChildren<DialogueUI>();

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
        //InitializePlayer();
        InitializeScene();
        m_missionUI.InitializeMainUI(m_enemyList.Count);
        m_dialogueUI.InitializeDialogueUI();

    }

    void InitializePlayer()
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        if (m_playerStart) 
        {
            pos = m_playerStart.transform.position;
            rot = m_playerStart.transform.rotation;
        }   
        m_player = Instantiate(m_playerPrefab, pos, rot);
    }

    void InitializeScene()
    {
        InitializeEnemies();
        // Deal with events
        EventManager.AddListener<Event_Enemy_Die>(OnEnemyKilled);
        EventManager.AddListener<Event_Win>(OnWin);
        EventManager.AddListener<Event_Player_Die>(OnPlyaerDie);

    }

    void InitializeEnemies() 
    {
        m_enemyList = new List<GameObject>();
        foreach (EnemySpawn obj in FindObjectsOfType<EnemySpawn>()) 
        {
            CreateEnemy(obj.type, obj.transform);
        }
    }

    /*
    private void InitializeEnemies()
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
    
    }
    */

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
        m_enemyList.Add(ret);
        m_totalEnemyCount += 1;
        return ret;
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
}

