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
    [SerializeField] private GameObject winMenuUI;
    [SerializeField] private GameObject gameOverMenuUI;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject zzzCatDialogue;

    public static event Action<bool> GamePausedEvent = delegate { };
    public static event Action<bool> GameStarted = delegate { };

    public static bool GameIsPaused = true;

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

        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        pauseButton.SetActive(false);

        if (firstLoad)
        {
            mainMenuUI.SetActive(true);
        }
        else
        {
            StartGame();
        }
    }

    private void OnEnable()
    {
        PlayerCat.PlayerDeath += GameOver;
        ZzzCatTrigger.ZzzCatDialogue += ShowZzzCatDialogue;
    }
    private void OnDisable()
    {
        DisableInputs(true);

        PlayerCat.PlayerDeath -= GameOver;
        ZzzCatTrigger.ZzzCatDialogue -= ShowZzzCatDialogue;
    }

    private void DisableInputs(bool disable)
    {
        if (disable)
        {
            inputActions.Disable();
        }
        else
        {
            inputActions.Enable();
        }
    }


    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        pauseButton.SetActive(true);

        DisableInputs(false);

        GameIsPaused = false;

        GameStarted.Invoke(true);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        zzzCatDialogue.SetActive(false);
        pauseButton.SetActive(true);

        Time.timeScale = 1f;

        GameIsPaused = false;
        GamePausedEvent.Invoke(false);
    }

    public void PauseGame()
    {
        AudioManager.Instance.PlayClip(AudioClipType.GamePaused);

        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);

        Time.timeScale = 0f;

        GameIsPaused = true;
        GamePausedEvent.Invoke(true);
    }

    public void RestartGame()
    {
        firstLoad = false;
        GameIsPaused = false;

        DisableInputs(false);

        Time.timeScale = 1f;

        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        pauseButton.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void GameOver()
    {
        gameOverMenuUI.SetActive(true);
        pauseButton.SetActive(false);

        DisableInputs(true);

        GameIsPaused = true;
    }

    private void Win()
    {
        winMenuUI.SetActive(true);
        pauseButton.SetActive(false);

        DisableInputs(true);

        Time.timeScale = 0f;

        GameIsPaused = true;
        GamePausedEvent.Invoke(true);
    }

    private void ShowZzzCatDialogue()
    {
        zzzCatDialogue.SetActive(true);
        pauseButton.SetActive(false);

        DisableInputs(true);

        Time.timeScale = 0f;

        GameIsPaused = true;
        GamePausedEvent.Invoke(true);
    }
}
