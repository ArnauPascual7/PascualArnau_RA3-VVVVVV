using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, InputSystem_Actions.IMenuActions
{
    private InputSystem_Actions inputActions;

    public static UI Instance { get; private set; }

    [Header("Elements UI")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private GameObject pauseButton;

    public static event Action<bool> GamePausedEvent = delegate { };
    public static event Action<bool> GameStarted = delegate { };

    public static bool GameIsPaused = true;
    private bool gameIsStarted = false;
    private bool gameIsOver = false;

    private bool firstLoad = true;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Menu.SetCallbacks(this);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        GameStarted.Invoke(false);

        if (firstLoad)
        {
            mainMenuUI.SetActive(true);
        }
        else
        {
            StartGame();
        }
        
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        pauseButton.SetActive(false);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        PlayerCat.PlayerDeath += GameOver;
    }
    private void OnDisable()
    {
        inputActions.Disable();
        PlayerCat.PlayerDeath -= GameOver;
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        pauseButton.SetActive(true);

        GameIsPaused = false;
        gameIsStarted = true;

        GameStarted.Invoke(true);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);

        Time.timeScale = 1f;

        GameIsPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);

        Time.timeScale = 0f;

        GameIsPaused = true;
    }

    public void RestartGame()
    {
        firstLoad = false;

        Time.timeScale = 1f;

        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed && gameIsStarted && !gameIsOver)
        {
            if (GameIsPaused)
            {
                ResumeGame();
                GamePausedEvent.Invoke(false);
            }
            else
            {
                GamePausedEvent.Invoke(true);
                PauseGame();
            }
        }
    }

    private void GameOver()
    {
        gameOverMenuUI.SetActive(true);
        pauseButton.SetActive(false);

        GameIsPaused = true;
        gameIsOver = true;
    }
}
