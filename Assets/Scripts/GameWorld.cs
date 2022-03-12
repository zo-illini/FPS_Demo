using UnityEngine;
using UnityEngine.UI;

public class GameWorld : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_playerPrefab;

    public GameObject m_simpleEnemyPrefab;

    public GameObject m_player{ get; private set; }

    public GameObject m_startMenuPrefab;

    public GameObject m_gameHUDPrefab;
    GameObject m_startMenu;

    Canvas m_canvas;

    Button m_startButton;

    GameObject m_tempCamera;

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
        Instantiate(m_gameHUDPrefab);

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
        Instantiate(m_simpleEnemyPrefab, new Vector3(10, 0, 10), Quaternion.identity);
    }
}

