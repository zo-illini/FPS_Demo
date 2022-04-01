using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_playerPrefab;

    public GameObject m_simpleEnemyPrefab;

    public GameObject m_triangleEnemyPrefab;

    public GameObject m_circleEnemyPrefab;

    public GameObject m_player{ get; private set; }

    public GameObject m_startMenuPrefab;

    public GameObject m_gameHUDPrefab;
    GameObject m_startMenu;

    GameObject m_gameHUD;

    Canvas m_canvas;

    Button m_startButton;

    GameObject m_tempCamera;

    int m_killCount;

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
        m_player = Instantiate(m_playerPrefab, Vector3.zero, Quaternion.identity);
    }

    void InitializeScene()
    {
        Instantiate(m_triangleEnemyPrefab, new Vector3(-15, 2, -10), Quaternion.identity);
        Instantiate(m_triangleEnemyPrefab, new Vector3(-10, 2, -20), Quaternion.identity);
        Instantiate(m_circleEnemyPrefab, new Vector3(-33, 2, -26), Quaternion.identity);

        // Deal with events
        EventManager.AddListener<Event_Enemy_Die>(OnEnemyKilled);
        EventManager.AddListener<Event_Win>(OnWin);

    }

    void OnEnemyKilled(Event_Enemy_Die evt)
    {
        m_gameHUD.GetComponentInChildren<Text>().text = "Enemy Killed: " + (++m_killCount).ToString();
        if (m_killCount == 3)
        {
            EventManager.Broadcast(Events.EventWin);
        }
        else
        {
            //Instantiate(m_triangleEnemyPrefab, new Vector3(10, 0, 10), Quaternion.identity);
        }

    }

    void OnWin(Event_Win evt)
    {
        m_gameHUD.GetComponentsInChildren<Text>()[1].color = Color.black;
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

